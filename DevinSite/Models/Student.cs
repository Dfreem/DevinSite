namespace DevinSite.Models
{
    public class Student : IdentityUser
    {
        List<Course> Courses { get; set; } = default!;
        List<Assignment> Assignment { get; set; } = default!;
    }
}

