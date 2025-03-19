using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class Products : UserControl
    {
        public List<Product> productList { get; set; } = [];
        public int nextProdID { get; set; }

        public Products()
        {
            InitializeComponent();
            DataContext = this;
            LoadProductData();
        }

        private void LoadProductData()
        {
            productList = dbActions.GetProducts();
            nextProdID = productList.Count + 1;
        }

        private void CreateProdBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditCustBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
