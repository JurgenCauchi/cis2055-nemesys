﻿using Nemesys.Models;
using Nemesys.Models.ViewModels;

namespace Nemesys.Repositories.Interfaces
{
    public interface IReportRepository
    {
        IEnumerable<ReportPost> GetAllReportPosts();
        ReportPost GetReportPostById(int blogPostId);

        IEnumerable<HazardType> GetAllHazardTypes();
        HazardType GetHazardById(int hazardId);
        IEnumerable<ReportStatus> GetAllReportStatuses();
        ReportStatus GetStatusById(int StatusId);

        void UpvoteReport(int reportPostId, string userId);
        bool HasUserUpvoted(int reportPostId, string userId);
        public int GetUpvoteCount(int reportPostId);

        public IEnumerable<ReporterRankingViewModel> GetReporterRankings(int year);

        void CreateReportPost(ReportPost newBlogPost);

        void UpdateReportPost(ReportPost updatedBlogPost);

        void DeleteReportPost(int id);

    }
}