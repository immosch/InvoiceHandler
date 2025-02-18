using DotNetEnv;
using InvoiceHandler.Data;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Windows;

namespace InvoiceHandler
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                using (var db = new InvoiceDbContext())
                {
                    if (db.Database.CanConnect()) // if db exists
                    {
                        db.Database.EnsureDeleted(); // delete db
                    }
                    db.Database.EnsureCreated(); // recreate db
                }
                PopulateDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during startup: {ex.Message}");
            }
        }

        private static void PopulateDB()
        {
            var cust = new Customer("Malli Asiakas", "Asiakkaantie 22, 80200 Joensuu"); // 1
            var prod = new Product("Work", "h", 80); // 1
            var prod2 = new Product("Silicone", "Pcs", 5.95); // 2
            var prod3 = new Product("Sealant", "Pcs", 1); // 3
            var prod4 = new Product("Lumber 22x100 ST", "m", 1.18); // 4
            var inv1 = new Invoice(new DateOnly(2025, 02, 02), new DateOnly(2025, 02, 02).AddDays(14), false, 402.04, "Kattoremontti", 1); // 1
            var inv1Lines1 = new InvoiceLine(3, (80 * 3), 1, 1);
            var inv1Lines2 = new InvoiceLine(2, (5.95 * 2), 2, 1);
            var inv1Lines3 = new InvoiceLine(5, 5, 3, 1);
            var inv1Lines4 = new InvoiceLine(123, (123 * 1.18), 4, 1);

            using (var db = new InvoiceDbContext())
            {
                try
                {
                    db.Customers.Add(cust);
                    db.SaveChanges();

                    db.Products.AddRange(
                        prod,
                        prod2,
                        prod3,
                        prod4
                        );
                    db.SaveChanges();

                    db.Invoices.Add(inv1);
                    db.SaveChanges();

                    db.InvoiceLines.AddRange(
                        inv1Lines1,
                        inv1Lines2,
                        inv1Lines3,
                        inv1Lines4
                        );
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting test data into db: {ex}");
                }            
            }
        }
    }
}
