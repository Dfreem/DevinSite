using System;
namespace DevinSite.ViewModels
{
    public class RegisterVM 
    {
        public string Name { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public static explicit operator Student(RegisterVM v)
        {
            return new Student()
            {
                Name = v.Name,
                UserName = v.UserName,
                RoleNames = new List<string>(){ "Student" },
                LockoutEnabled = false,
                EmailConfirmed = true
            };
        }
    }
}

