using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using WPF_LoginForm.DbSettings;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.Services
{
    public class AuthenticationService
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public AuthenticationService()
        {
            var context = new ApplicationDbContext();
            var userStore = new UserStore<ApplicationUser>(context);
            var roleStore = new RoleStore<IdentityRole>(context);

            userManager = new UserManager<ApplicationUser>(userStore);
            roleManager = new RoleManager<IdentityRole>(roleStore);
        }

        public async Task<IdentityResult> RegisterUser(string userName, string password, string email, string role = "User")
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FirstName = "New User", 
                LastName = "New User"    
            };

            
            var result = await userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRoleAsync(user.Id, role); 
            }

            return result;
        }
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                return user;  
            }
            return null;  
        }
    }
}
