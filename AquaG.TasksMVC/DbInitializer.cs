using AquaG.TasksDbModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AquaG.TasksMVC
{
    class DbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("paid") == null)
                await roleManager.CreateAsync(new IdentityRole("paid"));
        }
    }
}

