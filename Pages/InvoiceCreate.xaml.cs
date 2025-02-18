using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class InvoiceCreate : UserControl
    {
        public InvoiceCreate()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now;

            List<Customer> CustomersComboSource = dbActions.CustomersToList() ?? [];
            customerCombo.ItemsSource = CustomersComboSource.Select(c => c.Name);
        }

        private void addInf_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (addInf.Text == "You can write additional information about the invoice here...") addInf.Text = "";
        }

        private void addInf_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (addInf.Text == "") addInf.Text = "You can write additional information about the invoice here...";
        }
    }
}
