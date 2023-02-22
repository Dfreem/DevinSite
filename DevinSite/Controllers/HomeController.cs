
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
    public string? _currentUser;

    public HomeController(ILogger<HomeController> logger, System.IServiceProvider services, IConfiguration configuration)
    {
        // injected dependencies. 
        _services = services;
        _config = configuration;

        // used to retrieve the current user.
        _signInManager = _services.GetRequiredService<SignInManager<Student>>();
        _userManager = _services.GetRequiredService<UserManager<Student>>(); _logger = logger;
        _repo = services.GetRequiredService<ISiteRepository>();
        _currentUser = _signInManager.Context.User.Identity!.Name;
        Console.WriteLine(MoodleWare.MoodleOptions.NextWeek);
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
    /// <param name="newMoodle">The new moodle calendar connection string.</param>
    //public void SetMoodleString(string newMoodle)
    //{
    //    int baseEndIndex = newMoodle.IndexOf('?');
    //    int optionsIndex = newMoodle.IndexOf('')
    //}
    public IActionResult Index(string searchString)
    {
        // retrieve all assignments in db.
        var currentUser = _signInManager.Context.User.Identity!.Name;
        var assignments = from m in _repo.Assignments
                          select m;
        DateTime searchDate;
        if (currentUser is not null) UpdateScheduleAsync().Wait();

        // if navigated to by a search, deteremine if search string is date.
        bool didParse = DateTime.TryParse(searchString, out searchDate);
        if (didParse)
        {
            // if date -> search by due date
            assignments = MoodleWare.SearchAssignmentsByDate(searchDate, assignments.ToList<Assignment>());
        }
        else if (searchString is not null)
        {
            // if not date -> search by title
            assignments = assignments.Where(a => a.Title.Contains(searchString));
        }
        return View(assignments!.ToList());
    }



    /// <summary>
    /// Update Schedule will check when the currently logged in user was last updated.
    /// If it was more than 3 days ago, the users assignments are deleted from the DB and
    /// replaced with A freshly retreived schedule. 
    /// </summary>
    /// <returns>A Task that completes when the DB is finished updating the current user.</returns>
    public async Task UpdateScheduleAsync()
    {
        // look for the currently signed in user name using signInManager
        // retrieved currently signed in user.
        var currentUser = await _userManager.FindByNameAsync(_currentUser);

        // check how long ago the users last update was.
        if (DateTime.Today.Subtract(currentUser.LastUpdate).Days > 3)
        {
            // Full size version of pre-configured URL
            var options = Util.MoodleWare.MoodleOptions.ThisWeek;
            var moodleOptions = MoodleWare.Options[options];
            string moodleString = MoodleWare.AssembleMoodleString(moodleOptions, _config);

            // delete assignments in the DB and replace with newly retreived. 
            _repo.DeleteAllStudentAssignments();
            var cal = await MoodleWare.GetCalendarAsync(_services, moodleString);
            await _repo.AddAssignmentRangeAsync(cal);

            // reset last update to todays date.
            currentUser.LastUpdate = DateTime.Now;
            await _userManager.UpdateAsync(currentUser);
        }
    }

    public async Task<IActionResult> RefreshFromDB()
    {
        var current = await _userManager.FindByNameAsync(_currentUser);
        current.LastUpdate = new DateTime(DateTime.Now.Day - 4);
        _ = await _userManager.UpdateAsync(current);
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

    public IActionResult EditAssignment(int id)
    {
        return View(_repo.Assignments.First(a => a.AssignmentId.Equals(id)));
    }



    [HttpPost]
    public IActionResult EditAssignment(Assignment assignment)
    {
        // if this assignment is in  the DB, get that version.
        _repo.UpdateAssignmnent(assignment);
        return RedirectToAction("Index", "Home");
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

