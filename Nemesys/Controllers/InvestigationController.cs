using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
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
    public class InvestigationController : Controller
    {
        private readonly NemesysContext _context;
        private readonly UserManager<AppUser> _userManager;
        private IInvestigationRepository _investigationRepository { get; set; }
        public InvestigationController(NemesysContext context, IInvestigationRepository investigationRepository, UserManager<AppUser> userManager)
        {
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
                .Include(i => i.ReportStatus) // Ensure ReportStatus is loaded with the Investigation
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
                    Id = investigation.UserId,
                    Name = investigation.User != null ? investigation.User.UserName : "Anonymous"
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
        public IActionResult Create([Bind("ReportId, Description, ReportStatusId,Title")] EditInvestigationViewModel newInvestigation)
        {
            if (ModelState.IsValid)
            {
                Investigation investigation = new Investigation()
                {
                    ReportId = newInvestigation.ReportId,
                    Description = newInvestigation.Description,
                    Report = newInvestigation.Report,
                    CreatedDate = DateTime.UtcNow,
                    ReportStatusId = newInvestigation.ReportStatusId,
                    UserId = _userManager.GetUserId(User),
                    
                };

                _investigationRepository.CreateInvestigation(investigation);
                return RedirectToAction("Index");
            }
            else
            {
                // RE-POPULATE DROPDOWN
                newInvestigation.AvailableReports = _context.ReportPosts
                    .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = $"Report #{r.Id} - {r.Title}" // or any display
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
        public IActionResult Edit(EditInvestigationViewModel updatedInvestigation)
        {
            if (updatedInvestigation.ReportStatusId == 0)
            {
                ModelState.AddModelError("ReportStatusId", "Status is required");
            }

            if (!ModelState.IsValid)
            {
                // Repopulate dropdown list if returning to view
                updatedInvestigation.ReportStatusList = _investigationRepository.GetAllReportStatuses()
                    .Select(s => new ReportStatusViewModel { Id = s.Id, Name = s.Name })
                    .ToList();
                return View(updatedInvestigation);
            }

            var investigationToUpdate = _investigationRepository.GetInvestigationByIdWithReport(updatedInvestigation.Id);

            if (investigationToUpdate != null && investigationToUpdate.UserId == _userManager.GetUserId(User))
            {
                // Update investigation
                investigationToUpdate.Description = updatedInvestigation.Description;
                investigationToUpdate.UpdatedDate = DateTime.UtcNow;
                investigationToUpdate.ReportStatusId = updatedInvestigation.ReportStatusId;

                // Update linked report status if it exists
                if (investigationToUpdate.Report != null)
                {
                    investigationToUpdate.Report.ReportStatusId = updatedInvestigation.ReportStatusId;
                    investigationToUpdate.Report.UpdatedDate = DateTime.UtcNow;
                }

                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("Details", new { id = investigationToUpdate.Id });
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                    // Log the error (ex)
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
