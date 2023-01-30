namespace DevinSite.Data;

public static class SeedData
{
    public static void Init(System.IServiceProvider services, ApplicationDbContext context)
    {
        ApplicationDbContext _context = context;
        UserManager<Student> _userManager = services.GetRequiredService<UserManager<Student>>();

        if (_context.Courses.Any() || _context.Assignments.Any())
        {
            return;
        }
        Student joe = new()
        {
            UserName = "joe",
            Email = "fake@user.com",
            EmailConfirmed = true
        };
        _userManager.CreateAsync(joe, "BadPassword123");

        Student devin = new()
        {
            UserName = "dfreem987",
            Email = "freemand@my.lanecc.edu",
            EmailConfirmed = true,
        };
        _userManager.CreateAsync(devin, "!BassCase987");

        Assignment firstAssignment = new()
        {
            Title = "Fake1",
            DueDate = DateTime.Now.AddDays(3),
            Details = "Test test"
        };
        Assignment secondAssignment = new()
        {
            Title = "Fake2",
            DueDate = DateTime.Now.AddDays(3),
            Details = "Test test"
        };
        Assignment thirdAssignment = new()
        {
            Title = "Fake3",
            DueDate = DateTime.Now.AddDays(3),
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

        // assign 1:1 => Assignment.Course : Course
        firstAssignment.Course = course1;
        secondAssignment.Course = course1;
        thirdAssignment.Course = course1;

        // assign 1:many => Student:Course
        devin.Courses ??= new();
        devin.Courses.Add(course1);

        // assign 1:many => Student:Assignment
        devin.Assignments.Add(firstAssignment);
        devin.Assignments.Add(secondAssignment);
        devin.Assignments.Add(thirdAssignment);

        // save to DB
        //_context.Assignments.AddRange(firstAssignment, secondAssignment, thirdAssignment);
        _context.Courses.Add(course1);
        _context.Users.Add(devin);
        _context.Users.Add(joe);
        _context.SaveChanges();
    }
}
