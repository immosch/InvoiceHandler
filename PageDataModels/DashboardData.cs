using InvoiceHandler.Data;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows;
using System.Windows.Media;

namespace InvoiceHandler.PageDataModels
{
    class DashboardData
    {
        public int InvoiceCount { get; set; }
        public string TotalRevenue { get; set; } = "0$";
        public int CustomerCount { get; set; }
        public int ProductsSold { get; set; }
        public SeriesCollection? Series { get; set; }
        public string[]? Labels { get; set; }
        public Func<double, string>? Yformatter { get; set; }


        public DashboardData()
        {
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

            Series = new SeriesCollection
            {
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
            };

            Labels = new[]
            {
                "Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
            };

            Yformatter = value => value.ToString("C");

        }
    }
}
