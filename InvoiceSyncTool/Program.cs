using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using InvoiceSyncTool.Data;
using InvoiceSyncTool.Services;
using InvoiceSyncTool.Utilities;
using System.Data;
using Intacct.SDK.Functions.AccountsPayable;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string dbConnection = config.GetConnectionString("AppDatabase")
    ?? throw new Exception("Connection string is null or empty");

var services = new ServiceCollection();
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(dbConnection));
services.AddTransient<AppDataService>();

var serviceProvider = services.BuildServiceProvider();
var dataService = serviceProvider.GetService<AppDataService>()
    ?? throw new InvalidOperationException("Data service could not be resolved");

dataService.LoadDBValues();

foreach (var customerInvoice in dataService.CustomerInvoices)
{
    try
    {
        var relatedAccountingInvoices = dataService.AccountingInvoices
            .Where(si => si.InvoiceId == customerInvoice.InvoiceId);

        DataTable? invoiceData = InvoiceToDataTable.Execute(relatedAccountingInvoices, "InvoiceId");

        if (invoiceData == null)
            continue;

        var locationCode = invoiceData.Rows[0]["LocationId"].ToString()!;
        var entityCode = locationCode[..10];
        var vendorId = invoiceData.Rows[0]["VendorId"].ToString()!;
        var client = CreateAccountingClient.Execute(config, entityCode);

        if (ValidateBillNumberAvailability.Execute(client, customerInvoice.InvoiceNumber.ToString(), vendorId))
        {
            dataService.UpdateImportStatus(customerInvoice);
            continue;
        }

        List<AbstractBillLine> lines = [.. invoiceData.AsEnumerable()
            .Where(row => row.Field<long>("InvoiceNumber") == customerInvoice.InvoiceNumber)
                .Select(row => new BillLineCreate
                {
                    GlAccountNumber = row.Field<int>("AccountNumber").ToString(),
                    TransactionAmount = row.Field<decimal>("Amount"),
                    LocationId = row.Field<string>("LocationId")
                })
                .Cast<AbstractBillLine>()];

        var billCreate = new BillCreate
        {
            VendorId = vendorId,
            GlPostingDate = customerInvoice.InvoiceDate,
            TransactionDate = customerInvoice.InvoiceDate,
            DueDate = customerInvoice.InvoiceDate,
            BillNumber = customerInvoice.InvoiceNumber.ToString(),
            Lines = lines
        };

        await client.Execute(billCreate);
        dataService.UpdateImportStatus(customerInvoice);
    }
    catch (Exception) { }
}