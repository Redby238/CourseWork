using System.Collections.Generic;

namespace WPF_LoginForm.Model
{
    public class CustomComputer
    {
        public int CustomComputerID { get; set; }
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }
        public ICollection<CustomComputerComponent> Components { get; set; }
        public string Name { get; internal set; }
        public string Specifications { get; internal set; }
    }
}
