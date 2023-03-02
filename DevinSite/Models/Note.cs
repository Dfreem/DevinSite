namespace DevinSite.Models;

public class Note
{
    public int NoteId { get; set; }
    [ForeignKey("GetStudent")]
    public int StudentId { get; set; }
    public Student GetStudent { get; set; } = default!;
    [ForeignKey("GetAssignment")]
    public int AssignmentId { get; set; }
    public Assignment GetAssignment { get; set; } = default!;
    public string Body { get; set; } = "";
}

