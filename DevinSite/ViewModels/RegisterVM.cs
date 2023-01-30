using System;
namespace DevinSite.ViewModels
{
    public class AdminVM
    {
        public IEnumerable<Student> Students { get; set; } = default!;
        public IEnumerable<IdentityRole> Roles { get; set; } = default!;
    }
}

