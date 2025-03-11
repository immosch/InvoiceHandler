using InvoiceHandler.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace InvoiceHandler.Pages
{
    public partial class searchInvoice : UserControl
    {
        public ObservableCollection<InvoiceDisplay> Displays { get; set; } = [];
        public bool IsPlaceholdertext { get; set; } = true;
        public bool IsMultipleSearches { get; set; } = false;
        private List<InvoiceDisplay> placeholder = [];

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
        }

        private void searchTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            if (searchTxt.Text == "You can search using invoice ID or company name...")
            {
                searchTxt.Text = "";
                IsPlaceholdertext = false;
            }
        }

        private void searchTxt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchTxt.Text == "")
            {
                searchTxt.Text = "You can search using invoice ID or company name...";
                Displays.Clear();
                foreach (var il in placeholder)
                {
                    Displays.Add(il);
                }
                IsPlaceholdertext = true;
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!IsPlaceholdertext)
            {
                if (IsMultipleSearches)
                {
                    Displays.Clear();
                    foreach (var il in placeholder)
                    {
                        Displays.Add(il);
                    }
                }

                if (int.TryParse(searchTxt.Text, out int id))
                {
                    var filteredDisplays = Displays.Where(i => i.ID == id).ToList();
                    Displays.Clear();
                    foreach(var fd in filteredDisplays)
                    {
                        Displays.Add(fd);
                    }
                    IsMultipleSearches = true;
                }
                else
                {
                    var filteredDisplays = Displays.Where(i => i.CustomerName.ToLower().Contains(searchTxt.Text.ToLower())).ToList();
                    Displays.Clear();
                    foreach (var fd in filteredDisplays)
                    {
                        Displays.Add(fd);
                    }
                    IsMultipleSearches = true;
                }
            }
            else
            {
                if (IsMultipleSearches)
                {
                    return;
                }
                MessageBox.Show("Please enter a search term.");
                return;
            }
        }
    }
}
