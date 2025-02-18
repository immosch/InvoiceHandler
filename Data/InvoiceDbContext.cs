using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace InvoiceHandler.Data
{
    public class InvoiceDbContext : DbContext
    {       
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DotNetEnv.Env.Load();
            string connectionString = Env.GetString("DATABASE_URL");

            optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 29)));
        }
    }
}
