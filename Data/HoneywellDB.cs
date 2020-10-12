
using System.Linq;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;
using Honeywell_backend.Models;

namespace Honeywell_backend.Data
{
    public class HoneywellDB: DbContext

    {
        public HoneywellDB(DbContextOptions<HoneywellDB> options) : base(options) { }

        public DbSet<Costumer> Costumers { get; set; }

    }
}
