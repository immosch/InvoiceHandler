using System.Windows.Controls;

namespace InvoiceHandler.Pages
{
    /// <summary>
    /// Interaction logic for addInvoice.xaml
    /// </summary>
    public partial class InvoiceCreate : UserControl
    {
        public InvoiceCreate()
        {
            InitializeComponent();
            datePicker.SelectedDate = DateTime.Now;
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
