using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eCommerce.DataAccess.Models
{
    public partial class eCommDbContext : DbContext
    {
        public eCommDbContext()
        {
        }

        public eCommDbContext(DbContextOptions<eCommDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Frequency> Frequency { get; set; }
        public virtual DbSet<Service> Service { get; set; }
    }
}
