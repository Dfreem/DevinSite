namespace DevinSite.Data;

public static class SeedData
{
    public static void Init(System.IServiceProvider services, IConfiguration configuration)
    {
        var moodleOptions = Util.MoodleWare.MoodleOptions.ThisWeek;
        string options = MoodleWare.Options[moodleOptions];
        string moodleString = MoodleWare.AssembleMoodleString(options, configuration);
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        UserManager<Student> userManager = services.GetRequiredService<UserManager<Student>>();
        if (userManager.Users.Any())
        {
            return;
        }
        SeedUsersAsync(userManager).Wait();
        SeedAssignments(services, moodleString).Wait();
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
        
        var cal = await MoodleWare.GetCalendarAsync(moodleString);
        await repo.AddAssignmentRangeAsync(cal);
    }

    public static void SeedCourses()
    {

    }

    public static async Task SeedEnrollments(IServiceProvider services)
    {
        var repo = services.GetRequiredService<ISiteRepository>();
        var userManager = services.GetRequiredService<UserManager<Student>>();
        foreach (Student student in userManager.Users)
        {
            foreach (var course in student.Courses)
            {

            }
        }
    }
}