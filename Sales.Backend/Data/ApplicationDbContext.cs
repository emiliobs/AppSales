namespace Sales.Backend.Data
{
    using Microsoft.EntityFrameworkCore;
    using Sales.Backend.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Product { get; set; }
    }
}
