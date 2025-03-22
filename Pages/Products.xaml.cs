using System.Windows;
using System.Windows.Controls;
using InvoiceHandler.Data;

namespace InvoiceHandler.Pages
{
    public partial class Products : UserControl
    {
        public List<Product> productList { get; set; } = [];
        public int nextProdID { get; set; }
        private bool[] IsPlaceholderText = [true, true, true];
        private bool[] IsPlaceholderTextEdit = [true, true, true, true];

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

        // Create product button click event
        private void CreateProdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaceholderText.Any(x => x))
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            string name = prodCreateNameInput.Text;
            string unit = prodCreateUnitInput.Text;

            if (productList.Any(p => p.Name == name && p.Unit == unit))
            {
                MessageBox.Show("Product already exists");
                return;
            }

            if (double.TryParse(prodCreatePPUInput.Text, out double ppu))
            {
                var newProduct = new Product(name, unit, ppu);
                if (dbActions.CreateProduct(newProduct))
                {
                    MessageBox.Show("Product created successfully");
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.mainContent.Content = new Products();
                }
                else
                {
                    MessageBox.Show("Error creating product");
                }
            }
            else
            {
                MessageBox.Show("Invalid price per unit");
            }
        }

        // Edit product button click event
        private void EditCustBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsPlaceholderTextEdit.Any(x => x))
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }

            if (int.TryParse(prodIdEditInput.Text, out int id))
            {
                var foundProduct = productList.FirstOrDefault(p => p.ID == id);

                if (foundProduct != null)
                {
                    if (double.TryParse(prodPPUEditInput.Text, out double ppu))
                    {
                        foundProduct.Name = prodNameEditInput.Text;
                        foundProduct.Unit = prodUnitEditInput.Text;
                        foundProduct.PricePerUnit = ppu;

                        if (dbActions.EditProduct(foundProduct))
                        {
                            MessageBox.Show("Product edited successfully");
                            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                            mainWindow.mainContent.Content = new Products();
                        }
                        else
                        {
                            MessageBox.Show("Error editing product");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid price per unit input");
                    }
                }
                else
                {
                    MessageBox.Show("Product not found");
                }
                
            }
            else
            {
                MessageBox.Show("Invalid ID");
            }
        }

        // input field focus events
        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            // Create product input fields
            if (sender == prodCreateNameInput || sender == prodCreateUnitInput || sender == prodCreatePPUInput)
            {
                int index = (sender == prodCreateNameInput) ? 0 : (sender == prodCreateUnitInput) ? 1 : 2;

                if (IsPlaceholderText[index])
                {
                    if (sender is TextBox tb)
                    {
                        tb.Text = "";
                        IsPlaceholderText[index] = false;
                    }
                }
            }

            // Edit product input fields
            if (sender == prodIdEditInput || sender == prodNameEditInput || sender == prodUnitEditInput || sender == prodPPUEditInput)
            {
                int index = (sender == prodIdEditInput) ? 0 : (sender == prodNameEditInput) ? 1 : (sender == prodUnitEditInput) ? 2 : 3;

                if (IsPlaceholderTextEdit[index])
                {
                    if (sender is TextBox tb)
                    {
                        tb.Text = "";
                        IsPlaceholderTextEdit[index] = false;
                    }
                }
            }
        }

        // input field lost focus events
        private void Input_LostFocus(object sender, RoutedEventArgs e)
        {
            // Create product input fields
            if (sender == prodCreateNameInput || sender == prodCreateUnitInput || sender == prodCreatePPUInput)
            {
                int index = (sender == prodCreateNameInput) ? 0 : (sender == prodCreateUnitInput) ? 1 : 2;
                string text = (sender == prodCreateNameInput) ? "Type name..." : (sender == prodCreateUnitInput) ? "Type unit..." : "Type PPU...";

                if (sender is TextBox tb)
                {
                    if (string.IsNullOrWhiteSpace(tb.Text))
                    {
                        tb.Text = text;
                        IsPlaceholderText[index] = true;
                    }
                }
            }

            // Edit product input fields
            if (sender == prodIdEditInput || sender == prodNameEditInput || sender == prodUnitEditInput || sender == prodPPUEditInput)
            {
                int index = (sender == prodIdEditInput) ? 0 : (sender == prodNameEditInput) ? 1 : (sender == prodUnitEditInput) ? 2 : 3;
                string text = (sender == prodIdEditInput) ? "Type product ID..." : (sender == prodNameEditInput) ? "Type new name..." : (sender == prodUnitEditInput) ? "Type new unit..." : "Type new ppu...";

                if (sender is TextBox tb)
                {
                    if (string.IsNullOrWhiteSpace(tb.Text))
                    {
                        tb.Text = text;
                        IsPlaceholderTextEdit[index] = true;

                        if (index == 0)
                        {
                            prodNameEditInput.Text = "Type new name...";
                            prodNameEditInput.IsEnabled = false;
                            prodUnitEditInput.Text = "Type new unit...";
                            prodUnitEditInput.IsEnabled = false;
                            prodPPUEditInput.Text = "Type new ppu...";
                            prodPPUEditInput.IsEnabled = false;
                        }
                    }

                    if (index == 0)
                    {
                        if (int.TryParse(tb.Text, out int id))
                        {
                            var foundProduct = productList.FirstOrDefault(p => p.ID == id);

                            if (foundProduct != null)
                            {
                                prodNameEditInput.Text = foundProduct.Name;
                                prodNameEditInput.IsEnabled = true;
                                prodUnitEditInput.Text = foundProduct.Unit;
                                prodUnitEditInput.IsEnabled = true;
                                prodPPUEditInput.Text = foundProduct.PricePerUnit.ToString();
                                prodPPUEditInput.IsEnabled = true;
                                IsPlaceholderTextEdit = [false, false, false, false];
                            }
                            else
                            {
                                MessageBox.Show("Product not found");
                                tb.Text = text;
                                IsPlaceholderTextEdit[index] = true;
                            }
                        }
                        else
                        {
                            tb.Text = "Invalid ID";
                            IsPlaceholderTextEdit[index] = true;
                        }
                    }
                }
            }
        }

        // input field key down events
        private void prodIdEditInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Input_LostFocus(sender, e);
            }
        }
    }
}

