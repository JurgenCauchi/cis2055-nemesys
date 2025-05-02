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
                Location = reportPost.Location,
                Category = new CategoryViewModel()
                {
                    Id = reportPost.Category.Id,
                    Name = reportPost.Category.Name
                },
                HazardType = new HazardTypeViewModel()
                {
                    Id = reportPost.Hazard.Id,
                    Name = reportPost.Hazard.Name
                },
                ReportStatus = new ReportStatusViewModel()
                {
                    Id = reportPost.ReportStatus.Id,
                    Name = reportPost.ReportStatus.Name
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

            var hazardList = _reportRepository.GetAllHazardTypes().Select(c => new HazardTypeViewModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            var statusList = _reportRepository.GetAllReportStatuses().Select(c => new ReportStatusViewModel()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();


            //Pass the list into an EditBlogPostViewModel, which is used by the View (all other properties may be left blank, unless you want to add other default values
            var model = new EditReportPostViewModel()
            {
                CategoryList = categoryList,
                HazardTypeList = hazardList,
                ReportStatusList = statusList,
                ReportStatusId = 1,

            };

            //Pass model to View
            return View(model);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult Create([Bind("Title, Content, ImageToUpload, CategoryId, Location, HazardTypeId, ReportStatusId")] EditReportPostViewModel newReportPost)

        {
            if (newReportPost.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Category is required");
            }
            if (newReportPost.HazardTypeId == 0)
            {
                ModelState.AddModelError("HazardTypeId", "Hazard type is required");
            }
            if (newReportPost.ReportStatusId == 0)
            {
                ModelState.AddModelError("ReportStatusId", "Status is required");
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
                    UserId = _userManager.GetUserId(User),
                    HazardTypeId = newReportPost.HazardTypeId,
                    ReportStatusId = newReportPost.ReportStatusId,
                    Location = newReportPost.Location

                };

                _reportRepository.CreateReportPost(reportPost);

                return RedirectToAction("Index");
            }
            else
            {
                newReportPost.CategoryList = _reportRepository.GetAllCategories().Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

                newReportPost.HazardTypeList = _reportRepository.GetAllHazardTypes().Select(h => new HazardTypeViewModel
                {
                    Id = h.Id,
                    Name = h.Name
                }).ToList();

                newReportPost.ReportStatusList = _reportRepository.GetAllReportStatuses().Select(s => new ReportStatusViewModel
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList();

                return View(newReportPost);
            }

        }

        public IActionResult Edit(int id)
        {
            var reportPost = _reportRepository.GetReportPostById(id);
            if (reportPost != null)
            {
                var currentUserId = _userManager.GetUserId(User);
                if (reportPost.UserId == currentUserId)
                {
                    var model = new EditReportPostViewModel
                    {
                        Id = reportPost.Id,
                        Title = reportPost.Title,
                        Content = reportPost.Content,
                        ImageUrl = reportPost.ImageUrl,
                        CategoryId = reportPost.CategoryId,
                        Location = reportPost.Location,
                        HazardTypeId = reportPost.HazardTypeId,
                        ReportStatusId = reportPost.ReportStatusId
                    };

                    // Load category list
                    var categoryList = _reportRepository.GetAllCategories().Select(c => new CategoryViewModel
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

                    var hazardList = _reportRepository.GetAllHazardTypes().Select(c => new HazardTypeViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

                    var statusList = _reportRepository.GetAllReportStatuses().Select(c => new ReportStatusViewModel()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();

                    model.CategoryList = categoryList;
                    model.HazardTypeList = hazardList;
                    model.ReportStatusList = statusList;

                    return View(model);
                }
                else return Forbid();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditReportPostViewModel updatedReportPost)
        {
            if (updatedReportPost.CategoryId == 0)
            {
                ModelState.AddModelError("CategoryId", "Category is required");
            }
            if (updatedReportPost.HazardTypeId == 0)
            {
                ModelState.AddModelError("HazardTypeId", "Hazard type is required");
            }
            if (updatedReportPost.ReportStatusId == 0)
            {
                ModelState.AddModelError("ReportStatusId", "Status is required");
            }

            if (ModelState.IsValid)
            {
                string fileName = updatedReportPost.ImageUrl; // keep existing image by default

                // If a new image is uploaded, process it
                if (updatedReportPost.ImageToUpload != null)
                {
                    var extension = Path.GetExtension(updatedReportPost.ImageToUpload.FileName);
                    fileName = "/images/reportposts/" + Guid.NewGuid().ToString() + extension;
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName.TrimStart('/'));

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        updatedReportPost.ImageToUpload.CopyTo(stream);
                    }
                }

                var reportToUpdate = _reportRepository.GetReportPostById(updatedReportPost.Id);

                if (reportToUpdate != null && reportToUpdate.UserId == _userManager.GetUserId(User))
                {
                    reportToUpdate.Title = updatedReportPost.Title;
                    reportToUpdate.Content = updatedReportPost.Content;
                    reportToUpdate.CategoryId = updatedReportPost.CategoryId;
                    reportToUpdate.ImageUrl = fileName;
                    reportToUpdate.UpdatedDate = DateTime.UtcNow;
                    reportToUpdate.Location = updatedReportPost.Location;
                    reportToUpdate.HazardTypeId = updatedReportPost.HazardTypeId;
                    reportToUpdate.ReportStatusId = updatedReportPost.ReportStatusId;


                    _context.SaveChanges(); // or _reportRepository.UpdateReportPost(reportToUpdate);

                    return RedirectToAction("Details", new { id = reportToUpdate.Id });
                }

                return Forbid();
            }

            // if validation fails, reattach categories
            updatedReportPost.CategoryList = _reportRepository.GetAllCategories().Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();

            return View(updatedReportPost);
        }



        // GET: ReportPosts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _reportRepository.DeleteReportPost(id);
            return RedirectToAction("Index");
        }

        private bool ReportPostExists(int id)
        {
            return _context.ReportPosts.Any(e => e.Id == id);
        }
    }
}
