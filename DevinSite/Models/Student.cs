using System.ComponentModel.DataAnnotations.Schema;

namespace DevinSite.Models
{
    public class Student : IdentityUser
    {
        public string Name { get; set; } = default!;
        //public List<Course> Courses { get; set; } = new();
        [NotMapped]
        public IList<string>? RoleNames { get; set; }
        public List<Enrollment>? GetEnrollments { get; set; }
        DateTime LastUpdate { get; set; } = DateTime.UnixEpoch;
    }
}

