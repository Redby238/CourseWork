using Microsoft.AspNet.Identity;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_LoginForm.DbSettings;



namespace WPF_LoginForm
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationDbContext _context;
        public MainWindow(string userName,string role)
        {
            InitializeComponent();

            txtUserName.Text = "Имя пользователя: " + userName;
            txtRole.Text = "Роль: " + role;

        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadSuppliersAndProducts()
        {
            var suppliers = _context.Suppliers
                .Include(s => s.Products) 
                .ToList();

            foreach (var supplier in suppliers)
            {
               
                Console.WriteLine($"Поставщик: {supplier.CompanyName}");
                foreach (var product in supplier.Products)
                {
                    Console.WriteLine($"Продукция: {product.ProductName}, Категория: {product.Category}");
                }
            }
        }

        private void LoadCustomersWhoPurchasedFromFirm(string firmName)
        {
            var customers = _context.Customers
                .Where(c => c.Purchases.Any(p => p.Product.Supplier.CompanyName == firmName))
                .ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"Клиент: {customer.FirstName} {customer.LastName}");
                foreach (var purchase in customer.Purchases)
                {
                    Console.WriteLine($"Купленная продукция: {purchase.Product.ProductName}, Дата покупки: {purchase.PurchaseDate}");
                }
            }
        }

        private void GetWarrantyRepairs()
        {
            var repairs = _context.Repairs
                .Where(r => r.IsUnderWarranty)
                .ToList();

            foreach (var repair in repairs)
            {
                Console.WriteLine($"Товар: {repair.Product.ProductName}, Дата ремонта: {repair.RepairDate}, Статус: {repair.Status}");
            }
        }

        private void CalculateMaxRevenueForMonth(DateTime date)
        {
            var maxRevenue = _context.Orders
                .Where(o => o.OrderDate.Month == date.Month && o.OrderDate.Year == date.Year)
                .GroupBy(o => o.OrderDate.Month)
                .Max(g => g.Sum(o => o.TotalPrice));

            Console.WriteLine($"Максимальная выручка за месяц: {maxRevenue:C}");
        }

        private void FindAffordableComponents(decimal maxPrice)
        {
            var components = _context.Products
                .Where(p => p.Price <= maxPrice)
                .ToList();

            foreach (var component in components)
            {
                Console.WriteLine($"Продукт: {component.ProductName}, Цена: {component.Price:C}");
            }
        }

        private void GetSuppliersWithFrequentRepairs()
        {
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
                Console.WriteLine($"Поставщик: {supplier.Supplier.CompanyName}, Количество ремонтов: {supplier.RepairCount}");
            }
        }

        private void GetCustomComputerComponents(int customerId)
        {
            var customComputer = _context.CustomComputers
                .Include(cc => cc.Components)
                .FirstOrDefault(cc => cc.CustomerID == customerId);

            if (customComputer != null)
            {
                foreach (var component in customComputer.Components)
                {
                    Console.WriteLine($"Компонент: {component.ProductName}, Количество: {component.Quantity}");
                }
            }
        }

        private void GetReceivedProducts(DateTime startDate, DateTime endDate)
        {
            var receivedProducts = _context.Products
                .Where(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate)
                .ToList();

            foreach (var product in receivedProducts)
            {
                Console.WriteLine($"Продукт: {product.ProductName}, Дата поступления: {product.CreatedAt}");
            }
        }

        private void GetContractsAndCount(DateTime startDate, DateTime endDate)
        {
            var contracts = _context.Contracts
                .Where(c => c.ContractDate >= startDate && c.ContractDate <= endDate)
                .GroupBy(c => c.Supplier)
                .Select(g => new
                {
                    Supplier = g.Key,
                    ContractCount = g.Count()
                })
                .ToList();

            foreach (var contractInfo in contracts)
            {
                Console.WriteLine($"Поставщик: {contractInfo.Supplier.CompanyName}, Количество договоров: {contractInfo.ContractCount}");
            }
        }

        private void GetSupplierByContractNumber(int contractId)
        {
            var contract = _context.Contracts
                .Include(c => c.Supplier)
                .FirstOrDefault(c => c.ContractID == contractId);

            if (contract != null)
            {
                Console.WriteLine($"Поставщик: {contract.Supplier.CompanyName}");
            }
            else
            {
                Console.WriteLine("Поставщик не найден.");
            }
        }

        private void btnGetSuppliersAndProducts_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliersAndProducts();
        }
    }

  
}
