namespace DevinSite.Models;

public class Enrollment
{
    public string StudentId { get; set; } = "";
    public int CourseId { get; set; }
    public Student GetStudent { get; set; } = default!;
    public Course GetCourse { get; set; } = default!;
}