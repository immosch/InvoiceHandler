using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using InvoiceHandler.Data;

namespace InvoiceHandler
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string Invoicer { get; set; } = "-";
        public string InvoicerAddress { get; set; } = "-";
        private string _TimeDisplay = DateTime.Now.ToString("HH:mm:ss");
        public string TimeDisplay
        {
            get => _TimeDisplay;
            set
            {
                _TimeDisplay = value;
                OnPropertyChanged(nameof(TimeDisplay));
            }
        }
        private string _DateDisplay = DateTime.Now.ToString("dd.MM.yyyy");
        public string DateDisplay
        {
            get => _DateDisplay;
            set
            {
                _DateDisplay = value;
                OnPropertyChanged(nameof(DateDisplay));
            }
        }
        public string Version { get; set; } = "-";
        public event PropertyChangedEventHandler? PropertyChanged;
        private DispatcherTimer? timer;

        public MainWindow()
        {
            InitializeComponent();
            mainContent.Content = new Pages.Dashboard(); // default
            btnDash.IsChecked = true;
            DataContext = this;
            LoadMainData();
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadMainData()
        {
            Invoicer = Invoice.InvoicerName;
            InvoicerAddress = Invoice.InvoicerAddress;
            Version = " v0.10";
            Timer_Start();
        }

        // Start Time and Date display timer and update display every second
        private void Timer_Start()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        // Update Time and Date display on interval
        private void Timer_Tick(object? sender, EventArgs e)
        {
            TimeDisplay = DateTime.Now.ToString("HH:mm:ss");
            DateDisplay = DateTime.Now.ToString("dd.MM.yyyy");
        }

        // check which button is clicked and change the content
        private void btnLeftMenu_Click(object sender, RoutedEventArgs e)
        {
            // reset all buttons
            btnDash.IsChecked = false;
            btnInvoices.IsChecked = false;
            btnCustomers.IsChecked = false;
            btnProducts.IsChecked = false;

            if (sender == btnDash)
            {
                mainContent.Content = new Pages.Dashboard();
                btnDash.IsChecked = true;
            }
            else if (sender == btnInvoices)
            {
                mainContent.Content = new Pages.Invoices();
                btnInvoices.IsChecked = true;
            }
            else if (sender == btnCustomers)
            {
                mainContent.Content = new Pages.Customers();
                btnCustomers.IsChecked = true;
            }
            else if (sender == btnProducts)
            {
                mainContent.Content = new Pages.Products();
                btnProducts.IsChecked = true;
            }
        }

        // change the size of the left menu if the window gets too small
        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 1250)
            {
                leftLogo.Visibility = Visibility.Collapsed;
                enviromentInfo.Visibility = Visibility.Collapsed;
                leftMenu.Width = 60;
            }
            else
            {
                leftLogo.Visibility = Visibility.Visible;
                enviromentInfo.Visibility = Visibility.Visible;
                leftMenu.Width = 230;
            }
        }
    }
}