using Microsoft.AspNet.Identity.EntityFramework;

public class ApplicationUser : IdentityUser
{
  
    public string FirstName { get; set; }
    public string LastName { get; set; }

 
}


public class Admin : ApplicationUser
{
 
}

public class Main : ApplicationUser
{
   
}

public class Operator : ApplicationUser
{
    
}
