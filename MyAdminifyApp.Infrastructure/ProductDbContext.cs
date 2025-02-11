

using Microsoft.EntityFrameworkCore;
using MyAdminifyApp.Domain.Entities;


namespace MyAdminifyApp.Application.Interfaces
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }


    }
}
