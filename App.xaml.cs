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
                MessageBox.Show($"Error during database setup: {ex.Message}");
            }
        }

        private static void PopulateDB()
        {
            // customers
            var cust = new Customer("Malli Asiakas", "Asiakkaantie 22, 80200 Joensuu"); // 1
            var cust2 = new Customer("Rakennus asd", "Asiakkaankatu 22, 80400 Ylämylly"); // 2
            var cust3 = new Customer("TaloTaikurit", "Pihlajakatu 23B, 00140 Helsinki"); // 3

            // products
            var prod = new Product("Work", "h", 80); // 1
            var prod2 = new Product("Silicone", "Pcs", 5.95); // 2
            var prod3 = new Product("Sealant", "Pcs", 1); // 3
            var prod4 = new Product("Lumber", "m", 1.18); // 4

            // invoice 1
            var inv1 = new Invoice(new DateOnly(2025, 02, 02), new DateOnly(2025, 02, 02).AddDays(14), false, 402.04, "Kattoremontti", 1); // 1
            var inv1Lines1 = new InvoiceLine(3, (80 * 3), 1, 1);
            var inv1Lines2 = new InvoiceLine(2, (5.95 * 2), 2, 1);
            var inv1Lines3 = new InvoiceLine(5, 5, 3, 1);
            var inv1Lines4 = new InvoiceLine(123, (123 * 1.18), 4, 1);

            // invoice 2
            var inv2 = new Invoice(new DateOnly(2025, 01, 05), new DateOnly(2025, 01, 05).AddDays(14), false, 160, "Työkartoitus", 1); // 2
            var inv2Lines1 = new InvoiceLine(2, (80 * 2), 1, 2);

            using (var db = new InvoiceDbContext())
            {
                try
                {
                    db.Customers.AddRange(
                        cust,
                        cust2,
                        cust3
                        );
                    db.SaveChanges();

                    db.Products.AddRange(
                        prod,
                        prod2,
                        prod3,
                        prod4
                        );
                    db.SaveChanges();

                    db.Invoices.AddRange(
                        inv1,
                        inv2
                        );
                    db.SaveChanges();

                    db.InvoiceLines.AddRange(
                        inv1Lines1,
                        inv1Lines2,
                        inv1Lines3,
                        inv1Lines4,
                        inv2Lines1
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
