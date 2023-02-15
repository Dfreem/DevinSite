﻿using System;

namespace DevinSite.Repositories
{
    public class MockRepository : ISiteRepository
    {
        public MockRepository()
        {
            string moodleString = "https://classes.lanecc.edu/calendar/export_execute.php?userid=110123&authtoken=9688ee8ecad434630fe9e7b8120a93c9a138b350&preset_what=all&preset_time=weeknow";
            //string moodleString = MoodleWare.AssembleMoodleString()
            Assignments = MoodleWare.GetCalendarAsync(moodleString).Result;
            
        }
        public List<Assignment> Assignments { get; set; }
        public List<Course> Courses { get; set; } = new ();
        public List<Student> Students { get; set; } = new();
        public List<Enrollment> Enrollments { get; set; } = new();

        public async Task AddAssignmentAsync(Assignment assignment)
        {
            Assignments.Add(assignment);
            await Task.CompletedTask;
        }

        public async Task AddAssignmentRangeAsync(List<Assignment> assignments)
        {
            Assignments.AddRange(assignments);
            await Task.CompletedTask;
        }

        public async Task AddCourseAsync(Course course)
        {
            Courses.Add(course);
            await Task.CompletedTask;
        }

        public void DeleteAllStudentAssignments()
        {
            Assignments = new();
        }

        public void DeleteAssignmnent(Assignment assignment)
        {
            Assignments.Remove(assignment);
        }

        public void DeleteCourse(Course course)
        {
            Courses.Remove(course);
        }

        public void DeleteStudent(Student student)
        {
            Students.Remove(student);
        }

        public void UpdateAssignmnent(Assignment assignment)
        {
            int index = Assignments.FindIndex(a => a.AssignmentId.Equals(assignment.AssignmentId));
            Assignments[index] = assignment;
        }

        public void UpdateCourse(Course course)
        {
            int index = Courses.FindIndex(c => c.CourseID.Equals(course.CourseID));
            Courses[index] = course;
        }

        public void UpdateStudent(Student student)
        {
            int index = Students.FindIndex(s => s.Id.Equals(student.Id));
            Students[index] = student;
        }
    }
}

