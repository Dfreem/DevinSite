namespace DevinSite.Data;

public static class SeedData
{
    public static void Init(System.IServiceProvider services, IConfiguration configuration)
    {
        string moodleString = configuration["ConnectionStrings:MoodleString"];
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        UserManager<Student> userManager = services.GetRequiredService<UserManager<Student>>();
        if (userManager.Users.Any())
        {
            return;
        }
        SeedUsersAsync(userManager).Wait();
        SeedAssignments(services, moodleString).Wait();
        SeedEnrollments(context, userManager).Wait();
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
    public static async Task SeedAssignments(IServiceProvider services, string moodleString)
    {
        var repo = services.GetRequiredService<ISiteRepository>();
        
        var cal = await MoodleWare.GetCalendarAsync(services, moodleString);
        await repo.AddAssignmentRangeAsync(cal);
    }

    public static async Task SeedEnrollments(ApplicationDbContext context, UserManager<Student> userManager)
    {

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
        }
        await context.SaveChangesAsync();
    }
}