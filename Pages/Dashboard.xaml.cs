using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.PageDataModels;


namespace InvoiceHandler.Pages
{
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            this.DataContext = new DashboardData();
        }

        // Change the content based on which statistic button is pressed.
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
