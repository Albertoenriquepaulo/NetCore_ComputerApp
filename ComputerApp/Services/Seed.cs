using ComputerApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputerApp.Services
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] rolesName = {
                "admin",
                "client"
            };

            //CREANDO ROLES admin y client, at STARTUP
            foreach (string rolName in rolesName)
            {
                if (!await roleManager.RoleExistsAsync(rolName))
                {
                    await roleManager.CreateAsync(new IdentityRole(rolName));
                }
            }
            //FIN CREANDO ROLES admin y client, at STARTUP

            //CREANDO USUARIO admin Y AÑADIENDO ROL admin, at STARTUP
            AppUser adminUser = new AppUser
            {
                UserName = "root",
                Email = "root@admin.com",
                Name = "admin",
                BirthDate = new DateTime(1979, 01, 29)
            };

            var result = await userManager.CreateAsync(adminUser, "a");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "admin");
            }
            //FIN CREANDO USUARIO admin Y AÑADIENDO ROL admin, at STARTUP
        }


    }
}
