namespace DevinSite.Data;

public static class SeedData
{
    public static async Task Init(System.IServiceProvider services)
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        UserManager<Student> userManager = services.GetRequiredService<UserManager<Student>>();
        if (userManager.Users.Any())
        {
            return;
        }
        await SeedUsersAsync(userManager);
    }
    public static async Task SeedUsersAsync(UserManager<Student> userManager)
    {
        Student joe = new()
        {
            UserName = "joeUser",
            Email = "fake@user.com",
            EmailConfirmed = true,
            Id = "j1",
            Name = "Joe"
        };
        await userManager.CreateAsync(joe, "BadPassword123");

        Student devin = new()
        {
            UserName = "dfreem987",
            Email = "freemand@my.lanecc.edu",
            EmailConfirmed = true,
            Name = "Devin Freeman",
            Id = "d1"
        };
        await userManager.CreateAsync(devin, "!BassCase987");

        Student yuri = new()
        {
            UserName = "Yurisaurus123",
            Email = "slowikowskiY@my.lanecc.edu",
            EmailConfirmed = true,
            Name = "Yuri Slowikowski",
            Id = "y1"
        };
        await userManager.CreateAsync(yuri);

    }
    public static async Task SeedAssignments(ApplicationDbContext context)
    {

        Assignment firstAssignment = new()
        {
            Title = "Fake1",
            DueDate = DateTime.Now.AddDays(3),
            Details = "Test test",
        };
        await context.Assignments.AddAsync(firstAssignment);
        Assignment secondAssignment = new()
        {
            Title = "Fake2",
            DueDate = DateTime.Now.AddDays(3),
            Details = "Test test"
        };
        await context.Assignments.AddAsync(secondAssignment);
        Assignment thirdAssignment = new()
        {
            Title = "Fake3",
            DueDate = DateTime.Now.AddDays(3),
            Details = "Test test"
        };
        await context.Assignments.AddAsync(thirdAssignment);
        Course CS246 = new()
        {
            Name = "Systems Design",
            Instructor = "Brian Bird",
            Assignments = new() { firstAssignment },
            MeetingTimes = "2PM : Tue, Thu",
            CourseID = 296
        };
        context.Courses.Add(CS246);
        Course CS276 = new()
        {
            Name = "Database Systems Design",
            Instructor = "Lindey Stewart",
            Assignments = new()
            { secondAssignment },
            MeetingTimes = "Never, Always, ∞",
            CourseID = 276
        };
        context.Add(CS276);
        Course CS296 = new()
        {
            Name = "ASP.NET Web Development",
            Instructor = "Brian Bird",
            Assignments = new()
            { thirdAssignment },
            MeetingTimes = "10AM : Tue, Thu",
            CourseID = 296
        };
        context.Courses.Add(CS296);
        context.SaveChanges();
    }

    public static async Task SeedEnrollments(ApplicationDbContext context, IServiceProvider services)
    {
        UserManager<Student> userManager = services.GetRequiredService<UserManager<Student>>();
        var joe = await userManager.FindByNameAsync("joeUser");
        var devin = await userManager.FindByNameAsync("dfreem987");
        var yuri = await userManager.FindByNameAsync("Yurisaurus123");

        foreach (var course in context.Courses)
        {
            await context.Enrollments.AddRangeAsync(
                new Enrollment()
                {
                    GetCourse = course,
                    CourseId = course.CourseID,
                    GetStudent = joe,
                    StudentId = "j1"
                },
                new Enrollment()
                {
                    GetCourse = course,
                    CourseId = course.CourseID,
                    GetStudent = devin,
                    StudentId = "d1"
                },
                new Enrollment()
                {
                    GetCourse = course,
                    CourseId = course.CourseID,
                    GetStudent = yuri,
                    StudentId = "y1"
                }
            );
            await context.SaveChangesAsync();
        }
    }
}