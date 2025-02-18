using System.Windows.Controls;
using InvoiceHandler.PageDataModels;

namespace InvoiceHandler.Pages
{
    public partial class InvoiceCreate : UserControl
    {
        public InvoiceCreate()
        {
            InitializeComponent();
            this.DataContext = new InvoiceData();
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
