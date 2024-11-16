using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_LoginForm.DbSettings;
using WPF_LoginForm.Model;
using WPF_LoginForm.View;

namespace WPF_LoginForm
{
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;
        private readonly string _currentUserName;
        private readonly string _currentUserRole;


        public MainWindow(ApplicationDbContext context, string userName, string role)
        {
            InitializeComponent();
            _context = context ?? throw new ArgumentNullException(nameof(context));

            txtUserName.Text = "Имя пользователя: " + userName;
            txtRole.Text = "Роль: " + role;
            Loaded += MainWindow_Loaded;
            _currentUserName = userName;
            _currentUserRole = role;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (HasAdminAccess())
            {
                btnGoToAdminPanel.Visibility = Visibility.Visible;
            }
            else
            {
                btnGoToAdminPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void btnGoToAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            var adminPanelWindow = new AdminPanelWindow();
            adminPanelWindow.Show();
            this.Close();
        }
        private void btnGoToViewTables_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUserRole == "Admin" || _currentUserRole == "Owner") 
            {
               
                View.ViewTablesWindow tablesWindow = new View.ViewTablesWindow(_context, _currentUserName);
                tablesWindow.Show();
            }
            else
            {
                
                MessageBox.Show("У вас нет прав для доступа к этому окну.", "Ошибка доступа", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        public bool HasAdminAccess()
        {
            using (var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context)))
            {
                var currentUser = userManager.FindByName(_currentUserName);

                if (currentUser == null)
                {
                    MessageBox.Show("Пользователь не найден.");
                    return false;
                }

                var roles = userManager.GetRoles(currentUser.Id);
               
                return roles.Contains("Admin") || roles.Contains("Owner");
            }
        }
        public bool HasOperatorAccess()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var currentUser = userManager.FindByName(_currentUserName); 

            if (currentUser == null)
            {
                MessageBox.Show("Користувач не знайдений.");
                return false;
            }

            var roles = userManager.GetRoles(currentUser.Id);
            return roles.Contains("Operator");
        }

        public bool HasGuestAccess()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var currentUser = userManager.FindByName(_currentUserName); 

            if (currentUser == null)
            {
                MessageBox.Show("Користувач не знайдений.");
                return false;
            }

            var roles = userManager.GetRoles(currentUser.Id);
            return roles.Contains("Guest");
        }



        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        

        private void DisplayDataInListBox(string data)
        {
            listBox.Items.Add(data);
        }

        private void LoadSuppliersAndProducts()
        {
            listBox.Items.Clear();
            var suppliers = _context.Suppliers
                .Include(s => s.Products)
                .ToList();

            foreach (var supplier in suppliers)
            {
                DisplayDataInListBox($"Поставщик: {supplier.CompanyName}");
                foreach (var product in supplier.Products)
                {
                    DisplayDataInListBox($"Продукция: {product.ProductName}, Категория: {product.Category}");
                }
            }
        }

        private void LoadCustomersWhoPurchasedFromFirm(string firmName)
        {
            listBox.Items.Clear();  

            var customers = _context.Customers
                .Where(c => c.Purchases
                            .Any(p => p.Product.Supplier.CompanyName == firmName))  
                .ToList();

            if (customers.Count == 0)
            {
                DisplayDataInListBox("Нет клиентов, купивших продукцию от этой фирмы.");
                return;
            }

            foreach (var customer in customers)
            {
                DisplayDataInListBox($"Клиент: {customer.FirstName} {customer.LastName}");
                foreach (var purchase in customer.Purchases)
                {
                    DisplayDataInListBox($"Купленная продукция: {purchase.Product.ProductName}, Дата покупки: {purchase.PurchaseDate}");
                }
            }
        }


        private void GetWarrantyRepairs()
        {
            listBox.Items.Clear();
            var repairs = _context.Repairs
      .Include(r => r.Product) 
      .Where(r => r.IsUnderWarranty)
      .ToList();

            foreach (var repair in repairs)
            {
                if (repair.Product != null)
                {
                    DisplayDataInListBox($"Товар: {repair.Product.ProductName}, Дата ремонта: {repair.RepairDate}, Статус: {repair.Status}");
                }
                else
                {
                    DisplayDataInListBox($"Товар: Неизвестно, Дата ремонта: {repair.RepairDate}, Статус: {repair.Status}");
                }
            }
        }

        private void CalculateMaxRevenueForMonth(DateTime date)
        {
            listBox.Items.Clear();

            var maxRevenue = _context.Orders
                .Where(o => o.OrderDate.Month == date.Month && o.OrderDate.Year == date.Year)
                .ToList() 
                .Max(o => o.TotalPrice); 

            DisplayDataInListBox($"Максимальная выручка за месяц: {maxRevenue:C}");
        }

        private void FindAffordableComponents(decimal maxPrice)
        {
            listBox.Items.Clear();
            var components = _context.Products
                .Where(p => p.Price <= maxPrice)
                .ToList();

            if (components.Count == 0)
            {
                MessageBox.Show("Нет доступных компонентов по заданной цене.");
                return;
            }

            foreach (var component in components)
            {
                DisplayDataInListBox($"Продукт: {component.ProductName}, Цена: {component.Price:C}");
            }
        }


        private void GetSuppliersWithFrequentRepairs()
        {
            listBox.Items.Clear();
            var suppliers = _context.Repairs
                .Where(r => r.IsUnderWarranty)
                .GroupBy(r => r.Product.Supplier)
                .Select(g => new
                {
                    Supplier = g.Key,
                    RepairCount = g.Count()
                })
                .OrderByDescending(s => s.RepairCount)
                .ToList();

            foreach (var supplier in suppliers)
            {
                DisplayDataInListBox($"Поставщик: {supplier.Supplier.CompanyName}, Количество ремонтов: {supplier.RepairCount}");
            }
        }

        private void GetCustomComputerComponents(int customerId)
        {
            listBox.Items.Clear();
            var customComputer = _context.CustomComputers
                .Include(cc => cc.Components)
                .FirstOrDefault(cc => cc.CustomerID == customerId);

            if (customComputer != null)
            {
                foreach (var component in customComputer.Components)
                {
                    DisplayDataInListBox($"Компонент: {component.ProductName}, Количество: {component.Quantity}");
                }
            }
            else
            {
                DisplayDataInListBox("Индивидуальные компоненты не найдены.");
            }
        }

        private void GetReceivedProducts(DateTime startDate, DateTime endDate)
        {
            listBox.Items.Clear();
            var receivedProducts = _context.Products
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToList();

            foreach (var product in receivedProducts)
            {
                DisplayDataInListBox($"Продукт: {product.ProductName}, Дата поступления: {product.CreatedAt}");
            }
        }

        private void GetContractsAndCount(DateTime startDate, DateTime endDate)
        {
            listBox.Items.Clear(); 

          
            var contracts = _context.Contracts
                .Where(c => c.ContractDate >= startDate && c.ContractDate <= endDate)  
                .GroupBy(c => c.Supplier)  
                .Select(g => new
                {
                    Supplier = g.Key,  
                    ContractCount = g.Count()  
                })
                .ToList();  

            
            if (contracts.Count == 0)
            {
                DisplayDataInListBox("Нет договоров за указанный период.");
                return;
            }

         
            foreach (var contractInfo in contracts)
            {
                DisplayDataInListBox($"Поставщик: {contractInfo.Supplier.CompanyName}, Количество договоров: {contractInfo.ContractCount}");
            }
        }


        private void GetSupplierByContractNumber(int contractId)
        {
            listBox.Items.Clear();
            var contract = _context.Contracts
                .Include(c => c.Supplier)
                .FirstOrDefault(c => c.ContractID == contractId);

            if (contract != null)
            {
                DisplayDataInListBox($"Поставщик: {contract.Supplier.CompanyName}");
            }
            else
            {
                DisplayDataInListBox("Поставщик не найден.");
            }
        }

        private void btnExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            if (queryComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string queryName = selectedItem.Content.ToString();
                switch (queryName)
                {
                    case "Загрузить поставщиков и продукцию":
                        LoadSuppliersAndProducts();
                        break;
                    case "Клиенты, покупающие продукцию":
                        LoadCustomersWhoPurchasedFromFirm("Hator");
                        break;
                    case "Ремонты по гарантии":
                        GetWarrantyRepairs();
                        break;
                    case "Максимальная выручка за месяц":
                        CalculateMaxRevenueForMonth(DateTime.Now);
                        break;
                    case "Доступные компоненты по цене":
                        FindAffordableComponents(600);
                        break;
                    case "Поставщики с частыми ремонтами":
                        GetSuppliersWithFrequentRepairs();
                        break;
                    case "Индивидуальные компоненты":
                        GetCustomComputerComponents(1);
                        break;
                    case "Продукция за период":
                        GetReceivedProducts(DateTime.Now.AddMonths(-1), DateTime.Now);
                        break;
                    case "Договоры по поставщикам":
                        GetContractsAndCount(DateTime.Now.AddMonths(-1), DateTime.Now);
                        break;
                    default:
                        MessageBox.Show("Выберите запрос для выполнения.");
                        break;
                }
            }
        }
    }
}
