
namespace DevinSite.Models;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public Student GetStudent { get; set; } = default!;
    public Course GetCourse { get; set; } = default!;
}

