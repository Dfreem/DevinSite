using System;
namespace DevinSite.Repositories;

public interface ISiteRepository
{
    List<Assignment> Assignments { get; set; }
    List<Course> Courses { get; set; }
    List<Enrollment> Enrollments { get; set; }
    List<Note> Notes { get; set; }

    public Task AddAssignmentAsync(Assignment assignment);
    public Task AddCourseAsync(Course course);
    public Task AddAssignmentRangeAsync(List<Assignment> assignments);
    public Task AddNotesAsync(Note newNotes);
    public void UpdateAssignment(Assignment assignment);
    public void UpdateCourse(Course course);
    public void UpdateNotes(Note toAdd);
    /// <summary>
    /// Retreive all the assignments that belong to the signed in user, and delete them from the database.
    /// This does not delete assignments from moodle, Only what is currently stored in our database.
    /// </summary>
    public void DeleteAllStudentAssignments();
    public void DeleteAssignment(Assignment assignment);
    public void DeleteCourse(Course course);
    public void DeleteNotes(Note note);
 }

