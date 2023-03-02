using System;

namespace DevinSite.Repositories;

public class SiteRepository : ISiteRepository
{
    public List<Assignment> Assignments { get; set; }
    public List<Course> Courses { get; set; }
    public List<Student> Students { get; set; }
    public List<Enrollment> Enrollments { get; set; }
    public List<Note> Notes { get; set; }

    private readonly ApplicationDbContext _context;

    public SiteRepository(ApplicationDbContext context)
    {
        _context = context;
        Courses = _context.Courses.ToList();

        // get all the assignments related to the current students courses
        // and that have not been marked as complete.
        Assignments = _context.Assignments
            .Include(a => a.GetCourse)
            .Where(a => Courses.Contains(a.GetCourse!) && !a.IsDone)
            .ToList();
        Students = _context.Users.ToList<Student>();
        Enrollments = _context.Enrollments
            .Include(e => e.GetCourse)
            .Include(e => e.GetStudent)
            .ToList<Enrollment>();
        Notes = _context.Notes
            .Include(n => n.GetAssignment)
            .Include(n => n.GetStudent).ToList();
    }

    public async Task AddAssignmentAsync(Assignment assignment)
    {
        await _context.Assignments.AddAsync(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task AddAssignmentRangeAsync(List<Assignment> assignments)
    {
        await _context.Assignments.AddRangeAsync(assignments);
        await _context.SaveChangesAsync();
    }

    public async Task AddCourseAsync(Course course)
    {
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
    }

    public void DeleteAssignment(Assignment assignment)
    {
        _context.Assignments.Remove(assignment);
        _context.SaveChanges();
    }
    
    public void DeleteAllStudentAssignments()
    {
        _context.Assignments.RemoveRange(Assignments);
        _context.SaveChanges();
    }

    public void DeleteCourse(Course course)
    {
        _context.Courses.Remove(course);
        _context.SaveChanges();
    }

    public void DeleteStudent(Student student)
    {
        _context.Users.Remove(student);
        _context.SaveChanges();
    }

    public void UpdateAssignment(Assignment assignment)
    {
        _context.Assignments.Update(assignment);
        _context.SaveChanges();
    }
    public void UpdateCourse(Course course)
    {
        _context.Courses.Update(course);
        _context.SaveChanges();
    }

    public void UpdateStudent(Student student)
    {
        _context.Users.Update(student);
        _context.SaveChanges();
    }

    public async Task AddNotesAsync(Note newNotes)
    {
        await _context.Notes.AddAsync(newNotes);
        await _context.SaveChangesAsync();
    }

    public void UpdateNotes(Note toAdd)
    {
        _context.Notes.Update(toAdd);
        _context.SaveChanges();
    }

    public void DeleteNotes(Note note)
    {
        _context.Notes.Remove(note);
        var deleteAssignments = _context.Assignments.Where(a => a.Notes.NoteId.Equals(note.NoteId)).ToList();
        foreach (var item in deleteAssignments)
        {
            item.Notes = new();
        }
        _context.SaveChanges();
    }
}

