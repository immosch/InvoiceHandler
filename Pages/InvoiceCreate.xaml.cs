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
        public List<Grid> InvoiceLines { get; set; } = [];


        public InvoiceCreate()
        {
            InitializeComponent();
            DataContext = this;
            LoadInvoiceData();
        }

        private void LoadInvoiceData()
        {
            CurrentDate = DateTime.Now;
            CustomerList = dbActions.GetAllCustomers();
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

        // add new invoice line when plus button is clicked
        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
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

        // remove invoice line when trash button is clicked
        private void Removebutton_Click(object sender, System.Windows.RoutedEventArgs e)
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
            if (InvoiceLines.Count > 0) InvoiceLines.RemoveAt(InvoiceLines.Count - 1);
        }

        // Update total amount when amount input gets focus
        private void AmountInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox amountInput) // if sender is TextBox
            {
                if (double.TryParse(amountInput.Text, out double amount)) // if amount is a number
                {

                    if (amountInput.Tag is not Grid parentGrid) return;

                    TextBlock? PPUTextBlock = parentGrid.Children // find price per unit textblock
                        .OfType<TextBlock>()
                        .FirstOrDefault(tb => Grid.GetColumn(tb) == 3);

                    TextBlock? totalAmountTextBlock = parentGrid.Children // find total amount textblock
                        .OfType<TextBlock>()
                        .FirstOrDefault(tb => Grid.GetColumn(tb) == 4);

                    if (PPUTextBlock == null || totalAmountTextBlock == null) return;

                    if (double.TryParse(PPUTextBlock.Text, out double pricePerUnit)) // if price per unit is a number
                    {
                        totalAmountTextBlock.Text = (amount * pricePerUnit).ToString("F2"); // update total amount
                    }
                }
                else
                {
                    amountInput.Text = "";
                    return;
                }

            }
        }

        // Update total amount when amount input loses focus
        private void AmountInput_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox amountInput) // if sender is textbox
            {
                if (double.TryParse(amountInput.Text, out double amount)) // id amount is number
                {

                    if (amountInput.Tag is not Grid parentGrid) return;

                    TextBlock? PPUTextBlock = parentGrid.Children // find price per unit textblock
                        .OfType<TextBlock>()
                        .FirstOrDefault(tb => Grid.GetColumn(tb) == 3);

                    TextBlock? totalAmountTextBlock = parentGrid.Children // find total amount textblock
                        .OfType<TextBlock>()
                        .FirstOrDefault(tb => Grid.GetColumn(tb) == 4);

                    if (PPUTextBlock == null || totalAmountTextBlock == null) return;

                    if (double.TryParse(PPUTextBlock.Text, out double pricePerUnit)) // if price per unit is number
                    {
                        totalAmountTextBlock.Text = (amount * pricePerUnit).ToString("F2"); // update total amount
                    }
                }
                else
                {
                    amountInput.Text = "Type amount...";
                    return;
                }
            }
        }

        // Create invoice
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            var date = datePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(datePicker.SelectedDate.Value) : DateOnly.FromDateTime(DateTime.Today);
            DateOnly dueDate;
            string additionalInfo = addInf.Text == "You can write additional information about the invoice here..." ? "No additional information given" : addInf.Text;
            List<InvoiceLine> invoiceLinesToInsert = [];

            // find out which due date is selected
            switch (dueDateCombo.SelectedIndex)
            {
                case 0:
                    dueDate = date.AddDays(7);
                    break;
                case 1:
                    dueDate = date.AddDays(14);
                    break;
                case 2:
                    dueDate = date.AddDays(24);
                    break;
                case 3:
                    dueDate = date.AddDays(30);
                    break;
                default:
                    dueDate = date.AddDays(14);
                    break;
            }
            double invoiceTotal = 0;

            #region xaml hard coded first line

            var firstLineAmount = double.TryParse(amountText.Text, out double amountFirst) ? amountFirst : 0; // amount of products

            if (prodCombo.SelectedItem is Product selectedProductFirst) // what product is selected
            {
                invoiceLinesToInsert.Add(new InvoiceLine // add invoice line
                {
                    Amount = firstLineAmount,
                    LineTotal = firstLineAmount * selectedProductFirst.PricePerUnit,
                    ProductID = selectedProductFirst.ID,
                    InvoiceID = NextID
                });
                invoiceTotal += amountFirst * selectedProductFirst.PricePerUnit; // update invoice total
            }
            else
            {
                MessageBox.Show("Please select a product!");
                return;
            }
            #endregion

            #region dynamically added lines

            foreach (var invoiceLine in InvoiceLines) // go through all dynamically added lines
            {
                if (invoiceLine.Children[1] is Border comboBorder && comboBorder.Child is ComboBox prodCombo) // find product combo box
                {
                    if (prodCombo.SelectedItem is Product selectedProduct) // if product is selected
                    {
                        if (invoiceLine.Children[2] is Border textBorder && textBorder.Child is TextBox amountTextBox) // find amount of products
                        {
                            if (double.TryParse(amountTextBox.Text, out double amount) && amount > 0) // if amount is a number
                            {
                                invoiceLinesToInsert.Add(new InvoiceLine // add invoice line
                                {
                                    Amount = amount,
                                    LineTotal = amount * selectedProduct.PricePerUnit,
                                    ProductID = selectedProduct.ID,
                                    InvoiceID = NextID,                                    
                                });
                                invoiceTotal += amount * selectedProduct.PricePerUnit; // update invoice total
                            }
                            else
                            {
                                MessageBox.Show("Please type a positive number in the amount field!");
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a product!");
                        return;
                    }
                }
            }
            #endregion


            Invoice? newInvoice;

            if (customerCombo.SelectedItem is Customer selectedCustomer) // if customer is selected
            {
                newInvoice = new Invoice // create new invoice
                {
                    CustomerID = selectedCustomer.ID,
                    InvoiceDate = date,
                    InvoiceDueDate = dueDate,
                    WorkDescription = additionalInfo,
                    PaymentStatus = false,
                    InvoiceTotal = invoiceTotal
                };
            }
            else
            {
                MessageBox.Show("Please select a customer!");
                return;
            }

            if ( newInvoice != null && dbActions.CreateInvoice(newInvoice, invoiceLinesToInsert)) // insert invoice and lines to database
            {
                MessageBox.Show("Invoice created successfully!");

                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.mainContent.Content = new Invoices();
            }
            else
            {
                MessageBox.Show("Error creating invoice!");
            }
        }

        // Dynamic invoice line creation method
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
                Cursor = Cursors.Hand,
                Width = 65,
                Height = 65
            };

            imageButton.Click += Removebutton_Click;

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
                Name = "amountText",
                Text = "Type amount...",
                FontSize = 22,
                Background = Brushes.Transparent,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                Tag = grid
            };

            amountTextBox.GotFocus += AmountInput_GotFocus;
            amountTextBox.LostFocus += AmountInput_LostFocus;

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

            TextBlock totalAmount = new()
            {
                Text = "0.00",
                FontSize = 22,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            Grid.SetColumn(totalAmount, 4);
            grid.Children.Add(totalAmount);

            return grid;
        }
    }
}