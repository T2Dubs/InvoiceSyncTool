using InvoiceSyncTool.Data;
using InvoiceSyncTool.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSyncTool.Services
{
    public class AppDataService(AppDbContext dbContext)
    {
        public readonly AppDbContext _dbcontext = dbContext;

        public List<CustomerInvoice> CustomerInvoices { get; set; } = [];
        public List<AccountingInvoice> AccountingInvoices { get; set; } = [];

        public void LoadDBValues()
        {
            CustomerInvoices = [.. _dbcontext.CustomerInvoices.Where(ci => ci.ImportedAccounting == "F")];
            AccountingInvoices = [.. _dbcontext.AccountingInvoices];
        }

        public void UpdateImportStatus(CustomerInvoice customerInvoice)
        {
            customerInvoice.ImportedAccounting = "T";
            _dbcontext.Entry(customerInvoice).State = EntityState.Modified;
            _dbcontext.SaveChanges();
        }
    }
}
