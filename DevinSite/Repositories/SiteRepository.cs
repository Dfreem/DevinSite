using System;

namespace DevinSite.Repositories;

public class SiteRepository : ISiteRepository
{
    public List<Assignment> Assignments { get; set; }
    public List<Course> Courses { get; set; }
    public List<Student> Students { get; set; }
    public List<Enrollment> Enrollments { get; set; }
    private readonly ApplicationDbContext _context;

    public SiteRepository(ApplicationDbContext context)
    {
        _context = context;
        Assignments = _context.Assignments.Include(a => a.GetCourse).ToList();
        Courses = _context.Courses.ToList();
        Students = _context.Users.ToList<Student>();
        Enrollments = _context.Enrollments.Include(e => e.GetCourse).Include(e => e.GetStudent).ToList<Enrollment>();
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

    public void DeleteAssignmnent(Assignment assignment)
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

    public void UpdateAssignmnent(Assignment assignment)
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

   
}

