using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InvoiceSyncTool.Models
{
    [Table("ven_Invoices")]
    public partial class CustomerInvoice
    {
        [Key]
        public int InvoiceId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public long InvoiceNumber { get; set; }

        [Required, StringLength(1)]
        public required string InvoiceType { get; set; }

        [Required, Column("invoiceDate", TypeName = "date")]
        public required DateTime InvoiceDate { get; set; }

        [StringLength(1)]
        public string? ImportedAccounting { get; set; }
    }
}
