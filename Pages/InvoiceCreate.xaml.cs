using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class InvoiceCreate : UserControl
    {
        public List<Customer> CustomerList { get; set; } = [];
        public List<Product> ProductList { get; set; } = [];
        public DateTime CurrentDate { get; set; }
        public int NextID { get; set; }
        public static ObservableCollection<Grid> InvoiceLines { get; set; } = [];


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
        }

        private void addInf_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (addInf.Text == "You can write additional information about the invoice here...") addInf.Text = "";
        }

        private void addInf_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (addInf.Text == "") addInf.Text = "You can write additional information about the invoice here...";
        }

        private void PlusButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AddInvoiceLine();
        }

        private void Trashbutton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoveInvoiceLine();
        }

        private void AddInvoiceLine()
        {

            RowDefinition row = new()
            { Height = GridLength.Auto };
            prodCont.RowDefinitions.Add(row);

            Grid newLine = CreateInvoiceLineElement();

            int rowIndex = prodCont.RowDefinitions.Count - 1;

            Grid.SetRow(newLine, rowIndex - 1);
            Grid.SetRow(plusGrid, rowIndex);

            InvoiceLines.Add(newLine);
            prodCont.Children.Add(newLine);
        }

        private void RemoveInvoiceLine()
        {
            int toRemoveIndex = prodCont.RowDefinitions.Count - 2;

            var elementsToRemove = prodCont.Children.Cast<UIElement>()
                .Where(e => Grid.GetRow(e) == toRemoveIndex)
                .ToList();

            foreach (var element in elementsToRemove)
            {
                prodCont.Children.Remove(element);
            }

            prodCont.RowDefinitions.RemoveAt(toRemoveIndex);
            Grid.SetRow(plusGrid, toRemoveIndex + 1);
            if (InvoiceLines.Count > 0) InvoiceLines.RemoveAt(0);

        }


        private Grid CreateInvoiceLineElement()
        {
            Grid grid = new()
            {
                Height = 65,
                MinHeight = 65,
                Margin = new Thickness(250, 0, 250, 0),
            };

            for (int i = 0; i < 5; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            Button imageButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent,
                Cursor = Cursors.Hand
            };

            imageButton.Click += Trashbutton_Click;

            Grid buttonGrid = new();
            Image trashImage = new()
            {
                Source = new BitmapImage(new Uri("pack://application:,,,/Assets/Icons/trash_16px.png")),
                Visibility = Visibility.Visible
            };

            buttonGrid.Children.Add(trashImage);
            imageButton.Content = buttonGrid;
            grid.Children.Add(imageButton);

            Border comboBorder = new()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(10)
            };
            Grid.SetColumn(comboBorder, 1);

            ComboBox prodCombo = new()
            {
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Style = Application.Current.TryFindResource("ComboBoxinvoice") as Style,
                ItemsSource = ProductList,
                DisplayMemberPath = "Name",
                SelectedIndex = 0
            };

            comboBorder.Child = prodCombo;
            grid.Children.Add(comboBorder);

            Border textBorder = new()
            {
                BorderThickness = new Thickness(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(10)
            };
            Grid.SetColumn(textBorder, 2);

            TextBox amountTextBox = new()
            {
                Text = "Type amount...",
                FontSize = 22,
                Background = Brushes.Transparent,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            textBorder.Child = amountTextBox;
            grid.Children.Add(textBorder);

            TextBlock priceText = new()
            {
                FontSize = 22,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = (prodCombo.SelectedItem as Product)?.PricePerUnit.ToString() ?? "0"
            };

            Binding PPUbind = new()
            {
                Path = new PropertyPath("SelectedItem.PricePerUnit"),
                Source = prodCombo,
                FallbackValue = "0",
                Mode = BindingMode.OneWay,
                StringFormat = "{0:F2}"
            };
            priceText.SetBinding(TextBlock.TextProperty, PPUbind);

            Grid.SetColumn(priceText, 3);
            grid.Children.Add(priceText);

            TextBlock staticText = new()
            {
                Text = "1234",
                FontSize = 22,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(staticText, 4);
            grid.Children.Add(staticText);

            return grid;
        }
    }
}