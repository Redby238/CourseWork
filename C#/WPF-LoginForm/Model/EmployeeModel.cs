using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public string Position { get; set; } 
        public decimal Salary { get; set; } 
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public DateTime HireDate { get; set; } 
        public string Department { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }

}
