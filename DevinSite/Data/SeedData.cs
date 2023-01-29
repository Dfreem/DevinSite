namespace DevinSite.Data;

public static class SeedData
{
    public static void Init(System.IServiceProvider services)
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Courses.Any() || context.Assignments.Any())
        {
            return;
        }

        PasswordHasher<Student> phash = new PasswordHasher<Student>();

        Student joe = new()
        {
            UserName = "joe",
            Email = "fake@user.com",
            
        };
        joe.PasswordHash = phash.HashPassword(joe, "badPassword");

        Assignment firstAssignment = new()
        {
            Title = "Fake1",
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
            Details = "Test test"
        };
        Assignment secondAssignment = new()
        {
            Title = "Fake2",
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
            Details = "Test test"
        };
        Assignment thirdAssignment = new()
        {
            Title = "Fake3",
            DueDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3)),
            Details = "Test test"
        };

        Course course1 = new()
        {
            Title = "Fake Course 1",
            Instructor = "Schneabley",
            Assignments = new()
                {
                    firstAssignment,
                    secondAssignment,
                    thirdAssignment
                },
            MeetingTimes = "Never, Always, ∞"
        };

        firstAssignment.Course = course1;
        secondAssignment.Course = course1;
        thirdAssignment.Course = course1;
        context.Assignments.AddRange(firstAssignment, secondAssignment, thirdAssignment);
        context.Courses.Add(course1);
        context.SaveChanges();
    }
}
