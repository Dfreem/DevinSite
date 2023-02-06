using System.ComponentModel.DataAnnotations.Schema;

namespace DevinSite.Models
{
    public class Student : IdentityUser
    {
        public string Name { get; set; } = default!;
        public List<Course> Courses { get; set; } = new();
        public List<Assignment> Assignments { get; set; } = new();
        [NotMapped]
        public IList<string> RoleNames { get; set; } = new List<string>();
        DateTime LastUpdate { get; set; } = DateTime.UnixEpoch;
    }
}

