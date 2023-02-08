﻿using System;
namespace DevinSite.Models;

public class Course
{
    public int CourseID { get; set; }
    public string Instructor { get; set; } = default!;
    public string MeetingTimes { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<Assignment>? Assignments { get; set; }
    public List<Enrollment>? GetEnrollments { get; set; }
}

