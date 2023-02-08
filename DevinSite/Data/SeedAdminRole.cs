using System;
namespace DevinSite.Data
{
    public static class SeedRoles
    {
        public static async Task SeedAdminRole(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<Student>>();
            string username = "admin";
            string password = "Password123";
            string rolename = "Admin";

            if (await roleManager.FindByNameAsync(rolename) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(rolename));
            }
            Student adminUser = new() { UserName = username };
            var result = await userManager.CreateAsync(adminUser, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, rolename);
            }
        }

        public static async Task SeedStudentRole(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<Student>>();

            string username = "dfreem987";
            var getDevin = await userManager.FindByNameAsync(username);
            if (await roleManager.FindByNameAsync("Student") is null)
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

        }
    }
}

