using Nemesys.Models;

namespace Nemesys.Repositories.Interfaces
{
    public interface IReportRepository
    {
        IEnumerable<ReportPost> GetAllReportPosts();
        ReportPost GetReportPostById(int blogPostId);

        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int categoryId);
        IEnumerable<HazardType> GetAllHazardTypes();
        HazardType GetHazardById(int hazardId);
        IEnumerable<ReportStatus> GetAllReportStatuses();
        ReportStatus GetStatusById(int StatusId);

        void UpvoteReport(int reportPostId, string userId);
        bool HasUserUpvoted(int reportPostId, string userId);
        public int GetUpvoteCount(int reportPostId);

        void CreateReportPost(ReportPost newBlogPost);

        void UpdateReportPost(ReportPost updatedBlogPost);

        void DeleteReportPost(int id);

    }
}
