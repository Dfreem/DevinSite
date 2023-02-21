using System;
namespace DevinSite.ViewModels
{
    public class UserProfileVM
    {
        // TODO Fill in properties to use on user profile page.
        public List<Assignment> GetAssignments { get; set; } = default!;
        public Student GetStudent { get; set; } = default!;
        public List<Course> GetCourses { get; set; } = default!;
    }
}

