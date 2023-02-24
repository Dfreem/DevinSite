using Microsoft.IdentityModel.Tokens;

namespace DevinSite.Controllers;

//[Authorize(Roles ="Student")]
[Authorize]
public class HomeController : Controller
{

    private readonly IServiceProvider _services;
    private readonly ISiteRepository _repo;
    private readonly IConfiguration _config;
    private readonly SignInManager<Student> _signInManager;
    private readonly UserManager<Student> _userManager;
    private Student CurrentUser { get; }

    public string? _currentUserName;

    public HomeController(IServiceProvider services, IConfiguration configuration)
    {
        // injected dependencies. 
        _services = services;
        _config = configuration;

        // used to retrieve the current user.
        _signInManager = _services.GetRequiredService<SignInManager<Student>>();
        _userManager = _services.GetRequiredService<UserManager<Student>>();
        _repo = services.GetRequiredService<ISiteRepository>();
        _currentUserName = _signInManager.Context.User.Identity!.Name;
        CurrentUser = _userManager.FindByNameAsync(_currentUserName).Result;
    }

    public IActionResult Index(string searchString)
    {
        UserProfileVM userVM = new(CurrentUser);
        //UpdateScheduleAsync().Wait();

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
            CurrentUser.GetAssignments = await MoodleWare.GetCalendarAsync(CurrentUser.MoodleString);
            CurrentUser.LastUpdate = DateTime.Now;
            // update the user on the userManager with the new retrieved schedule.
            await _userManager.UpdateAsync(CurrentUser);
        }
        await Task.CompletedTask;
    }

    public async Task<IActionResult> RefreshFromMoodle()
    {
        CurrentUser.LastUpdate = CurrentUser.LastUpdate.AddDays(-3);
        await _userManager.UpdateAsync(CurrentUser);
        await UpdateScheduleAsync();
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Retreive all the assignments that belong to the signed in user, and delete them from the database.
    /// This does not delete assignments from moodle, Only what is currently stored in our database.
    /// </summary>
    /// <returns><see cref="RedirectResult"/></returns>
    public IActionResult RemoveAllAssignments()
    {
        _repo.DeleteAllStudentAssignments();
        return RedirectToAction("Index");
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
    public async Task<IActionResult> UpdateAssignment(Assignment assignment)
    {
        var oldAssignment = _repo.Assignments.Find(a => a.AssignmentId.Equals(assignment.AssignmentId));
        oldAssignment!.Notes = oldAssignment.Notes + "\n" + assignment.Notes;
        _repo.UpdateAssignmnent(oldAssignment);
        return RedirectToAction("Index");
    }

    //public static List<Assignment>? SearchAssignmentsByDate(DateTime toSearch, List<Assignment> assignments)
    //{
    //assignments.Sort((x, y) => x.DueDate.CompareTo(y.DueDate));
    //    string moodleString = "";
    //    if (DateTime.Today < toSearch)
    //    {
    //        moodleString += MoodleWare.MoodleOptions.NextWeek;
    //    }
    //    else
    //    {
    //        moodleString += MoodleWare.MoodleOptions.ThisWeek;
    //    }
    //    return assignments.FindAll(a => a.DueDate.Day.Equals(toSearch.Day));
    //}

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

