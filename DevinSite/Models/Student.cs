namespace DevinSite.Models;

public class Student : IdentityUser
{
    public string Name { get; set; } = default!;
    public List<Course> Courses { get; set; } = new();
    public List<Assignment> GetAssignments { get; set; } = new();
    [NotMapped]
    public IList<string>? RoleNames { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.UnixEpoch;
    public string MoodleString { get; set; } = "";
    public bool MoodleIsSet { get; set; } = false;
}

