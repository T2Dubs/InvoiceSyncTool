namespace InvoiceSyncTool.Models
{
    public partial class AccountingInvoice
    {
        public int InvoiceId { get; set; }
        public required string LocationId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long InvoiceNumber { get; set; }
        public required string VendorId { get; set; }
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}