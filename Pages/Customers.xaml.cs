using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class Customers : UserControl
    {
        public List<Customer> CustomersList { get; set; } = [];
        public int nextCustID { get; set; }
        public bool[] IsPlaceholderText = [true,true];

        public Customers()
        {
            InitializeComponent();
            DataContext = this;
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            CustomersList = dbActions.GetAllCustomers();
            nextCustID = CustomersList.Count + 1;
        }

        private void CreateCustBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsPlaceholderText[0] || IsPlaceholderText[1])
            {
                MessageBox.Show("Please fill in all fields before creating a new customer.");
                return;
            }

            string name = custNameInput.Text;

            if (CustomersList.Any(c => c.Name == name))
            {
                MessageBox.Show("Customer already exists.");
                return;
            }

            string address = custAddressInput.Text;
            var newCust = new Customer(name, address);

            if (dbActions.CreateCustomer(newCust))
            {
                MessageBox.Show("Customer added successfully.");

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.mainContent.Content = new Customers();
            }
            else
            {
                MessageBox.Show("Failed to add customer.");
            }
        }

        private void custInput_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = sender == custNameInput ? 0 : 1;

            if (sender is TextBox tb)
            {
                if (IsPlaceholderText[index])
                {
                    tb.Text = "";
                    IsPlaceholderText[index] = false;
                }
            }
        }

        private void custInput_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            string text = sender == custNameInput ? "Type customer name..." : "Type customer address...";
            int index = sender == custNameInput ? 0 : 1;

            if (sender is TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = text;
                    IsPlaceholderText[index] = true;
                }
            }
        }
    }
}
