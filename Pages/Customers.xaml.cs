using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class Customers : UserControl
    {
        public List<Customer> CustomersList { get; set; } = [];
        public int nextCustID { get; set; }
        public bool[] IsPlaceholderText = [true, true];
        public bool[] IsEditPlaceholderText = [true, true, true];
        public bool IsRemovePlaceholderText = true;

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
            if (IsPlaceholderText.Any(x => x))
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

        private void EditCustBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsEditPlaceholderText.Any(x => x))
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            int id = int.Parse(custIdEditInput.Text);

            var foundCust = CustomersList.FirstOrDefault(c => c.ID == id);

            if (foundCust != null)
            {
                foundCust.Name = custNameEditInput.Text;
                foundCust.Address = custAddressEditInput.Text;

                if (dbActions.EditCustomer(foundCust))
                {
                    MessageBox.Show("Customer updated successfully.");
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.mainContent.Content = new Customers();
                }
                else
                {
                    MessageBox.Show("Failed to update customer.");
                }
            }
            else
            {
                MessageBox.Show("Customer not found.");
                return;
            }
        }
        private void custInput_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = (sender == custNameInput) ? 0 : 1;

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
            string text = (sender == custNameInput) ? "Type name..." : "Type address...";
            int index = (sender == custNameInput) ? 0 : 1;

            if (sender is TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = text;
                    IsPlaceholderText[index] = true;
                }
            }
        }

        private void custEditInput_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = (sender == custIdEditInput) ? 0 : (sender == custNameEditInput) ? 1 : 2;

            if (sender is TextBox tb)
            {
                if (IsEditPlaceholderText[index])
                {
                    tb.Text = "";
                    IsEditPlaceholderText[index] = false;
                }
            }
        }

        private void custEditInput_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            int index = (sender == custIdEditInput) ? 0 : (sender == custNameEditInput) ? 1 : 2;
            string text = (sender == custIdEditInput) ? "Type customer ID..." : (sender == custNameEditInput) ? "Type new name..." : "Type new address...";

            if (sender is TextBox tb)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.Text = text;
                    IsEditPlaceholderText[index] = true;

                    if (sender == custIdEditInput)
                    {
                        custNameEditInput.Text = "Type new name...";
                        custAddressEditInput.Text = "Type new address...";
                        custNameEditInput.IsEnabled = false;
                        custAddressEditInput.IsEnabled = false;
                    }
                }
            }

            if (sender == custIdEditInput)
            {
                if (int.TryParse(custIdEditInput.Text, out int id))
                {
                    var customer = CustomersList.FirstOrDefault(c => c.ID == id);
                    if (customer != null)
                    {
                        custNameEditInput.Text = customer.Name;
                        custNameEditInput.IsEnabled = true;
                        custAddressEditInput.Text = customer.Address;
                        custAddressEditInput.IsEnabled = true;
                        IsEditPlaceholderText = [false, false, false];
                    }
                    else
                    {
                        MessageBox.Show("Customer not found.");
                    }
                }
                else
                {
                    if (!IsEditPlaceholderText[0]) MessageBox.Show("Invalid customer ID");
                }
            }
        }

        private void custIdEditInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                custEditInput_LostFocus(sender, e);
            }
        }
    }
}
