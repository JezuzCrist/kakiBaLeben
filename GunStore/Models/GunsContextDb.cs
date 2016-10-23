using System.Data.Entity;

namespace GunStore.Models
{
    public class GunsContextDb : DbContext
    {
        public GunsContextDb()
        {

        }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<Gun> Guns { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}