using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nemesys.Models;
using System.Net;
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<AccountController> logger)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string code)
    {
        if (userId == null || code == null)
        {
            return RedirectToAction("Index", "Home");
        }

        // Decode the URL-encoded code
        code = WebUtility.UrlDecode(code);

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);
        if (!result.Succeeded)
        {
            // Log the specific errors
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError($"Email confirmation failed for user {userId}: {errors}");

            // For debugging, show the errors
            ViewBag.ErrorMessage = errors;
            return View("Error");
        }
        // Redirect to your custom confirmation page
        return RedirectToPage("/Account/ConfirmEmail", new
        {
            area = "Identity",
        });
    }
    }
