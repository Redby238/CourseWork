using System;
using System.Collections.Generic;

namespace WPF_LoginForm.Model
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<Contract> Contracts { get; set; }
        public string ContactName { get; internal set; }
        public string Name { get; internal set; }
    }
}
