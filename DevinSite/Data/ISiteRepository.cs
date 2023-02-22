using System;
namespace DevinSite.Data
{
    public interface ISiteRepository
    {
        List<Assignment> Assignments { get; set; }
        List<Course> Courses { get; set; }
        List<Student> Students { get; set; }
        List<Enrollment> Enrollments { get; set; }

        public Task AddAssignmentAsync(Assignment assignment);
        public void UpdateAssignmnent(Assignment assignment);
        public void DeleteAssignmnent(Assignment assignment);
        /// <summary>
        /// Retreive all the assignments that belong to the signed in user, and delete them from the database.
        /// This does not delete assignments from moodle, Only what is currently stored in our database.
        /// </summary>
        public void DeleteAllStudentAssignments();

        public Task AddAssignmentRangeAsync(List<Assignment> assignments);

        public Task AddCourseAsync(Course course);
        public void UpdateCourse(Course course);
        public void DeleteCourse(Course course); 

        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student); 
    }
}

