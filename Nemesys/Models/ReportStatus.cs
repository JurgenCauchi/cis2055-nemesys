﻿namespace Nemesys.Models
{
    public class ReportStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Collection navigation property
        public List<ReportPost> ReportPosts { get; set; }
    }
}
