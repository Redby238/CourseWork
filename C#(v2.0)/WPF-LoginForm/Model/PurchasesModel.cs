using System;

namespace WPF_LoginForm.Model
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }

        public int Quantity { get; set; }  

        public decimal Price { get; set; } 

        public DateTime PurchaseDate = DateTime.Now;
    }
}
