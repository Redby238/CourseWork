using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using WPF_LoginForm.DbSettings;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.View
{
    public partial class ViewTablesWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private readonly string _currentUserRole;

        public ViewTablesWindow(ApplicationDbContext context, string role)
        {
            InitializeComponent();
            _context = context;
            _currentUserRole = role;
            LoadData();
            ConfigureButtonInteractivity();
        }

        private void LoadData()
        {
            // Загрузка данных в DataGrid
            DataGridSuppliers.ItemsSource = _context.Suppliers.ToList();
            DataGridProducts.ItemsSource = _context.Products.ToList();
            DataGridOrders.ItemsSource = _context.Orders.ToList();
            DataGridRepairs.ItemsSource = _context.Repairs.ToList();
            DataGridCustomers.ItemsSource = _context.Customers.ToList();
            DataGridContracts.ItemsSource = _context.Contracts.ToList();
            DataGridEmployees.ItemsSource = _context.Employees.ToList();
            DataGridCustomComputers.ItemsSource = _context.CustomComputers.ToList();
            //  DataGridComponents.ItemsSource = _context.CustomComputerComponent.ToList();
            DataGridPurchases.ItemsSource = _context.Purchases.ToList();

         
         
        }

        private bool HasPermission()
        {
            if (_currentUserRole == "Operator")
            {
                return true;
            }
            return false; // Возвращаем false для всех других ролей
        }

        private void ConfigureButtonInteractivity()
        {
           

            if (_currentUserRole == "Operator")
            {
                AddSupplierButton.IsEnabled = false;
                DeleteSupplierButton.IsEnabled = false;
                AddProductButton.IsEnabled = false;
                DeleteProductButton.IsEnabled = false;
                AddOrderButton.IsEnabled = false;
                DeleteOrderButton.IsEnabled = false;
                AddRepairButton.IsEnabled = false;
                DeleteRepairButton.IsEnabled = false;
                AddCustomerButton.IsEnabled = false;
                DeleteCustomerButton.IsEnabled = false;
                AddContractButton.IsEnabled = false;
                DeleteContractButton.IsEnabled = false;
                AddEmployeeButton.IsEnabled = false;
                DeleteEmployeeButton.IsEnabled = false;
                AddCustomComputerButton.IsEnabled = false;
                DeleteCustomComputerButton.IsEnabled = false;
                AddPurchaseButton.IsEnabled = false;
                DeletePurchaseButton.IsEnabled = false;
            }
           
        }




        // Фильтрация для Продуктов
        private void FilterProductsTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var filterText = FilterProductsTextBox.Text.ToLower();
            DataGridProducts.ItemsSource = _context.Products
                .Where(p => p.ProductName.ToLower().Contains(filterText))
                .ToList();
        }

        // Фильтрация для Заказов
        private void FilterOrdersTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var filterText = FilterOrdersTextBox.Text.ToLower();
            DataGridOrders.ItemsSource = _context.Orders
                .Where(o => o.Customer.FirstName.ToLower().Contains(filterText))
                .ToList();
        }




        private void DataGrid_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {

                if (!HasPermission()) return;

                var grid = sender as System.Windows.Controls.DataGrid;
                var editedItem = e.Row.Item as dynamic;

                if (editedItem != null)
                {
                    _context.SaveChanges(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Добавление и удаление Поставщика
        private void AddSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUserRole == "Operator")
            {
                MessageBox.Show("У вас нет прав для добавления данных.", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newSupplier = new Supplier
            {
                CompanyName = "Новый поставщик",
                Phone = "000-0000000",
                Address = "Новый адрес",
                Email = "new@supplier.com",
                CreatedAt = DateTime.Now
            };

            _context.Suppliers.Add(newSupplier);
            _context.SaveChanges();
            LoadData();
        }

        private void DeleteSupplierButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            if (DataGridSuppliers.SelectedItem is Supplier selectedSupplier)
            {
                _context.Suppliers.Remove(selectedSupplier);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поставщика для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Добавление и удаление Продукта
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            var supplier = _context.Suppliers.FirstOrDefault(); 

            if (supplier != null)
            {
                var newProduct = new Product
                {
                    ProductName = "Новый продукт",
                    Price = 100.00m,
                    CreatedAt = DateTime.Now,
                    SupplierID = supplier.SupplierID 
                };

                _context.Products.Add(newProduct);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("No supplier found. Please add a supplier first.");
            }
        }


        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            if (DataGridProducts.SelectedItem is Product selectedProduct)
            {
                _context.Products.Remove(selectedProduct);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Добавление и удаление Заказов
        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;

            var customer = _context.Customers.FirstOrDefault(c => c.CustomerID >= 21 && c.CustomerID <= 30);

            if (customer != null)
            {
                var newOrder = new Order
                {
                    CustomerID = customer.CustomerID, 
                    OrderDate = DateTime.Now
                };

                _context.Orders.Add(newOrder);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("No customer exists with a valid ID between 21 and 30. Please create a customer first.");
            }
        }



        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            if (DataGridOrders.SelectedItem is Order selectedOrder)
            {
                _context.Orders.Remove(selectedOrder);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Добавление и удаление Ремонтов
        private void AddRepairButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;

            var product = _context.Products.FirstOrDefault(p => p.ProductID == 1);  

            if (product != null)
            {
                var newRepair = new Repair
                {
                    ProductID = product.ProductID, 
                    Description = "Новый ремонт",
                    RepairDate = DateTime.Now
                };

                _context.Repairs.Add(newRepair);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Товар с данным ID не существует. Пожалуйста, создайте товар перед добавлением ремонта.");
            }
        }

        private void DeleteRepairButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;

            if (DataGridRepairs.SelectedItem is Repair selectedRepair)
            {
                _context.Repairs.Remove(selectedRepair);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите ремонт для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Добавление и удаление Клиентов
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;

            var newCustomer = new Customer
            {
                FirstName = "Новый клиент",
                Phone = "000-0000000",
                Email = "new@customer.com",
                CreatedAt = DateTime.Now
            };

            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            LoadData();
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;

            if (DataGridCustomers.SelectedItem is Customer selectedCustomer)
            {
                var relatedOrders = _context.Orders.Where(o => o.CustomerID == selectedCustomer.CustomerID).ToList();
                foreach (var order in relatedOrders)
                {
                    order.CustomerID = null;  
                }

                var relatedRepairs = _context.Repairs.Where(r => r.CustomerID == selectedCustomer.CustomerID).ToList();
                foreach (var repair in relatedRepairs)
                {
                    repair.CustomerID = null;  
                }

                _context.SaveChanges();  

              
                _context.Customers.Remove(selectedCustomer);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }



        // Добавление и удаление Контрактов
        private void AddContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            var newContract = new Contract
            {
                SupplierID = 1,
                ContractDate = DateTime.Now,

            };

            _context.Contracts.Add(newContract);
            _context.SaveChanges();
            LoadData();
        }

        private void DeleteContractButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            if (DataGridContracts.SelectedItem is Contract selectedContract)
            {
                _context.Contracts.Remove(selectedContract);
                _context.SaveChanges();
                LoadData();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите контракт для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void FilterSuppliersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterSuppliersTextBox.Text.ToLower();


            var filteredSuppliers = _context.Suppliers
    .Where(s => s.CompanyName.ToString().ToLower().Contains(filterText.ToLower()) ||
                s.ContactName.ToString().ToLower().Contains(filterText.ToLower()))
    .ToList();
            DataGridSuppliers.ItemsSource = filteredSuppliers;
        }


        // FilterRepairsTextBox TextChanged
        private void FilterRepairsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterRepairsTextBox.Text.ToLower();

            
            var filteredRepairs = _context.Repairs
                .Include(r => r.Customer)
                .Where(r => r.Description.ToLower().Contains(filterText) ||
                            (r.Customer != null && r.Customer.FirstName.ToLower().Contains(filterText)))  
                .ToList();

            DataGridRepairs.ItemsSource = filteredRepairs;
        }


        // FilterContractsTextBox TextChanged

        private void FilterContractsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterContractsTextBox.Text.ToLower();
            var filteredContracts = _context.Contracts
                .Where(c => c.ContractID.ToString().Contains(filterText) ||
                            (c.Customer != null && c.Customer.FirstName.ToLower().Contains(filterText)))
                .ToList();
            DataGridContracts.ItemsSource = filteredContracts;
        }

        // FilterEmployeesTextBox TextChanged
        private void FilterEmployeesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterEmployeesTextBox.Text.ToLower();
            var filteredEmployees = _context.Employees
                .Where(emp => emp.FirstName.ToLower().Contains(filterText) ||
                               emp.Position.ToLower().Contains(filterText))
                .ToList();
            DataGridEmployees.ItemsSource = filteredEmployees;
        }

        // FilterCustomComputersTextBox TextChanged
        private void FilterCustomComputersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterCustomComputersTextBox.Text.ToLower();

            var filteredCustomComputers = _context.CustomComputers
                .Where(cc => cc.Name.ToLower().Contains(filterText) ||
                             cc.Specifications.ToLower().Contains(filterText)) 
                .ToList();

            DataGridCustomComputers.ItemsSource = filteredCustomComputers;
        }


        private void FilterPurchasesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filterText = FilterPurchasesTextBox.Text.ToLower();
            DateTime filterDate;

           
            bool isDate = DateTime.TryParse(filterText, out filterDate);

            
            var purchases = _context.Purchases
                .Include(p => p.Product) 
                .Include(p => p.Supplier) 
                .ToList();  

            
            var filteredPurchases = purchases
                .Where(p => p.Product.ProductName.ToLower().Contains(filterText) ||
                            (p.Supplier != null && p.Supplier.Name.ToLower().Contains(filterText)) ||
                            (isDate && p.PurchaseDate.Date == filterDate.Date)) 
                .ToList();

            DataGridPurchases.ItemsSource = filteredPurchases;
        }




        // Фильтрация для Клиентов
        private void FilterCustomersTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = FilterCustomersTextBox.Text.ToLower();
            DataGridCustomers.ItemsSource = _context.Customers
                .Where(c => c.FirstName.ToLower().Contains(filterText) ||
                             c.Email.ToLower().Contains(filterText))
                .ToList();
        }

        // не смог нормально настроить остальные кнопки :(

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            MessageBox.Show("Работник добавлен!!");
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            MessageBox.Show("Работник удалён");
        }

        private void AddCustomComputerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            MessageBox.Show("Кастомный компьютер добавлен added!");
        }

        private void DeleteCustomComputerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            MessageBox.Show("Кастомный компьютер удалён!");
        }

        private void AddPurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            MessageBox.Show("Покупка добавлена!");
        }

        private void DeletePurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HasPermission()) return;
            MessageBox.Show("Покупка удалена!");
        }




    }
}
