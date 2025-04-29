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

        public void CreateBlogPost(ReportPost newBlogPost)
        {
            _appDbContext.ReportPost.Add(newBlogPost);
            _appDbContext.SaveChanges();
        }

        public void CreateReportPost(ReportPost newBlogPost)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ReportPost> GetAllReportPosts()
        {
            return _appDbContext.ReportPost
                .Include(x => x.Category)
                .Include(x => x.User)
                .OrderByDescending(x => x.CreatedDate).ToList();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _appDbContext.Category;
        }

        public ReportPost GetBlogPostById(int blogPostId)
        {
            return _appDbContext.ReportPost
                .Include(x => x.Category)
                .Include(x => x.User)
                .FirstOrDefault(p => p.Id == blogPostId);
        }

        public Category GetCategoryById(int categoryId)
        {
            //Not loading related blog posts
            return _appDbContext.Category.FirstOrDefault(c => c.Id == categoryId);

        }

        public void UpdateBlogPost(ReportPost updatedBlogPost)
        {
            var existingBlogPost = _appDbContext.ReportPost.SingleOrDefault(bp => bp.Id == updatedBlogPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Title = updatedBlogPost.Title;
                existingBlogPost.Content = updatedBlogPost.Content;
                existingBlogPost.UpdatedDate = updatedBlogPost.UpdatedDate;
                existingBlogPost.ImageUrl = updatedBlogPost.ImageUrl;
                existingBlogPost.CategoryId = updatedBlogPost.CategoryId;

                _appDbContext.SaveChanges();
            }

        }

        public void UpdateReportPost(ReportPost updatedBlogPost)
        {
            throw new NotImplementedException();
        }
    }
}
