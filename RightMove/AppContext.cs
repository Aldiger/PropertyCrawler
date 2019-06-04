using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightMove
{
    public class AppContext : DbContext
    {
        public DbSet<PostalCode> PostalCodes { get; set; }
        public DbSet<Url> Url { get; set; }
        public DbSet<Portal> Portal { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLExpress;Database=RightMove;Trusted_Connection=True;");
        }
    }

    public class PostalCode:Base
    {

        public string Code { get; set; }
        public string OpCode { get; set; }

        public ICollection<Url> Urls { get; set; }
    }

    public class Portal :Base
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public ICollection<Url> Urls { get; set; }
    }

    public class Url :Base
    {
        
        public string PropertyUrl { get; set; }
        public int Type { get; set; }

        public int PostalCodeId { get; set; }
        public int PortalId { get; set; }
    }

    public abstract class Base
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
    }
    public enum Type
    {
        Sell=1,
        Rent
    }
}
