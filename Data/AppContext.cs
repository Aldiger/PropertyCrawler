﻿using Microsoft.EntityFrameworkCore;

namespace RightMove.Data
{
    public class AppContext : DbContext
    {
        private readonly bool console;

        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Url> Urls { get; set; }
        public DbSet<Portal> Portals { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<Image> Images { get; set; }

        public AppContext(DbContextOptions<AppContext> options) : base(options)
        {
        }
        public AppContext(bool cons) : base()
        {
            console = cons;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (console)
            {
                optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=RightMove;Trusted_Connection=True;");
            }
        }
    }
}