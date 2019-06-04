using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Url> Url { get; set; }
        public DbSet<Portal> Portal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=RightMove;Trusted_Connection=True;");
        }
    }
}
