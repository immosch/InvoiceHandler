using System.Collections.ObjectModel;
using System.Windows;

namespace InvoiceHandler.Data
{
    public class dbActions
    {
        private static int currentYear = DateTime.Now.Year;

        public static int GetInvoiceCount() // invoice count query
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var invoiceCount = db.Invoices
                        .Count();
                    return invoiceCount;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching Invoice count: {ex}");
                return 0;
            }
        }

        public static double GetTotalRevenue() // total revenue query
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var totalRevenue = db.Invoices
                        .Sum(i => i.InvoiceTotal);
                    return Math.Round(totalRevenue, 2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching total revenue: {ex}");
                return 0;
            }
        }

        public static int GetCustomerCount() // Customer count query
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var totalCustomers = db.Customers.Count();
                    return totalCustomers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching customer count: {ex}");
                return 0;
            }
        }

        public static double GetProductsSold() // products sold count query
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var productsSold = db.InvoiceLines
                        .Sum(il => il.Amount);
                    return productsSold;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching products sold: {ex}");
                return 0;
            }
        }

        public static List<Customer> CustomersToList() // all customers to list query
        {
            try
            {
                using ( var db = new InvoiceDbContext())
                {
                    return db.Customers.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching Customer list: {ex}");
                return [];
            }
        }

        public static List<RevenueData> GetRevenueData() // revenue data grouped by month query
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var revenueData = db.Invoices
                        .Where(i => i.InvoiceDate.Year == currentYear)
                        .GroupBy(i => i.InvoiceDate.Month)
                        .Select(l => new RevenueData
                        {
                            Month = l.Key,
                            TotalRevenue = l.Sum(i => i.InvoiceTotal)
                        })
                        .ToList();

                    return revenueData;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching revenue chart data: {ex}");
                return [];
            }
        }

        public static int GetNextInvoiceID() // next invoice ID query
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var nextID = db.Invoices
                        .Max(i => i.ID);
                    return nextID + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching next invoice ID: {ex}");
                return 0;
            }
        }

        public static List<Product> GetProducts() // fetch all products
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    return db.Products.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching products data: {ex}");
                return [];
            }
        }

        public static bool CreateInvoice(Invoice invoice, List<InvoiceLine> invoiceLines)
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    db.Invoices.Add(invoice);
                    db.SaveChanges();

                    foreach (var line in invoiceLines)
                    {
                        db.InvoiceLines.Add(line);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating invoice: {ex}");
                return false;
            }
        }
    }
}
