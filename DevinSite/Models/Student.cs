namespace DevinSite.Models;

public class Student : IdentityUser
{
    [Required]
    [MaxLength(30, ErrorMessage = "Name should be less than 30 characters long including spaces and punctuation")]
    public string Name { get; set; } = default!;
    public List<Course>? GetCourses { get; set; } = new();
    public List<Assignment>? GetAssignments { get; set; } = new();
    [NotMapped]
    public IList<string>? RoleNames { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    [Url]
    [Required(AllowEmptyStrings = true)]
    public string MoodleString { get; set; } = "";
    public bool MoodleIsSet { get; set; } = false;
}

