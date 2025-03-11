using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class Customers : UserControl
    {
        public List<Customer> CustomersList { get; set; } = [];
        public int nextCustID { get; set; }

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

        }
    }
}
