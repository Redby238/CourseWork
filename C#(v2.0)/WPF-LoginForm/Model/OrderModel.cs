using System.Collections.Generic;
using System;
using System.Linq;

namespace WPF_LoginForm.Model
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public DateTime OrderDate { get; set; }
        public List<Purchase> Purchases { get; set; }

        
        public decimal TotalPrice
        {
            get
            {
                return Purchases.Sum(p => p.Product.Price * p.Quantity);  
            }
        }
    }
}
