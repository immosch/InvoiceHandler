using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;


namespace InvoiceHandler.Pages
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            int invoiceCount = dbActions.GetInvoiceCount();
            totInv.Text = invoiceCount.ToString();

            double totalRevenue = dbActions.GetTotalRevenue();
            totalRevenue = Math.Round(totalRevenue, 2);
            totRev.Text = totalRevenue.ToString() + "$";

            int customerCount = dbActions.GetCustomerCount();
            totCust.Text = customerCount.ToString();
            
            int productsSold = dbActions.GetProductsSold();
            prodSold.Text = productsSold.ToString();
        }

        private void dash_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            main.btnDash.IsChecked = false;
            main.btnInvoices.IsChecked = false;
            main.btnCustomers.IsChecked = false;
            main.btnProducts.IsChecked = false;

            if (sender == totalInvoices || sender == totalRevenue)
            {
                main.mainContent.Content = new Invoices();
                main.btnInvoices.IsChecked = true;
            }
            else if (sender == totalCustomers)
            {
                main.mainContent.Content = new Customers();
                main.btnCustomers.IsChecked = true;
            }
            else if (sender == productsSold)
            {
                main.mainContent.Content = new Products();
                main.btnProducts.IsChecked = true;
            }
        }
    }
}
