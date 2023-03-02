namespace DevinSite.Models;

public class Assignment
{
    public int AssignmentId { get; set; }
    public string Title { get; set; } = "No Title";
    public Course? GetCourse { get; set; }
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string? Details { get; set; }
    public bool IsDone { get; set; } = false;
    public List<Note> Notes { get; } = new();

    public override string ToString()
    {
        return $"title: {Title}\ndue: {DueDate}\ncourse: {GetCourse!.Name}\ninstructions: {Details}\nnotes: {Notes}";
    }
}

