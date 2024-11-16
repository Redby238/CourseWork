using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.DbSettings
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Server=DESKTOP-24KDLCK;Database=CourseWorkDB;Trusted_Connection=True;")
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Repair> Repairs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CustomComputer> CustomComputers { get; set; }
        public DbSet<CustomComputerComponent> CustomComputerComponent { get; set; }
        public DbSet<Contract> Contracts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка связи между продуктами и поставщиками
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между покупками и клиентами
            modelBuilder.Entity<Purchase>()
                .HasRequired(p => p.Customer)
                .WithMany(c => c.Purchases)
                .HasForeignKey(p => p.CustomerID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между покупками и продуктами
            modelBuilder.Entity<Purchase>()
                .HasRequired(p => p.Product)
                .WithMany()
                .HasForeignKey(p => p.ProductID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между заказами и клиентами
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Customer)
                .WithMany()
                .HasForeignKey(o => o.CustomerID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между заказами и покупками
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Purchases)
                .WithRequired(p => p.Order)
                .HasForeignKey(p => p.OrderID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между CustomComputer и Customer
            modelBuilder.Entity<CustomComputer>()
                .HasRequired(cc => cc.Customer)
                .WithMany()
                .HasForeignKey(cc => cc.CustomerID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между CustomComputer и компонентами
            modelBuilder.Entity<CustomComputer>()
                .HasMany(cc => cc.Components)
                .WithRequired(ccc => ccc.CustomComputer)
                .HasForeignKey(ccc => ccc.CustomComputerID)
                .WillCascadeOnDelete(false); 

            // Настройка связи между Contract и Supplier
            modelBuilder.Entity<Contract>()
                .HasRequired(c => c.Supplier)
                .WithMany(s => s.Contracts)
                .HasForeignKey(c => c.SupplierID)
                .WillCascadeOnDelete(false); 
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
