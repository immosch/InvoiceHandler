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
            var cust = new Customer("TaloTaikurit Oy", "Pihlajakatu 23B, 00140 Helsinki"); // 1
            var cust2 = new Customer("Esimerkki Asiakas", "Asiakkaankatu 22, 80400 Ylämylly"); // 2
            var cust3 = new Customer("Malli Asiakas", "Asiakkaantie 22, 80100 Joensuu"); // 3
            var cust4 = new Customer("Joulupukki", "Tähtikuja 1, 96930 Rovaniemi");  // 4


            // products
            var prod = new Product("Work", "h", 80); // 1
            var prod2 = new Product("Silicone", "Pcs", 5.95); // 2
            var prod3 = new Product("Sealant", "Pcs", 1); // 3
            var prod4 = new Product("Lumber", "m", 1.18); // 4
            var prod5 = new Product("Paint", "L", 3); // 5
            var prod6 = new Product("Nails", "kg", 5); // 6

            // invoice 1
            var inv1 = new Invoice(new DateOnly(2025, 02, 02), new DateOnly(2025, 02, 02).AddDays(14), true, 402.04, "Kattoremontti", 3); // 1
            var inv1Lines1 = new InvoiceLine(3, (80 * 3), 1, 1);
            var inv1Lines2 = new InvoiceLine(2, (5.95 * 2), 2, 1);
            var inv1Lines3 = new InvoiceLine(5, 5, 3, 1);
            var inv1Lines4 = new InvoiceLine(123, (123 * 1.18), 4, 1);

            // invoice 2
            var inv2 = new Invoice(new DateOnly(2025, 01, 05), new DateOnly(2025, 01, 05).AddDays(14), false, 160, "Konsultointi", 1); // 2
            var inv2Lines1 = new InvoiceLine(2, (80 * 2), 1, 2);

            // invoice 3
            var inv3 = new Invoice(new DateOnly(2025, 03, 05), new DateOnly(2025, 03, 05).AddDays(30), false, 2021, "Pukin reen korjaus ja maalaus", 4); // 3
            var inv3Lines1 = new InvoiceLine(6, (80 * 6), 1, 3);
            var inv3Lines2 = new InvoiceLine(1200, (1.18 * 1200), 4, 3);
            var inv3Lines3 = new InvoiceLine(40, (3 * 40), 5, 3);
            var inv3Lines4 = new InvoiceLine(1, 5, 6, 3);

            using (var db = new InvoiceDbContext())
            {
                try
                {
                    db.Customers.AddRange(
                        cust,
                        cust2,
                        cust3,
                        cust4
                        );
                    db.SaveChanges();

                    db.Products.AddRange(
                        prod,
                        prod2,
                        prod3,
                        prod4,
                        prod5,
                        prod6
                        );
                    db.SaveChanges();

                    db.Invoices.AddRange(
                        inv1,
                        inv2,
                        inv3
                        );
                    db.SaveChanges();

                    db.InvoiceLines.AddRange(
                        inv1Lines1,
                        inv1Lines2,
                        inv1Lines3,
                        inv1Lines4,
                        inv2Lines1,
                        inv3Lines1,
                        inv3Lines2,
                        inv3Lines3,
                        inv3Lines4
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
