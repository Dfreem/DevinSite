using System;
namespace DevinSite.Models;

public class Course
{
    public int CourseID { get; set; }
    public string? Instructor { get; set; }
    public string? MeetingTimes { get; set; } = default!;
    public string? Title { get; set; } = default!;
    public List<Assignment> Assignments { get; set; } = new();
    public List<Enrollment> GetEnrollments { get; set; } = new();
}

