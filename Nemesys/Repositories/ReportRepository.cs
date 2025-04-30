using Nemesys.Models;
using Nemesys.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;
using System;
using Nemesys.Data;

namespace Nemesys.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly NemesysContext _appDbContext;

        public ReportRepository(NemesysContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void CreateReportPost(ReportPost newReportPost)
        {
            _appDbContext.ReportPosts.Add(newReportPost);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<ReportPost> GetAllReportPosts()
        {
            return _appDbContext.ReportPosts
                .Include(x => x.Category)
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _appDbContext.Categories;
        }

        public ReportPost GetReportPostById(int reportPostId)
        {
            return _appDbContext.ReportPosts
                .Include(x => x.Category)
                .Include(x => x.User)
                .FirstOrDefault(p => p.Id == reportPostId);
        }

        public Category GetCategoryById(int categoryId)
        {
            //Not loading related blog posts
            return _appDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

        }

        public void UpdateReportPost(ReportPost updatedReportPost)
        {
            var existingBlogPost = _appDbContext.ReportPosts.SingleOrDefault(bp => bp.Id == updatedReportPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Title = updatedReportPost.Title;
                existingBlogPost.Content = updatedReportPost.Content;
                existingBlogPost.UpdatedDate = updatedReportPost.UpdatedDate;
                existingBlogPost.ImageUrl = updatedReportPost.ImageUrl;
                existingBlogPost.CategoryId = updatedReportPost.CategoryId;

                _appDbContext.SaveChanges();
            }

        }

        public void DeleteReportPost(int id)
        {
            var report = _appDbContext.ReportPosts.FirstOrDefault(r => r.Id == id);
            if (report != null)
            {
                _appDbContext.ReportPosts.Remove(report);
                _appDbContext.SaveChanges();
            }
        }


        /*public void UpdateReportPost(ReportPost updatedReportPost)
        {
            throw new NotImplementedException();
        } */
    }
}
