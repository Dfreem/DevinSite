namespace DevinSite.Data;

public static class SeedData
{
    public static void Init(System.IServiceProvider services, IConfiguration configuration)
    {
        var moodleOptions = Util.MoodleWare.MoodleOptions.ThisWeek;
        string options = MoodleWare.Options[moodleOptions];
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        UserManager<Student> userManager = services.GetRequiredService<UserManager<Student>>();
        if (userManager.Users.Any())
        {
            return;
        }
        SeedUsersAsync(userManager).Wait();
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
            MoodleString = "https://classes.lanecc.edu/calendar/export_execute.php?userid=110123&authtoken=9688ee8ecad434630fe9e7b8120a93c9a138b350&preset_what=all&preset_time=weeknow",
            MoodleIsSet = true,
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
        await userManager.CreateAsync(yuri, "Yuri123");

    }
    public static async Task SeedAssignments(IServiceProvider services, string moodleString)
    {
        var repo = services.GetRequiredService<ISiteRepository>();

        var cal = await MoodleWare.GetCalendarAsync("/icalexport.ics");
        await repo.AddAssignmentRangeAsync(cal);
    }

    public static async void SeedCourses(UserManager<Student> manager)
    {
        Student currentUser = await manager.FindByNameAsync("dfreem987");
        foreach (var assignment in currentUser.GetAssignments)
        {
            currentUser.Courses.Add(assignment.GetCourse!);
        }
        await manager.UpdateAsync(currentUser);
    }

    public static void SeedEnrollments(IServiceProvider services)
    {
        var repo = services.GetRequiredService<ISiteRepository>();
        var userManager = services.GetRequiredService<UserManager<Student>>();
        foreach (Student student in userManager.Users)
        {
            foreach (var course in student.Courses)
            {
                // coalescing opperator checks GetEnrollments for null, if null, creates new.
                (course.GetEnrollments ??= new()).Add(new()
                {
                    CourseId = course.CourseID,
                    GetCourse = course,
                    GetStudent = student
                });
            }
        }
    }
}