namespace DevinSite.Models;

public class Course
{
    public int CourseID { get; set; }
    public string Instructor { get; set; } = "";
    public string MeetingTimes { get; set; } = "n/a";
    [Required(AllowEmptyStrings = true)]
    public string Name { get; set; } = "No Name";
    public List<Assignment>? Assignments { get; set; }
   
}

