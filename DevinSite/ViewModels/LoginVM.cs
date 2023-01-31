using System;
namespace DevinSite.ViewModels;

public class LoginVM
{
    public IEnumerable<Student>? Students { get; set; }
    public IEnumerable<IdentityRole>? Roles { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public bool RememberMe { get; set; } = false;
    public string? ReturnUrl { get; set; }
}

