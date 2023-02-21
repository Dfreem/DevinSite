
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client.Extensions.Msal;
using NodaTime.Extensions;

namespace DevinSite.Controllers;

//[Authorize(Roles ="Student")]
[Authorize]
public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly IServiceProvider _services;
    private readonly ISiteRepository _repo;
    private readonly IConfiguration _config;
    private readonly SignInManager<Student> _signInManager;
    private readonly UserManager<Student> _userManager;
    private readonly Student CurrentUser;
    public string MoodleString { get; set; }

    public string? _currentUser;

    public HomeController(ILogger<HomeController> logger, IServiceProvider services, IConfiguration configuration)
    {
        // injected dependencies. 
        _logger = logger;
        _services = services;
        _config = configuration;

        // set calendar options for use when downloading calendar.
        var dateOption = MoodleWare.MoodleOptions.ThisWeek;
        var options = MoodleWare.Options[dateOption];
        MoodleString = MoodleWare.AssembleMoodleString(options, _config);

        // used to retrieve the current user.
        _signInManager = _services.GetRequiredService<SignInManager<Student>>();
        _userManager = _services.GetRequiredService<UserManager<Student>>();
        _repo = services.GetRequiredService<ISiteRepository>();
        _currentUser = _signInManager.Context.User.Identity!.Name;
        CurrentUser = _userManager.FindByNameAsync(_currentUser).Result;
    }

    /// <summary>
    /// The instructions for retrieving a Moodle Calendar String are as follows:<br />
    /// 1. Login to your Moodle student account<br />
    /// 2. Navigate to the Calendar page.<br />
    /// 3. Make sure to be on the Day or Month view, You should see a button the says "Export Calendar"<br />
    /// 4. select eiter this week or next week as the time frame, and all events.<br />
    /// 5. push "Get Calendar URL"<br />
    /// 6. A URL is generated and diswplayed at the bottom of the screen, copy that and paste as the parameter to this method.<br />
    /// </summary>
    /// <param name = "newMoodle" > The new moodle calendar connection string.</param>
    //public void SetMoodleString(string newMoodle)
    //{
    //    int baseEndIndex = newMoodle.IndexOf('?');
    //    int optionsIndex = newMoodle.IndexOf('')
    //}

    public IActionResult Index(string searchString)
    {
        UpdateScheduleAsync().Wait();
        UserProfileVM userVM = new()
        {
            GetStudent = CurrentUser,
            GetCourses = CurrentUser.Courses,
            GetAssignments = CurrentUser.GetAssignments
        };
        // if navigated to by a search, deteremine if search string is date.
        bool didParse = DateTime.TryParse(searchString, out DateTime searchDate);
        if (didParse)
        {
            // if date -> search by due date
            userVM.GetAssignments = CurrentUser.GetAssignments
                .FindAll(a => a.DueDate.Equals(searchDate));
        }
        else if (searchString is not null)
        {
            // if not date -> search by title
            userVM.GetAssignments = CurrentUser.GetAssignments
                .FindAll(a => a.Title.Contains(searchString) ||
                a.Details!.Contains(searchString) ||
                a.GetCourse!.Equals(searchString));
        }
        return View(userVM);
    }

    /// <summary>
    /// Update Schedule will check when the currently logged in user was last updated.
    /// If it was more than 3 days ago, the users assignments are deleted from the DB and
    /// replaced with A freshly retreived schedule. 
    /// </summary>
    /// <returns>A Task that completes when the DB is finished updating the current user.</returns>
    public async Task UpdateScheduleAsync()
    {
        // check users LastUpdate property to see if the last update is more than 3 days ago.
        if (CurrentUser.LastUpdate.AddDays(3).Day < DateTime.Now.Day)
        {
            // use the MoodleWare class to retrieve and sort the calendar.
            CurrentUser.GetAssignments = await MoodleWare.GetCalendarAsync(MoodleString);
            // update the user on the userManager with the new retrieved schedule.
            await _userManager.UpdateAsync(CurrentUser);
        }
        await Task.CompletedTask;
    }

    public async Task<IActionResult> RefreshFromMoodle()
    {
        CurrentUser.LastUpdate.AddDays(-3);
        await UpdateScheduleAsync();
        UserProfileVM userVM = new()
        {
            GetStudent = CurrentUser,
            GetAssignments = CurrentUser.GetAssignments,
            GetCourses = CurrentUser.Courses
        };
        return RedirectToAction("Index", userVM);
    }

    /// <summary>
    /// Retreive all the assignments that belong to the signed in user, and delete them from the database.
    /// This does not delete assignments from moodle, Only what is currently stored in our database.
    /// </summary>
    /// <returns><see cref="RedirectResult"/></returns>
    public IActionResult RemoveAllAssignments()
    {
        _repo.DeleteAllStudentAssignments();
        return RedirectToAction("Index", new List<Assignment>());
    }

    public IActionResult RemoveAssignment(int id)
    {
        // look for assignment in db with AssignmentId == id
        Assignment? toDelete = _repo.Assignments.First(assignment => assignment.AssignmentId.Equals(id));
        if (toDelete is not null)
        {
            _repo.DeleteAssignmnent(toDelete);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public ContentResult UpdateAssignment(Assignment assignment)
    {
        _repo.UpdateAssignmnent(assignment);
        return Content(assignment.Notes);
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

