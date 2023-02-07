using System;

namespace DevinSite.ViewModels
{
    public class StudentVM
    {
        public List<Student> Students { get; internal set; } = default!;
        public IQueryable<IdentityRole>? Roles { get; internal set; } = default!;
    }
}

