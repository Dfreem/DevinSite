using System.ComponentModel.DataAnnotations.Schema;

namespace DevinSite.Models
{
    public class Student : IdentityUser
    {
        public List<Course> Courses { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
        [NotMapped]
        public IList<string>? RoleNames { get; set; }
    }
}

