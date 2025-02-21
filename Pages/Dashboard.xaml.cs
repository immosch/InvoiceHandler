using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;


namespace InvoiceHandler.Pages
{
    public partial class Dashboard : UserControl
    {
        public int InvoiceCount { get; set; }
        public string TotalRevenue { get; set; } = "0$";
        public int CustomerCount { get; set; }
        public int ProductsSold { get; set; }
        public SeriesCollection? Series { get; set; }
        public string[]? Labels { get; set; }
        public Func<double, string>? Yformatter { get; set; }

        public Dashboard()
        {
            InitializeComponent();
            DataContext = this;
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // dashboard statistics setup
            InvoiceCount = dbActions.GetInvoiceCount();
            TotalRevenue = dbActions.GetTotalRevenue().ToString() + "$";
            CustomerCount = dbActions.GetCustomerCount();
            ProductsSold = dbActions.GetProductsSold();

            // YTD revenue chart setup
            var revenueData = dbActions.GetRevenueData();
            double[] monthlyRevenue = new double[12];
            foreach (RevenueData i in revenueData)
            {
                monthlyRevenue[i.Month - 1] = i.TotalRevenue;
            }

            Series =
            [
                new LineSeries
                {
                    Title = "Revenue:",
                    Values = new ChartValues<double>(monthlyRevenue),
                    LineSmoothness = 0.5,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 10,
                    Stroke = Brushes.Red,
                    Fill = Brushes.Transparent
                }
            ];

            Labels =
            [
                "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            ];

            Yformatter = value => value.ToString("C");

        }

        // Change the content based on which statistic button is pressed.
        private void dash_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;

            main.btnDash.IsChecked = false;
            main.btnInvoices.IsChecked = false;
            main.btnCustomers.IsChecked = false;
            main.btnProducts.IsChecked = false;

            if (sender == totalInvoices || sender == totalRevenue)
            {
                main.mainContent.Content = new Invoices();
                main.btnInvoices.IsChecked = true;
            }
            else if (sender == totalCustomers)
            {
                main.mainContent.Content = new Customers();
                main.btnCustomers.IsChecked = true;
            }
            else if (sender == productsSold)
            {
                main.mainContent.Content = new Products();
                main.btnProducts.IsChecked = true;
            }
        }
    }
}
