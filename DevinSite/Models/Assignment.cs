

namespace DevinSite.Models;

public class Assignment
{
    public int AssignmentId { get; set; }
    public Course Course { get; set; } = default!;
    public string Title { get; set; } = default!;
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string? Details { get; set; }
    public bool IsDone { get; set; } = false;
    public bool IsEditting { get; set; } = false;

    public override string ToString()
    {
        return $"Title: {Title}\nCourse: {Course!.Title} \nDueDate: {DueDate}\nDetails:\n{Details}";
    }
}

