using System.Windows;

namespace InvoiceHandler.Data
{
    public class dbActions
    {
        public static int GetInvoiceCount()
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
                MessageBox.Show($"Error loading Invoice count: {ex}");
                return 0;
            }
        }

        public static double GetTotalRevenue()
        {
            try
            {
                using (var db = new InvoiceDbContext())
                {
                    var totalRevenue = db.Invoices
                        .Sum(i => i.InvoiceTotal);
                    return totalRevenue;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading total revenue: {ex}");
                return 0;
            }
        }

        public static int GetCustomerCount()
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
                MessageBox.Show($"Error loading customer count: {ex}");
                return 0;
            }
        }

        public static int GetProductsSold()
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
                MessageBox.Show($"Error loading products sold: {ex}");
                return 0;
            }
        }

        public static List<Customer>? CustomersToList()
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
                MessageBox.Show($"Error loading Customer list: {ex}");
                return null;
            }
        }

    }
}
