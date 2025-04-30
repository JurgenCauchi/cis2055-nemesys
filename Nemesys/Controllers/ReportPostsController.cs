using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class ReportPostsController : Controller
    {
        private IReportRepository _reportRepository { get; set; }
        private readonly NemesysContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ReportPostsController(NemesysContext context, IReportRepository reportRepository, UserManager<AppUser> userManager)

        {
            _context = context;
            _reportRepository = reportRepository ?? throw new ArgumentNullException(nameof(reportRepository));
            _userManager = userManager;

        }

        // GET: ReportPosts
        public IActionResult Index()
        {
            //var blogPosts = _reportRepository.GetAllReportPosts().OrderByDescending(b => b.CreatedDate);

            var model = new ReportPostListViewModel()
            {
                TotalEntries = _reportRepository.GetAllReportPosts().Count(),
                ReportPosts = _reportRepository
                .GetAllReportPosts()
                .OrderByDescending(b => b.CreatedDate)
                .Select(b => new ReportPostViewModel
                {
                    Id = b.Id,
                    CreatedDate = b.CreatedDate,
                    Content = b.Content,
                    ImageUrl = b.ImageUrl,
                    //ReadCount = b.ReadCount,
                    Title = b.Title,
                    Category = new CategoryViewModel()
                    {
                        Id = b.Category.Id,
                        Name = b.Category.Name
                    },
                    Author = new AuthorViewModel()
                    {
                        Id = b.UserId,
                        Name = b.User != null ? b.User.UserName : "Anonymous"
                    }

                })
            };

            return View(model);

        }

        // GET: ReportPosts/Details/5
        public IActionResult Details(int id)
        {
            var reportPost = _reportRepository.GetReportPostById(id);
            if (reportPost == null)
                return NotFound();

            var model = new ReportPostViewModel()
            {
                Id = reportPost.Id,
                CreatedDate = reportPost.CreatedDate,
                ImageUrl = reportPost.ImageUrl,
                Title = reportPost.Title,
                Content = reportPost.Content,
                Category = new CategoryViewModel()
                {
                    Id = reportPost.Category.Id,
                    Name = reportPost.Category.Name
                },
                Author = new AuthorViewModel()
                {
                    Id = reportPost.UserId,
                    Name = reportPost.User != null ? reportPost.User.UserName : "Anonymous"
                },
                LoggedInUserId = _userManager.GetUserId(User)
            };

            return View(model);
        }


        // GET: ReportPosts/Create
        [HttpGet]
        //[Authorize]
        public IActionResult Create()
        {
            //Load all categories and create a list of CategoryViewModel
            var categoryList = _reportRepository.GetAllCategories().Select(c => new CategoryViewModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            //Pass the list into an EditBlogPostViewModel, which is used by the View (all other properties may be left blank, unless you want to add other default values
            var model = new EditReportPostViewModel()
            {
                CategoryList = categoryList
            };

            //Pass model to View
            return View(model);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult Create([Bind("Title, Content, ImageToUpload, CategoryId")] EditReportPostViewModel newReportPost)
        {
            if (newReportPost.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Category is required");
            }

            if (ModelState.IsValid)
            {
                string fileName = "";
                if (newReportPost.ImageToUpload != null)
                {
                    //At this point you should check size, extension etc...
                    //Then persist using a new name for consistency (e.g. new Guid)
                    var extension = "." + newReportPost.ImageToUpload.FileName.Split('.')[newReportPost.ImageToUpload.FileName.Split('.').Length - 1];
                    fileName = Guid.NewGuid().ToString() + extension;
                    var path = Directory.GetCurrentDirectory() + "\\wwwroot\\images\\reportposts\\" + fileName;
                    using (var bits = new FileStream(path, FileMode.Create))
                    {
                        newReportPost.ImageToUpload.CopyTo(bits);
                    }
                }

                ReportPost reportPost = new ReportPost()
                {
                    Title = newReportPost.Title,
                    Content = newReportPost.Content,
                    CreatedDate = DateTime.UtcNow,
                    ImageUrl = "/images/reportposts/" + fileName,
                    //ReadCount = 0,
                    CategoryId = newReportPost.CategoryId,
                    UserId = _userManager.GetUserId(User)
                };

                _reportRepository.CreateReportPost(reportPost);

                return RedirectToAction("Index");
            }
            else
            {
                var categoryList = _reportRepository.GetAllCategories().Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

                //Re-attach to view model before sending back to the View (this is necessary so that the View can repopulate the drop down and pre-select according to the CategoryId
                newReportPost.CategoryList = categoryList;

                return View(newReportPost);

            }
        }

        // GET: ReportPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportPost = await _context.ReportPosts.FindAsync(id);
            if (reportPost == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", reportPost.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", reportPost.UserId);
            return View(reportPost);
        }

        // POST: ReportPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedDate,Title,Content,ImageUrl,CategoryId,UpdatedDate,UserId")] ReportPost reportPost)
        {
            if (id != reportPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reportPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportPostExists(reportPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Id", reportPost.CategoryId);
            ViewData["UserId"] = new SelectList(_context.Set<AppUser>(), "Id", "Id", reportPost.UserId);
            return View(reportPost);
        }

        // GET: ReportPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reportPost = await _context.ReportPosts
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reportPost == null)
            {
                return NotFound();
            }

            return View(reportPost);
        }

        // POST: ReportPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reportPost = await _context.ReportPosts.FindAsync(id);
            if (reportPost != null)
            {
                _context.ReportPosts.Remove(reportPost);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportPostExists(int id)
        {
            return _context.ReportPosts.Any(e => e.Id == id);
        }
    }
}
