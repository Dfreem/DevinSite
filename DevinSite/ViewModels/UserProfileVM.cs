using System;
using NuGet.Packaging;
using NuGet.Packaging.Signing;

namespace DevinSite.ViewModels
{
    public class UserProfileVM
    {
        // TODO Fill in properties to use on user profile page.
        public List<Assignment> GetAssignments { get => GetStudent.GetAssignments; set => GetStudent.GetAssignments.AddRange(value); }
        public Student GetStudent { get; set; } = default!;
        public List<Course> GetCourses { get => GetStudent.Courses; set => GetStudent.Courses.AddRange(value); }
        public string Name { get => GetStudent.Name; set => GetStudent.Name = value; }
        public string UserName { get => GetStudent.UserName; set => GetStudent.UserName = value; }
        public string Email { get => GetStudent.Email; set => GetStudent.Email= value; }
        public DateTime LastUpdate { get => GetStudent.LastUpdate; set => GetStudent.LastUpdate = value; }
        public string Id { get; set; } = default!;

        public UserProfileVM(Student s)
        {
            GetStudent = s;
        }

        public static explicit operator Student(UserProfileVM up)
        {
            return up.GetStudent;
        }

    }
}

