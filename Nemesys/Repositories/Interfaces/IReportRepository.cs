using Nemesys.Models;

namespace Nemesys.Repositories.Interfaces
{
    public interface IReportRepository
    {
        IEnumerable<ReportPost> GetAllReportPosts();
        ReportPost GetReportPostById(int blogPostId);

        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int categoryId);

        void CreateReportPost(ReportPost newBlogPost);

        void UpdateReportPost(ReportPost updatedBlogPost);

    }
}
