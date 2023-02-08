﻿using System;

namespace DevinSite.Data
{
    public class SiteRepository : ISiteRepository
    {
        public List<Assignment> Assignments { get; set; }
        public List<Course> Courses { get; set; }
        public List<Student> Students { get; set; }
        private readonly ApplicationDbContext _context;

        public SiteRepository(ApplicationDbContext context)
        {
            _context = context;
            Assignments = _context.Assignments.ToList();
            Courses = _context.Courses.ToList();
            Students = _context.Users.ToList<Student>();
        }

        public void AddAssignment(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
        }

        public void AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void AddStudent(Student student)
        {
            _context.Users.Add(student);
        }

        public void DeleteAssignmnent(Assignment assignment)
        {
            _context.Assignments.Remove(assignment);
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
}

