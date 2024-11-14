using System;

namespace WPF_LoginForm.Model
{
    public class Contract
    {
        public int ContractID { get; set; } 
        public int SupplierID { get; set; }  
        public Supplier Supplier { get; set; } 
        public DateTime ContractDate { get; set; }  
        public decimal TotalAmount { get; set; }  
    }
}
