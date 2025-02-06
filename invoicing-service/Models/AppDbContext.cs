using Microsoft.EntityFrameworkCore;

namespace invoicing_service.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Invoice> Invoices { get; set; }
    }
}