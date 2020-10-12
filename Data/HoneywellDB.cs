
using System.Linq;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using Honeywell_backend.Models;

namespace Honeywell_backend.Data
{
    public class HoneywellDB: DbContext

    {
        public HoneywellDB(DbContextOptions<HoneywellDB> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HoneywellDB).Assembly);
            base.OnModelCreating(modelBuilder);
        }

    }
}
