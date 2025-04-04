﻿using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using System.Globalization;


namespace InvoiceHandler.Pages
{
    public partial class Dashboard : UserControl
    {
        public string InvoiceCount { get; set; }
        public string TotalRevenue { get; set; } = "0$";
        public string CustomerCount { get; set; }
        public string ProductsSold { get; set; }
        public SeriesCollection? Series { get; set; }
        public string[] Labels { get; set; } = [];
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
            InvoiceCount = dbActions.GetInvoiceCount().ToString("N0");
            TotalRevenue = dbActions.GetTotalRevenue().ToString("C", new System.Globalization.CultureInfo("en-US"));
            CustomerCount = dbActions.GetCustomerCount().ToString("N0");
            ProductsSold = dbActions.GetProductsSold().ToString("N0");

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
                    Stroke = Brushes.Black,
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

        // Change the size of the left menu based on the window size.
        private void Dashboard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 900)
            {
                Grid.SetRow(chartCont, 3);
                Grid.SetColumn(chartCont, 0);
                Grid.SetColumnSpan(chartCont, 2);
            }
            else
            {
                Grid.SetRow(chartCont, 1);
                Grid.SetColumn(chartCont, 2);
                Grid.SetColumnSpan(chartCont, 1);
            }
        }
    }
}
