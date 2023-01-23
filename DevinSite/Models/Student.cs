namespace DevinSite.Models
{
    public class Student : IdentityUser
    {
        int StudentID { get; set; }
        List<Course> Courses { get; set; } = default!;
        List<Assignment> Assignment { get; set; } = default!;
    }
}

