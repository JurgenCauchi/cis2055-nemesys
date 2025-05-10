namespace Nemesys.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Investigation> ReportPosts { get; set; }


    }
}
