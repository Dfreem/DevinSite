namespace DevinSite.Data;

public static class SeedData
{
    /// <summary>
    /// Init is used to seed a database without authorization in place.
    /// For use with Identity Role based authorizatioin, use <see cref="SeedAdminUserAsync(System.IServiceProvider)"/> and <see cref="Seed(ModelBuilder)"/>
    /// </summary>
    /// <param name="services"></param>
    //public static void Init(System.IServiceProvider services)
    //{
    //    ApplicationDbContext _context = services.GetRequiredService<ApplicationDbContext>();
    //    UserManager<Student> _userManager = services.GetRequiredService<UserManager<Student>>();

    //    if (_context.Courses.Any() || _context.Assignments.Any())
    //    {
    //        return;
    //    }
    //    Student joe = new()
    //    {
    //        UserName = "joe",
    //        Email = "fake@user.com",
    //        EmailConfirmed = true
    //    };
    //    _userManager.CreateAsync(joe, "BadPassword123");

    //    Student devin = new()
    //    {
    //        UserName = "dfreem987",
    //        Email = "freemand@my.lanecc.edu",
    //        EmailConfirmed = true,
    //    };
    //    _userManager.CreateAsync(devin, "!BassCase987");

    //    Assignment firstAssignment = new()
    //    {
    //        Title = "Fake1",
    //        DueDate = DateTime.Now.AddDays(3),
    //        Details = "Test test"
    //    };
    //    Assignment secondAssignment = new()
    //    {
    //        Title = "Fake2",
    //        DueDate = DateTime.Now.AddDays(3),
    //        Details = "Test test"
    //    };
    //    Assignment thirdAssignment = new()
    //    {
    //        Title = "Fake3",
    //        DueDate = DateTime.Now.AddDays(3),
    //        Details = "Test test"
    //    };

    //    Course course1 = new()
    //    {
    //        Title = "Fake Course 1",
    //        Instructor = "Schneabley",
    //        Assignments = new()
    //            {
    //                firstAssignment,
    //                secondAssignment,
    //                thirdAssignment
    //            },
    //        CourseID = 1,
    //        MeetingTimes = "Never, Always, ∞"
    //    };

    //    // assign 1:1 => Assignment.Course : Course
    //    firstAssignment.Course = course1;
    //    secondAssignment.Course = course1;
    //    thirdAssignment.Course = course1;

    //    // assign 1:many => Student:Course
    //    devin.Courses ??= new();
    //    devin.Courses.Add(course1);

    //    // assign 1:many => Student:Assignment
    //    devin.Assignments.Add(firstAssignment);
    //    devin.Assignments.Add(secondAssignment);
    //    devin.Assignments.Add(thirdAssignment);

    //    // save to DB
    //    //_context.Assignments.AddRange(firstAssignment, secondAssignment, thirdAssignment);
    //    _context.Courses.Add(course1);
    //    _context.Users.Add(devin);
    //    _context.Users.Add(joe);
    //    _context.SaveChanges();
    //}

    const string USER_NAME = "dfreem987";
    const string PASSWORD = "!BassCase987";
    const string ROLE_NAME = "Admin";

    /// <summary>
    /// Async method used in conjunction with Identity Role based authorization.
    /// </summary>
    /// <param name="services">a <see cref="IServiceProvider"/> that handles inject of dependencies.</param>
    public static async Task SeedAdminUserAsync(IServiceProvider services)
    {
        RoleManager<IdentityRole> roleManager = services
            .GetRequiredService<RoleManager<IdentityRole>>();

        UserManager<Student> studentManager = services
            .GetRequiredService<UserManager<Student>>();

        // check for admin role existance
        if (await roleManager.FindByNameAsync(ROLE_NAME) is null)
        {
            await roleManager.CreateAsync(new IdentityRole(ROLE_NAME));
        }
        if (await studentManager.FindByNameAsync(USER_NAME) is null)
        {
            // I'm the admin.
            Student Devin = new()
            {
                UserName = USER_NAME,
                Name = "Devin Freeman",
                Email = "freemand@my.lanecc.edu",
                EmailConfirmed = true,
                RoleNames = new List<string>(){ "Admin", "Student" },
            };

            //  administrator user creation with password
            var status = await studentManager.CreateAsync(Devin, PASSWORD);

            // add admin user to admin role.
            if (status.Succeeded)
            { await studentManager.AddToRoleAsync(Devin, ROLE_NAME); }
        }
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        // create 4 Courses
        Course CS296 = new()
        {
            Title = "ASP.NET Web Development",
            Instructor = "Brian Bird",
            MeetingTimes = "T, TH : 10AM",
            Assignments = new(),
            GetEnrollments = new(),
            CourseID = 1
        };
        Course CS276 = new()
        {
            Title = "Database Systems and Modeling",
            Instructor = " Lindy Stewart",
            Assignments = new(),
            CourseID = 2
        };
        Course CS246 = new()
        {
            Title = "Systems Design",
            Instructor = "Brian Bird",
            MeetingTimes = "T, TH : 2PM",
            Assignments = new(),
            CourseID = 3
        };
        // create 3 more students
        Student Ben = new()
        {
            Name = "Ben Wilson",
            Email = "wilsonb@my.lancc.edu",
            EmailConfirmed = true,
            RoleNames = new List<string> {"Student"}
        };

        Student Totoro = new()
        {
            Name = "Totoro and Co.",
            Email = "ttronco@my.lancc.edu",
            EmailConfirmed = true,
            RoleNames = new List<string> {"Student"}
        };

        Student Lauren = new()
        {
            Name = "Lauren Lastnameson",
            Email = "lastnamesonL@my.lancc.edu",
            EmailConfirmed = true,
            RoleNames = new List<string> {"Student"}
        };

        Enrollment totoroCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment benCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment LaurnCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment enrollmentCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment enrollmentCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment enrollmentCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment enrollmentCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment enrollmentCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        Enrollment enrollmentCS246 = new()
        {
            GetCourse = CS246,
            CourseId = CS246.CourseID,
            GetStudent = Totoro,
            StudentId = Totoro.Id
        };
        List<Assignment> cal = MoodleWare.GetCalendar();
        int nextId = 0;
        foreach (var assignment in cal)
        {
            nextId++;
            assignment.AssignmentId = nextId;
        }

        modelBuilder.Entity<Assignment>()
            .HasData(cal);
    }
}
