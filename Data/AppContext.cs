using Microsoft.EntityFrameworkCore;

namespace RightMove.Data
{
    public class AppContext : DbContext
    {
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Url> Url { get; set; }
        public DbSet<Portal> Portal { get; set; }
        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=RightMove;Trusted_Connection=True;");
        //}
    }
}
