namespace DevinSite.Models;

public class Note
{
    public int NoteId { get; set; }
    public int StudentId { get; set; }
    public Student GetStudent { get; set; } = default!;
    public int AssignmentId { get; set; }
    public Assignment GetAssignment { get; set; } = default!;
    public string Body { get; set; } = "";
}

