using System.Windows;

namespace InvoiceHandler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainContent.Content = new Pages.Dashboard();
            btnDash.IsChecked = true;
        }

        // tarkistetaan, että mitä nappia on painettu ja vaihdetaan näkymää ja nappien toggleja sen mukaan.
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            btnDash.IsChecked = false;
            btnInvoices.IsChecked = false;
            btnCustomers.IsChecked = false;
            btnProducts.IsChecked = false;

            if (sender == btnDash)
            {
                mainContent.Content = new Pages.Dashboard();
                btnDash.IsChecked = true;
            }
            else if (sender == btnInvoices)
            {
                mainContent.Content = new Pages.Invoices();
                btnInvoices.IsChecked = true;
            }
            else if (sender == btnCustomers)
            {
                mainContent.Content = new Pages.Customers();
                btnCustomers.IsChecked = true;
            }
            else if (sender == btnProducts)
            {
                mainContent.Content = new Pages.Products();
                btnProducts.IsChecked = true;
            }
        }
    }
}