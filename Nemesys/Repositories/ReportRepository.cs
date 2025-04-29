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

        public void CreateReportPost(ReportPost newBlogPost)
        {
            _appDbContext.ReportPosts.Add(newBlogPost);
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

        public ReportPost GetBlogPostById(int blogPostId)
        {
            return _appDbContext.ReportPosts
                .Include(x => x.Category)
                .Include(x => x.User)
                .FirstOrDefault(p => p.Id == blogPostId);
        }

        public Category GetCategoryById(int categoryId)
        {
            //Not loading related blog posts
            return _appDbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

        }

        public void UpdateBlogPost(ReportPost updatedBlogPost)
        {
            var existingBlogPost = _appDbContext.ReportPosts.SingleOrDefault(bp => bp.Id == updatedBlogPost.Id);
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
