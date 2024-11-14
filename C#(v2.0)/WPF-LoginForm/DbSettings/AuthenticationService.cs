using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using WPF_LoginForm.DbSettings;
using WPF_LoginForm.Model;
using System.Linq;

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

        // Метод для регистрации нового пользователя с ролью
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

       
        public async Task<string> GetUserRole(string userName)
        {
            var user = await userManager.FindByNameAsync(userName); 
            if (user != null)
            {
               
                var roles = await userManager.GetRolesAsync(user.Id);
                return roles.FirstOrDefault(); 
            }
            return null;
        }

        
        public async Task<string[]> GetRolesForUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName); 
            if (user != null)
            {
                // Получаем все роли пользователя по его ID
                var roles = await userManager.GetRolesAsync(user.Id); // Передаем user.Id, а не объект user
                return roles.ToArray(); // Возвращаем все роли пользователя
            }
            return new string[] { }; // Если пользователя нет, возвращаем пустой массив
        }
    }
}
