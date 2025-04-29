using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;

namespace Nemesys.Data
{
    public class NemesysContext : DbContext
    {
        public NemesysContext (DbContextOptions<NemesysContext> options)
            : base(options)
        {
        }

        public DbSet<Nemesys.Models.ReportPost> ReportPost { get; set; } = default!;
        public IEnumerable<Category> Category { get; internal set; }
    }
}
