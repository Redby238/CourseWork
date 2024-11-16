using System;
using WPF_LoginForm.Model;

public class Repair
{
    public int RepairID { get; set; }
    public int ProductID { get; set; }
    public int? CustomerID { get; set; }
    public Product Product { get; set; }
    public DateTime RepairDate { get; set; }
    public string Status { get; set; } 
    public bool IsUnderWarranty { get; set; }
    public string Description { get; internal set; }
    public Customer Customer { get; internal set; }
}
