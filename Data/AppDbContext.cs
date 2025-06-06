using Microsoft.EntityFrameworkCore;
using InvoiceSyncTool.Models;

namespace InvoiceSyncTool.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual DbSet<AccountingInvoice> AccountingInvoices { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountingInvoice>(entity =>
            {
                entity.ToView("LocationVendorInvoices");
                entity.HasNoKey();
            });
        }
    }
}
