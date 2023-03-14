namespace DevinSite.Models;

public class Assignment
{
    public int AssignmentId { get; set; }
    [Required]
    public string Title { get; set; } = "No Title";
    [Required]
    public Course GetCourse { get; set; } = default!;
    public DateTime DueDate { get; set; } = DateTime.Now;
    public string? Details { get; set; }
    public bool IsDone { get; set; } = false;
    public Note Notes { get; set; } = new();

    public override string ToString()
    {
        return $"title: {Title}\ndue: {DueDate}\ncourse: {GetCourse!.Name}\ninstructions: {Details}\nnotes: {Notes}";
    }
}

