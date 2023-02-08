namespace DevinSite.Models;

public class Student : IdentityUser
{
    public string Name { get; set; } = default!;
    public List<Course> Courses { get; set; } = new();
    public List<Assignment> GetAssignments { get; set; } = new();
    [NotMapped]
    public IList<string>? RoleNames { get; set; }
    // Do I need this also? TODO either remove or uncomment
    //public List<Enrollment>? GetEnrollments { get; set; }
    DateTime LastUpdate { get; set; } = DateTime.UnixEpoch;
}

