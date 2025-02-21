using System.Windows;

namespace InvoiceHandler
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainContent.Content = new Pages.Dashboard(); // default
            btnDash.IsChecked = true;
        }

        // check which button is clicked and change the content
        private void btnLeftMenu_Click(object sender, RoutedEventArgs e)
        {
            // reset all buttons
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