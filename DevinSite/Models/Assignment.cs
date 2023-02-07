

namespace DevinSite.Models;

public class Assignment
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = "";
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string? Details { get; set; }
    public bool IsDone { get; set; } = false;
    public bool IsEditting { get; set; } = false;
    public Course? GetCourse { get; set; }
    public int CourseId { get; set; }

    public override string ToString()
    {
        return $"Title: {Title} \nDueDate: {DueDate}\nDetails:\n{Details}";
    }
}

