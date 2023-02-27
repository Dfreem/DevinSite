
namespace DevinSite.ViewModels;

public class AssignmentVM
{
    public string Title { get; set; } = default!;
    public Course GetCourse { get; set; } = default!;
    public string? Details { get; set; }
    public string Instructor { get; set; } = default!;
    public DateTime DueDate { get; set; } = default!;
    public Assignment GetAssignment { get; set; } = default!;
    public bool IsDone { get; set; } = false;

    public AssignmentVM()
    {
        GetAssignment = new()
        {
            Title = "default"
        };
    }

    public AssignmentVM(Assignment assignment)
    {
        if (assignment is not null)
        {
            Title = assignment.Title;
            GetCourse = assignment.GetCourse!;
            Details = assignment.Details!;
            DueDate = assignment.DueDate;
            Instructor = GetCourse.Instructor;
        }

    }

    public static implicit operator Assignment(AssignmentVM vm)
    {
        return new Assignment()
        {
            Title = vm.Title,
            Details = vm.Details,
            DueDate = vm.DueDate,
            GetCourse = vm.GetCourse,
            IsDone = vm.IsDone
        };
    }

    public override string ToString()
    {
        return $"{Title}\n{GetCourse.Name}\n{Details}\n{DueDate}";
    }
}

