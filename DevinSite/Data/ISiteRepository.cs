using System;
namespace DevinSite.Data
{
    public interface ISiteRepository
    {
        List<Assignment> Assignments { get; set; }
        List<Course> Courses { get; set; }
        List<Student> Students { get; set; }

        public void AddAssignment(Assignment assignment);
        public void UpdateAssignmnent(Assignment assignment);
        public void DeleteAssignmnent(Assignment assignment);
        public void DeleteAssignmentRange(List<Assignment> assignments);

        public Task AddAssignmentRangeAsync(List<Assignment> assignments);

        public void AddCourse(Course course);
        public void UpdateCourse(Course course);
        public void DeleteCourse(Course course); 

        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student); 
    }
}

