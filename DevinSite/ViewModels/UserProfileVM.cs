using System;
using NuGet.Packaging;
using NuGet.Packaging.Signing;

namespace DevinSite.ViewModels;

public class UserProfileVM
{
    public Student GetStudent { get; set; } = default!;
    public List<Course> GetCourses { get; set; } = default!;
    public List<Assignment> GetAssignments { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
    public string ConfirmPassword { get; set; } = default!;
    public string NewMoodle { get; set; } = default!;
    public string Id { get; set ; } = default!;
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public DateTime LastUpdate { get; set; } = default!;
    public UserProfileVM()
    {

    }

    public UserProfileVM(Student student)
    {
        GetStudent = student;
        GetCourses = student.Courses;
        GetAssignments = student.GetAssignments;
        Id = student.Id;
        Name = student.Name;
        Email = student.Email;
        UserName = student.UserName;
        LastUpdate = student.LastUpdate;
        
    }

    public static explicit operator Student(UserProfileVM upv)
    {
        return upv.GetStudent;
    }

}

