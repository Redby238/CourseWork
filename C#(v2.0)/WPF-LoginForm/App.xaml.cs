using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Windows;
using WPF_LoginForm.DbSettings;
using WPF_LoginForm.Model;

namespace WPF_LoginForm
{ // Здесь находиться метод который заполняет тестовыми данными Базу данных
    public partial class App : Application
    {

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = new ApplicationDbContext();


            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            Seed(context);

            await ApplicationDbContextSeed.SeedRoles(context, roleManager);

        }

        public static void Seed(ApplicationDbContext context)
        {
            // Заполнение таблицы Customers и Orders 
            if (!context.Customers.Any())
            {
                var firstNames = new[] { "John", "Alice", "Bob", "David", "Mary" };
                var lastNames = new[] { "Doe", "Smith", "Green", "White", "Jones" };

                for (int i = 0; i < 10; i++)
                {
                    var customer = new Customer
                    {
                        FirstName = firstNames[new Random().Next(0, firstNames.Length)],
                        LastName = lastNames[new Random().Next(0, lastNames.Length)],
                        Email = $"customer{i}@example.com",
                        Phone = $"+1-555-{new Random().Next(1000, 9999)}",
                        Address = $"{new Random().Next(100, 999)} Elm St",
                        CreatedAt = DateTime.Now
                    };
                    context.Customers.Add(customer);
                }
                context.SaveChanges();
            }

            // Заполнение таблицы Repairs
            if (!context.Repairs.Any())
            {
                var productIds = context.Products.Select(p => p.ProductID).ToList();

                for (int i = 0; i < 10; i++)
                {
                    var repair = new Repair
                    {
                        ProductID = productIds[new Random().Next(0, productIds.Count)],
                        CustomerID = context.Customers.First().CustomerID, 
                        RepairDate = DateTime.Now.AddDays(-new Random().Next(1, 100)),
                        Status = "Completed",
                        IsUnderWarranty = new Random().Next(0, 2) == 1
                    };
                    context.Repairs.Add(repair);
                }
                context.SaveChanges();
            }

            // Заполнение таблицы Employees
            if (!context.Employees.Any())
            {
                var positions = new[] { "Manager", "Technician", "Sales", "HR", "Engineer" };

                for (int i = 0; i < 5; i++)
                {
                    var employee = new Employee
                    {
                        FirstName = $"Employee{i}",
                        LastName = $"Lastname{i}",
                        Position = positions[new Random().Next(0, positions.Length)],
                        Salary = new Random().Next(40000, 100000),
                        Email = $"employee{i}@company.com",
                        Phone = $"+1-555-{new Random().Next(1000, 9999)}",
                        HireDate = DateTime.Now.AddYears(-new Random().Next(1, 5)),
                        Department = "Department" + i,
                        CreatedAt = DateTime.Now
                    };
                    context.Employees.Add(employee);
                }
                context.SaveChanges();
            }

            // Заполнение таблицы CustomComputers
            if (!context.CustomComputers.Any())
            {
                var customerIds = context.Customers.Select(c => c.CustomerID).ToList();

                for (int i = 0; i < 5; i++)
                {
                    var customComputer = new CustomComputer
                    {
                        CustomerID = customerIds[new Random().Next(0, customerIds.Count)]
                    };
                    context.CustomComputers.Add(customComputer);
                }
                context.SaveChanges();
            }

            // Заполнение таблицы CustomComponents
            if (!context.CustomComputerComponent.Any())
            {
                var customComputerIds = context.CustomComputers.Select(cc => cc.CustomComputerID).ToList();
                var productIds = context.Products.Select(p => p.ProductID).ToList();

                for (int i = 0; i < 15; i++)
                {
                    var customComponent = new CustomComputerComponent()
                    {
                        CustomComputerID = customComputerIds[new Random().Next(0, customComputerIds.Count)],
                        CustomComputerComponentID = productIds[new Random().Next(0, productIds.Count)],
                        Quantity = new Random().Next(1, 5)
                    };
                    context.CustomComputerComponent.Add(customComponent);
                }
                context.SaveChanges();
            }

            // Заполнение таблицы Contracts
            if (!context.Contracts.Any())
            {
                var supplierIds = context.Suppliers.Select(s => s.SupplierID).ToList();

                for (int i = 0; i < 5; i++)
                {
                    var contract = new Contract
                    {
                        SupplierID = supplierIds[new Random().Next(0, supplierIds.Count)],
                        ContractDate = DateTime.Now.AddMonths(-new Random().Next(1, 12)),
                        TotalAmount = new Random().Next(5000, 20000)
                    };
                    context.Contracts.Add(contract);
                }
                context.SaveChanges();
            }

         
            if (!context.Purchases.Any())
            {
                var customerIds = context.Customers.Select(c => c.CustomerID).ToList();
                var productIds = context.Products.Select(p => p.ProductID).ToList();
                var orderIds = context.Orders.Select(o => o.OrderID).ToList(); 
                var random = new Random();

                for (int i = 0; i < 10; i++)
                {
                    var purchase = new Purchase
                    {
                        CustomerID = customerIds[random.Next(customerIds.Count)],
                        ProductID = productIds[random.Next(productIds.Count)],
                        OrderID = orderIds[random.Next(orderIds.Count)], 
                        Quantity = random.Next(1, 10),
                        Price = Math.Round((decimal)(random.Next(100, 5000) * random.NextDouble()), 2), 
                        PurchaseDate = DateTime.Now.AddDays(-random.Next(1, 365))
                    };
                    context.Purchases.Add(purchase);
                }
                context.SaveChanges();
            }


        }


    }


}


 
