using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; } 
        public string Phone { get; set; } 
        public string Address { get; set; } 
        public string Email { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }

}
