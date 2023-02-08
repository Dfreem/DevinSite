

namespace DevinSite.Models;

public class Assignment
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = default!;
    public Course? GetCourse { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string? Details { get; set; }
    public bool IsDone { get; set; } = false;
}

