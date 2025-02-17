using System.Windows.Controls;

namespace InvoiceHandler.Pages
{
    public partial class Invoices : UserControl
    {
        public Invoices()
        {
            InitializeComponent();
            invoiceContent.Content = new InvoiceCreate(); // default näkymä
            btnAddInvoice.IsChecked = true;
        }

        private void btnInvoicemenu_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            btnAddInvoice.IsChecked = false;
            btnSearchInvoice.IsChecked = false;

            if (sender == btnAddInvoice)
            {
                invoiceContent.Content = new InvoiceCreate();
                btnAddInvoice.IsChecked = true;
            }
            else if (sender == btnSearchInvoice)
            {
                invoiceContent.Content = new searchInvoice();
                btnSearchInvoice.IsChecked = true;
            }
        }
    }
}
