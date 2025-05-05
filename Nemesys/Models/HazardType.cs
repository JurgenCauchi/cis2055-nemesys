namespace Nemesys.Models
{
    public class HazardType
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public List<Investigation> ReportPosts { get; set; }
    }
}
