namespace DevinSite.Data;

public static class SeedRoles
{
    const string USER_NAME = "dfreem987";
    const string PASSWORD = "!BassCase987";
    const string ROLE_NAME = "Admin";

    /// <summary>
    /// Async method used in conjunction with Identity Role based authorization.
    /// </summary>
    /// <param name="services">a <see cref="IServiceProvider"/> that handles inject of dependencies.</param>
    public static async Task SeedAdminUserAsync(IServiceProvider services)
    {
        RoleManager<IdentityRole> roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();

        UserManager<Student> studentManager = services
            .GetRequiredService<UserManager<Student>>();

        // check for admin role existance
        if (await roleManager.FindByNameAsync(ROLE_NAME) is null)
        {
            var roleStatus = await roleManager.CreateAsync(new IdentityRole(ROLE_NAME));
            if (roleStatus.Succeeded) Console.WriteLine("Created Admin Role\nStatus: " + roleStatus.ToString());
        }
        if (await studentManager.FindByNameAsync(USER_NAME) is null)
        {
            // I'm the admin.
            Student Devin = new()
            {
                UserName = USER_NAME,
                Name = "Devin Freeman",
                Email = "freemand@my.lanecc.edu",
                EmailConfirmed = true,
                RoleNames = new List<string>() { "Admin", "Student" },
            };

            //  administrator user creation with password
            var status = await studentManager.CreateAsync(Devin, PASSWORD);

            // add admin user to admin role.
            if (status.Succeeded)
            { await studentManager.AddToRoleAsync(Devin, ROLE_NAME); }
        }
    }

}

