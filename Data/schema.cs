using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceHandler.Data
{
    public class Invoice
    {
        public const string InvoicerName = "Rakennus Oy";
        public const string InvoicerAddress = "Karjalankatu 3, 80200 JOENSUU";

        [Key]
        public int ID { get; set; }
        
        [Required]
        public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        
        [Required]
        public DateOnly InvoiceDueDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddDays(14));

        public bool PaymentStatus { get; set; } = false;

        public double InvoiceTotal { get; set; } = 0;

        [Required]
        public string WorkDescription { get; set; } = "No additional information";

        [ForeignKey("CustomerID")]
        public int CustomerID { get; set; } // foreign key

        public Customer? Customer { get; set; }

        public Invoice(DateOnly iDate,  DateOnly iDueDate, bool status, double iTotal, string workdesc, int custID)
        {
            InvoiceDate = iDate;
            InvoiceDueDate = iDueDate;
            PaymentStatus = status;
            InvoiceTotal = iTotal;
            WorkDescription = workdesc;
            CustomerID = custID;
        }
        public Invoice() { }
    }

    public class Customer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = "Name not set";

        [Required]
        public string Address { get; set; } = "Address not set";

        public Customer(string name, string add)
        {
            Name = name;
            Address = add;
        }

        public Customer() { }
    }

    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = "Name not set";

        [Required]
        public string Unit { get; set; } = "Pcs";

        public double PricePerUnit { get; set; } = 0;

        public Product(string name, string unit, double ppu)
        {
            Name = name;
            Unit = unit;
            PricePerUnit = ppu;
        }

        public Product() { }
    }

    public class InvoiceLine
    {
        [Key]
        public int ID { get; set; }

        public int Amount { get; set; } = 0;

        public double LineTotal { get; set; } = 0;

        [ForeignKey("ProductID")]
        public int ProductID { get; set; }

        [ForeignKey("InvoiceID")]
        public int InvoiceID { get; set; }

        public Product? Product { get; set; }

        public Invoice? Invoice { get; set; }

        public InvoiceLine(int amount, double total, int prodID, int InvID)
        {
            Amount = amount;
            LineTotal = total;
            ProductID = prodID;
            InvoiceID = InvID;
        }

        public InvoiceLine() { }

    }
}

