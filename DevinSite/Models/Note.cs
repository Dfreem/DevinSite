namespace DevinSite.Models;

public class Note
{
    public int NoteId { get; set; }
    public int StudentId { get; set; }
    public int AssignmentId { get; set; }
    public string Body { get; set; } = "";
}

