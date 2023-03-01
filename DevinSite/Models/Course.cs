namespace DevinSite.Models;

public class Course
{
    public int CourseID { get; set; }
    public string Instructor { get; set; } = "";
    public string MeetingTimes { get; set; } = "n/a";
    public string Name { get; set; } = "n/a";
    public List<Assignment>? Assignments { get; set; }
    public List<Enrollment>? GetEnrollments { get; set; }
   
    public static explicit operator Course(string? v)
    {
        return new Course()
        {
            Name = v!,
            Assignments = new(),
            GetEnrollments = new()
        };
    }
}

