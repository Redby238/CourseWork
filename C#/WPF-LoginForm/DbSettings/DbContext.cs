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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Supplier)
                .WithMany()
                .HasForeignKey(p => p.SupplierID);

           
        }

        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
