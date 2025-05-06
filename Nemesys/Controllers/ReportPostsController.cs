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
        public string ImageUrl { get; private set; }

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
            var reportPosts = _reportRepository.GetAllReportPosts();

            var model = new ReportPostListViewModel()
            {
                TotalEntries = reportPosts.Count(),
                ReportPosts = reportPosts
                    .OrderByDescending(b => b.CreatedDate)
                    .Select(b => new ReportPostViewModel
                    {
                        Id = b.Id,
                        CreatedDate = b.CreatedDate,
                        Content = b.Content,
                        ImageUrl = b.ImageUrl,
                        Title = b.Title,
                        Location = b.Location,
                        HazardType = new HazardTypeViewModel
                        {
                            Id = b.Hazard.Id,
                            Name = b.Hazard.Name
                        },
                        ReportStatus = new ReportStatusViewModel
                        {
                            Id = b.ReportStatus.Id,
                            Name = b.ReportStatus.Name
                        },
                        Author = new AuthorViewModel
                        {
                            Id = b.UserId,
                            Name = b.User != null ? b.User.UserName : "Anonymous"
                        },
                        UpvoteCount = _reportRepository.GetUpvoteCount(b.Id),
                        LoggedInUserId = _userManager.GetUserId(User)
                    }).ToList()
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult ToggleUpvote(int reportPostId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var upvote = _context.ReportUpvotes
                .FirstOrDefault(u => u.ReportPostId == reportPostId && u.UserId == userId);

            if (upvote != null)
            {
                _context.ReportUpvotes.Remove(upvote);
            }
            else
            {
                _context.ReportUpvotes.Add(new ReportUpvote
                {
                    ReportPostId = reportPostId,
                    UserId = userId
                });
            }

            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString()); //Refreshes screen

        }




        // GET: ReportPosts/Details/5
        public IActionResult Details(int id)
        {
            var reportPost = _reportRepository.GetReportPostById(id);
            if (reportPost == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);

            var model = new ReportPostViewModel()
            {
                Id = reportPost.Id,
                CreatedDate = reportPost.CreatedDate,
                ImageUrl = reportPost.ImageUrl,
                Title = reportPost.Title,
                Content = reportPost.Content,
                Location = reportPost.Location,
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
                UpvoteCount = _context.ReportUpvotes.Count(u => u.ReportPostId == id),
                HasUpvoted = _context.ReportUpvotes.Any(u => u.ReportPostId == id && u.UserId == userId),
                LoggedInUserId = userId
            };

            return View(model);
        }



        // GET: ReportPosts/Create
        [HttpGet]
        //[Authorize]
        public IActionResult Create()
        { 

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
                HazardTypeList = hazardList,
                ReportStatusList = statusList,
                ReportStatusId = 1,

            };

            //Pass model to View
            return View(model);
        }

        [HttpPost]
        //[Authorize]
        public IActionResult Create([Bind("Title, Content, ImageToUpload, Location, HazardTypeId, ReportStatusId")] EditReportPostViewModel newReportPost)

        {

            if (newReportPost.HazardTypeId == 0)
            {
                ModelState.AddModelError("HazardTypeId", "Hazard type is required");
            }
            //if (newReportPost.ReportStatusId == 0)
            //{
            //    ModelState.AddModelError("ReportStatusId", "Status is required");
            //}

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

                if (fileName.Length > 0)
                {
                    ImageUrl = "/images/reportposts/" + fileName;
                }
                else
                {
                    ImageUrl = "";
                }

                ReportPost reportPost = new ReportPost()
                {
                    Title = newReportPost.Title,
                    Content = newReportPost.Content,
                    CreatedDate = DateTime.UtcNow,
                    ImageUrl = ImageUrl,
                    //ReadCount = 0,
                    UserId = _userManager.GetUserId(User),
                    HazardTypeId = newReportPost.HazardTypeId,
                    //ReportStatusId = newReportPost.ReportStatusId,
                    ReportStatusId = 1,
                    Location = newReportPost.Location

                };

                _reportRepository.CreateReportPost(reportPost);

                return RedirectToAction("Index");
            }
            else
            {


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
                        Location = reportPost.Location,
                        HazardTypeId = reportPost.HazardTypeId,
                        //ReportStatusId = reportPost.ReportStatusId
                        ReportStatusId = 1
                    };

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