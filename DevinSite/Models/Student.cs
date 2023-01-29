using System.ComponentModel.DataAnnotations.Schema;

namespace DevinSite.Models
{
    public class Student : IdentityUser
    {
        List<Course> Courses { get; set; } = default!;
        List<Assignment> Assignment { get; set; } = default!;
        [NotMapped]
        IList<string> RoleNames { get; set; } = default!;
    }
}

