using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WPF_LoginForm.DbSettings;

public class ApplicationDbContextSeed
{
    public static async Task SeedRoles(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Owner", "Admin", "Operator", "Guest" };

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var role = new IdentityRole(roleName);
                var result = await roleManager.CreateAsync(role);

                // Проверяем результат создания роли
                if (!result.Succeeded)
                {
                    // Выводим описание ошибки
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"Failed to create role {roleName}: {error}");
                    }
                }
            }
        }
    }
}
