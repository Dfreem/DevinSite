namespace DevinSite.Data;

public static class SeedData
{
    public static void Init(IServiceProvider services)
    {
        using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            if (context.Courses.Any() || context.Assignments.Any())
            {
                return;
            }

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
        }
    }

}

