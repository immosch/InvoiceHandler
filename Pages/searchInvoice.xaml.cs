using InvoiceHandler.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace InvoiceHandler.Pages
{
    public partial class searchInvoice : UserControl
    {
        public ObservableCollection<InvoiceDisplay> Displays { get; set; } = [];
        public bool IsPlaceholdertext = true;
        public bool IsPlaceholdertextId = true;
        public bool IsMultipleSearches { get; set; } = false;
        private List<InvoiceDisplay> placeholder = [];
        private List<Invoice> invoices = [];

        public searchInvoice()
        {
            InitializeComponent();
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            placeholder = dbActions.GetInvoiceDisplays();
            Displays = new ObservableCollection<InvoiceDisplay>(placeholder);
            invoices = dbActions.GetInvoices();
        }

        private void EditInvBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaceholdertextId)
            {
                MessageBox.Show("Please enter an invoice ID.");
                return;
            }

            if (int.TryParse(invIdInput.Text, out int id))
            {
                var invoice = invoices.FirstOrDefault(i => i.ID == id);

                if (invoice != null)
                {
                    invoice.PaymentStatus = true;

                    if (dbActions.EditInvoice(invoice))
                    {
                        MessageBox.Show("Invoice payment status updated successfully.");
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                        Invoices invoices = (Invoices)mainWindow.mainContent.Content;
                        invoices.invoiceContent.Content = new searchInvoice();

                    }
                    else
                    {
                        MessageBox.Show("Failed to edit invoice.");
                    }
                }
                else
                {
                    MessageBox.Show("Invoice not found.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid invoice ID.");
            }
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {          
            if (sender == searchTxt && IsPlaceholdertext) 
            {
                searchTxt.Text = "";
                IsPlaceholdertext = false;
            }
            else if (sender == invIdInput && IsPlaceholdertextId)
            {
                invIdInput.Text = "";
                IsPlaceholdertextId = false;
            }
        }

        private void Txt_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = (sender == searchTxt) ? "You can search using invoice ID or company name..." : "Enter invoice ID...";

            if (sender == searchTxt)
            {
                if (string.IsNullOrWhiteSpace(searchTxt.Text))
                {
                    searchTxt.Text = text;
                    IsPlaceholdertext = true;
                    Displays.Clear();
                    foreach (var inv in placeholder)
                    {
                        Displays.Add(inv);
                    }
                }
            }
            else if (sender == invIdInput)
            {
                if (string.IsNullOrWhiteSpace(invIdInput.Text))
                {
                    invIdInput.Text = text;
                    IsPlaceholdertextId = true;
                }


            }
        }

        private void searchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (!IsPlaceholdertext)
            {
                if (string.IsNullOrWhiteSpace(searchTxt.Text))
                {
                    Displays.Clear();
                    foreach (var inv in placeholder)
                    {
                        Displays.Add(inv);
                    }
                    return;
                }

                List<InvoiceDisplay> filteredInvoices = [];

                if (int.TryParse(searchTxt.Text, out int id))
                {
                    filteredInvoices = placeholder.Where(i => i.ID == id).ToList();
                }
                else
                {
                    filteredInvoices = placeholder.Where(i => i.CustomerName.ToLower().Contains(searchTxt.Text.ToLower())).ToList();
                }

                Displays.Clear();
                foreach (var inv in filteredInvoices)
                {
                    Displays.Add(inv);
                }
                IsMultipleSearches = true;
            }
            else
            {
                Displays.Clear();
                foreach (var inv in placeholder)
                {
                    Displays.Add(inv);
                }
            }
        }
    }
}
