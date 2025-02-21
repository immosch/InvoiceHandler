using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class InvoiceCreate : UserControl
    {
        public List<Customer> CustomerList { get; set; } = [];
        public List<Product> ProductList { get; set; } = [];
        public DateTime CurrentDate { get; set; }
        public int NextID { get; set; }
        public static ObservableCollection<Visibility> IsLineVisible { get; set; } = [];


        public InvoiceCreate()
        {
            InitializeComponent();
            DataContext = this;
            LoadInvoiceData();
        }

        private void LoadInvoiceData()
        {
            CurrentDate = DateTime.Now;
            CustomerList = dbActions.CustomersToList();
            NextID = dbActions.GetNextInvoiceID();
            ProductList = dbActions.GetProducts();
            IsLineVisible = [Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed, Visibility.Collapsed];
        }

        private void addInf_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (addInf.Text == "You can write additional information about the invoice here...") addInf.Text = "";
        }

        private void addInf_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (addInf.Text == "") addInf.Text = "You can write additional information about the invoice here...";
        }

        private void LineButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender == btn1)
            {
                if (btn1trash.Visibility == Visibility.Visible)
                {
                    IsLineVisible[0] = Visibility.Hidden;
                    btn1plus.Visibility = Visibility.Visible;
                    btn1trash.Visibility = Visibility.Hidden;
                    btn2plus.Visibility = Visibility.Hidden;
                }
                else
                {

                    IsLineVisible[0] = Visibility.Visible;
                    btn1plus.Visibility = Visibility.Hidden;
                    btn1trash.Visibility = Visibility.Visible;
                    btn2plus.Visibility = Visibility.Visible;
                }
            }
            else if (sender == btn2)
            {
                IsLineVisible[1] = Visibility.Visible;
            }
            else if (sender == btn3)
            {
                IsLineVisible[2] = Visibility.Visible;
            }
            else if (sender == btn4)
            {
                IsLineVisible[3] = Visibility.Visible;
            }
        }
    }
}
