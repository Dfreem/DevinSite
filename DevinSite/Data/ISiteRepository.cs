using System;
namespace DevinSite.Data
{
    public interface ISiteRepository
    {
        IQueryable<Assignment> Assignments { get; set; }
        IQueryable<Course> Courses { get; set; }
        IQueryable<Student> Students { get; set; }

        public void AddAssignment(Assignment assignment);
        public void UpdateAssignmnent(Assignment assignment);
        public void DeleteAssignmnent(Assignment assignment); 

        public void AddCourse(Course course);
        public void UpdateCourse(Course course);
        public void DeleteCourse(Course course); 

        public void AddStudent(Student student);
        public void UpdateStudent(Student student);
        public void DeleteStudent(Student student); 
    }
}

