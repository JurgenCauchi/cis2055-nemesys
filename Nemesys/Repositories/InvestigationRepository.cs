using Nemesys.Models;
using Nemesys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;
using System;
using Nemesys.Data;
using Nemesys.Models.ViewModels;

namespace Nemesys.Repositories
{
    public class InvestigationRepository : IInvestigationRepository
    {
        private readonly NemesysContext _appDbContext;

        public InvestigationRepository(NemesysContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void CreateInvestigation(Investigation newinvestigations)
        {
            _appDbContext.Investigations.Add(newinvestigations);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _appDbContext.Investigations
                .Include(i => i.ReportStatus)
                .Include(static x => x.User)
                .Include(i => i.Report)
                .OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _appDbContext.Categories;
        }

        public Investigation GetInvestigationById(int reportPostId)
        {
            return _appDbContext.Investigations
                .Include(x => x.User)
                .FirstOrDefault(p => p.Id == reportPostId);
        }


        public Category GetCategoryById(int categoryId)
        {
            //Not loading related blog posts
            return _appDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

        }

        public IEnumerable<HazardType> GetAllHazardTypes()
        {
            return _appDbContext.HazardTypes;
        }

        public HazardType GetHazardById(int hazardId)
        {
            //Not loading related blog posts
            return _appDbContext.HazardTypes.FirstOrDefault(c => c.Id == hazardId);
        }

        public IEnumerable<ReportStatus> GetAllReportStatuses()
        {
            return _appDbContext.ReportStatuses;
        }

        public ReportStatus GetStatusById(int statusId)
        {
            //Not loading related blog posts
            return _appDbContext.ReportStatuses.FirstOrDefault(c => c.Id == statusId);

        }


        public IEnumerable<ReporterRankingViewModel> GetReporterRankings(int year)
        {
            return _appDbContext.ReportPosts
                .Include(r => r.User)
                .Where(r => r.CreatedDate.Year == year && r.User != null)
                .GroupBy(r => new { r.User.Email, r.User.AuthorAlias })
                .Select(g => new ReporterRankingViewModel
                {
                    ReporterEmail = g.Key.Email,
                    ReporterAlias = g.Key.AuthorAlias,
                    ReportCount = g.Count()
                })
                .OrderByDescending(r => r.ReportCount)
                .ToList();
        }
        public int GetUpvoteCount(int reportPostId)
        {
            return _appDbContext.ReportUpvotes.Count(u => u.ReportPostId == reportPostId);
        }


        public void UpdateInvestigation(Investigation updatedReportPost)
        {
            var existingBlogPost = _appDbContext.Investigations.Include(i => i.Report).SingleOrDefault(bp => bp.Id == updatedReportPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.ReportId = updatedReportPost.ReportId;
                existingBlogPost.Description = updatedReportPost.Description;
                existingBlogPost.UpdatedDate = updatedReportPost.UpdatedDate;
                existingBlogPost.ReportStatus = updatedReportPost.ReportStatus;
                _appDbContext.SaveChanges();
            }

        }
        public void DeleteInvestigation(int id)
        {
            var investigation = _appDbContext.Investigations.FirstOrDefault(r => r.Id == id);
            if (investigation != null)
            {
                _appDbContext.Investigations.Remove(investigation);
                _appDbContext.SaveChanges();
            }
        }

        public Investigation GetInvestigationByIdWithReport(int id)
        {
            return _appDbContext.Investigations
                .Include(i => i.Report)  // Important: Include the related Report
                .FirstOrDefault(i => i.Id == id);
        }

    }
}
