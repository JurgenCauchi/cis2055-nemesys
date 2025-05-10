using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nemesys.Data;
using Nemesys.Models;
using Nemesys.Models.ViewModels;
using Nemesys.Repositories;
using Nemesys.Repositories.Interfaces;

namespace Nemesys.Controllers
{
    public class InvestigationController : Controller
    {
        private readonly NemesysContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IInvestigationRepository _investigationRepository { get; set; }
        public InvestigationController(NemesysContext context, IInvestigationRepository investigationRepository, UserManager<AppUser> userManager, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _context = context;
            _userManager = userManager;
            _investigationRepository = investigationRepository ?? throw new ArgumentNullException(nameof(investigationRepository));
        }

        // GET: InvestigationViewModels
        public async Task<IActionResult> Index()
        {
            var investigations = _investigationRepository.GetAllInvestigations();


            var model = new InvestigationListViewModel
            {
                TotalEntries = investigations.Count(),
                Investigations = investigations
                   .OrderByDescending(b => b.CreatedDate)
                   .Select(b => new InvestigationViewModel
                   {
                       Id = b.Id,
                       CreatedDate = b.CreatedDate,
                       Description = b.Description,
                       Report = b.Report , // Ensure non-null assignment  
                       ReportId = b.ReportId,
                       ReportStatus = new ReportStatusViewModel
                       {
                           Id = b.ReportStatus?.Id ?? 0, // Handle potential null ReportStatus  
                           Name = b.ReportStatus?.Name ?? "Unknown"
                       },
                       Author = new AuthorViewModel
                       {
                           Id = b.UserId,
                           Name = b.User?.UserName ?? "Anonymous"
                       },
                       LoggedInUserId = _userManager.GetUserId(User)
                   })
                   .ToList()
            };

            return View(model); // Pass the correct model type
        }

        // GET: InvestigationViewModels/Details/5
        public IActionResult Details(int id)
        {
            var investigation = _context.Investigations
                .Include(i => i.Report)
                    .ThenInclude(r => r.User)
                .Include(i => i.ReportStatus)
                .FirstOrDefault(i => i.Id == id);


            if (investigation == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);

            var model = new InvestigationViewModel()
            {
                Id = investigation.Id,
                CreatedDate = investigation.CreatedDate,
                Description = investigation.Description,
                Report = investigation.Report ,
                ReportStatus = new ReportStatusViewModel()
                {
                    Id = investigation.ReportStatus.Id,
                    Name = investigation.ReportStatus.Name
                },
                Author = new AuthorViewModel()
                {
                    Id = investigation.Report.UserId,
                    Name = investigation.Report.User != null ? investigation.User.UserName : "Anonymous"
                },
                LoggedInUserId = userId
            };

            return View(model);
        }




        [HttpGet]
        public IActionResult Create()
        {
            
                var viewModel = new EditInvestigationViewModel
                {
                    // Available reports without investigations
                    AvailableReports = _context.ReportPosts
                        .Where(r => !_context.Investigations.Any(i => i.ReportId == r.Id))
                        .Select(r => new SelectListItem
                        {
                            Value = r.Id.ToString(),
                            Text = $"Report #{r.Id} - {r.Title}"
                        })
                        .ToList(),

                    // Initialize ReportStatusList with all possible statuses
                    ReportStatusList = _context.ReportStatuses
                        .Select(s => new ReportStatusViewModel
                        {
                            Id = s.Id,
                            Name = s.Name
                        })
                        .ToList(),


                };

                // Ensure lists are never null
                viewModel.AvailableReports ??= new List<SelectListItem>();
                viewModel.ReportStatusList ??= new List<ReportStatusViewModel>();

                return View(viewModel);
            

        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("ReportId, Description, ReportStatusId,Title")] EditInvestigationViewModel newInvestigation)
        {
            if (ModelState.IsValid)
            {
                Investigation investigation = new Investigation()
                {
                    ReportId = newInvestigation.ReportId,
                    Description = newInvestigation.Description,
                    CreatedDate = DateTime.UtcNow,
                    ReportStatusId = newInvestigation.ReportStatusId,
                    UserId = _userManager.GetUserId(User),
                };

                _context.Investigations.Add(investigation);

                var report = _context.ReportPosts
                    .Include(r => r.User) // Make sure to include the User
                    .FirstOrDefault(r => r.Id == newInvestigation.ReportId);

                if (report != null)
                {
                    report.ReportStatusId = newInvestigation.ReportStatusId;
                    report.UpdatedDate = DateTime.UtcNow;

                    // Send email notification
                    if (report.User != null && !string.IsNullOrEmpty(report.User.Email))
                    {
                        var subject = $"Investigation created for your report: {report.Title}";
                        var message = $@"
                        <p>Hello,</p>
                        <p>An investigation has been created for your report (#{report.Id} - {report.Title}).</p>
                        <p>Investigation details:</p>
                        <p>{newInvestigation.Description}</p>
                        <p>Status: {_context.ReportStatuses.FirstOrDefault(s => s.Id == newInvestigation.ReportStatusId)?.Name}</p>
                        <p>Thank you,</p>
                        <p>The Support Team</p>";

                        await _emailSender.SendEmailAsync(report.User.Email, subject, message);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                newInvestigation.AvailableReports = _context.ReportPosts
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"Report #{r.Id} - {r.Title}"
                    }).ToList();

                return View(newInvestigation);
            }
        }


        // GET: InvestigationViewModels/Edit/5
        public IActionResult Edit(int id)
        {
            var reportPost = _investigationRepository.GetInvestigationById(id);
            if (reportPost != null)
            {
                var currentUserId = _userManager.GetUserId(User);
                if (reportPost.UserId == currentUserId)
                {
                    var model = new EditInvestigationViewModel
                    {
                        Id = reportPost.Id,
                        Description = reportPost.Description,
                        ReportStatusId = reportPost.ReportStatusId
                    };

                    var statusList = _investigationRepository.GetAllReportStatuses().Select(c => new ReportStatusViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

                    model.ReportStatusList = statusList;

                    return View(model);
                }
                else return Forbid();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInvestigationViewModel updatedInvestigation)
        {
            if (updatedInvestigation.ReportStatusId == 0)
            {
                ModelState.AddModelError("ReportStatusId", "Status is required");
            }

            if (!ModelState.IsValid)
            {
                updatedInvestigation.ReportStatusList = _investigationRepository.GetAllReportStatuses()
                    .Select(s => new ReportStatusViewModel { Id = s.Id, Name = s.Name })
                    .ToList();
                return View(updatedInvestigation);
            }

            var investigationToUpdate = _investigationRepository.GetInvestigationByIdWithReport(updatedInvestigation.Id);

            if (investigationToUpdate != null && investigationToUpdate.UserId == _userManager.GetUserId(User))
            {
                // Store old values for comparison
                var oldStatusId = investigationToUpdate.ReportStatusId;
                var oldDescription = investigationToUpdate.Description;

                investigationToUpdate.Description = updatedInvestigation.Description;
                investigationToUpdate.UpdatedDate = DateTime.UtcNow;
                investigationToUpdate.ReportStatusId = updatedInvestigation.ReportStatusId;

                if (investigationToUpdate.Report != null)
                {
                    investigationToUpdate.Report.ReportStatusId = updatedInvestigation.ReportStatusId;
                    investigationToUpdate.Report.UpdatedDate = DateTime.UtcNow;

                    // Check if status or description changed
                    if (oldStatusId != updatedInvestigation.ReportStatusId ||
                        oldDescription != updatedInvestigation.Description)
                    {
                        // Get report with author info
                        var report = _context.ReportPosts
                            .Include(r => r.User)
                            .FirstOrDefault(r => r.Id == investigationToUpdate.ReportId);

                        if (report?.User != null && !string.IsNullOrEmpty(report.User.Email))
                        {
                            var statusName = _context.ReportStatuses
                                .FirstOrDefault(s => s.Id == updatedInvestigation.ReportStatusId)?.Name;

                            var subject = $"Investigation updated for your report: {report.Title}";
                            var message = $@"
                            <p>Hello,</p>
                            <p>The investigation for your report (#{report.Id} - {report.Title}) has been updated.</p>
                            <p>New investigation details:</p>
                            <p>{updatedInvestigation.Description}</p>
                            <p>Status: {statusName}</p>
                            <p>Thank you,</p>
                            <p>The Support Team</p>";

                            await _emailSender.SendEmailAsync(report.User.Email, subject, message);
                        }
                    }
                }

                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("Details", new { id = investigationToUpdate.Id });
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(updatedInvestigation);
        }

        // GET: InvestigationViewModels/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _investigationRepository.DeleteInvestigation(id);
            return RedirectToAction("Index");
        }

        // POST: InvestigationViewModels/Delete/5

        private bool InvestigationViewModelExists(int id)
        {
            return _context.InvestigationViewModel.Any(e => e.Id == id);
        }
    }
}
