

namespace DevinSite.Controllers;

//[Authorize(Roles ="Student")]
[Authorize]
public class HomeController : Controller
{
    #region Fields & Properties
    private readonly IServiceProvider _services;
    private readonly ISiteRepository _repo;
    private readonly IConfiguration _config;
    private readonly SignInManager<Student> _signInManager;
    private readonly UserManager<Student> _userManager;
    private string? _currentUserName;


    Student CurrentUser { get; }
    Assignment? SelectedAssignment { get; set; }

    #endregion

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

        CurrentUser = _userManager.FindByNameAsync(_currentUserName!).Result!;

        // makes sure the users schedule is up to date.
        RefreshFromMoodle().Wait();

    }
    #region Views & Partial Views
    public IActionResult Index(string searchString)
    {
        // encapsulate user in ViewModel
        UserProfileVM userVM = new(CurrentUser);
        //userVM = InitNewNote(userVM);
        DateTime searchDate;

        // try parse search string into DateTime, if DateTime, use to search due date.
        bool didParse = DateTime.TryParse(searchString, out searchDate);

        if (didParse)
        {
            userVM.GetAssignments = userVM.GetAssignments.FindAll(a => a.DueDate.Day.Equals(searchDate.Day));
        }
        if (SelectedAssignment is not null)
        {
            userVM.DisplayedAssignment = SelectedAssignment;
        }
        // if search string is not DateTime, use it to search the assignments for the search string.
        return View(userVM);
    }

    public IActionResult SearchByCourse(int courseId)
    {
        UserProfileVM uvm = new(CurrentUser)
        {
            GetAssignments = CurrentUser.GetAssignments.FindAll(a => a.GetCourse!.CourseID.Equals(courseId))
        };
        return View("Index", uvm);
    }

    public async Task<IActionResult> RefreshFromMoodle()
    {
        CurrentUser.LastUpdate = CurrentUser.LastUpdate.AddDays(-3);
        CurrentUser.GetCourses.Clear();
        await _userManager.UpdateAsync(CurrentUser);
        await UpdateScheduleAsync();
        return RedirectToAction("Index");
    }
    #endregion
    #region Non-view Controller Methods
    /// <summary>
    /// Update Schedule will check when the currently logged in user was last updated.
    /// If it was more than 3 days ago, the users assignments are deleted from the DB and
    /// replaced with A freshly retreived schedule. 
    /// </summary>
    /// <returns>A Task that completes when the DB is finished updating the current user.</returns>
    public async Task UpdateScheduleAsync()
    {
        // check users LastUpdate property to see if the last update is more than 3 days ago.
        if (CurrentUser.LastUpdate.AddDays(-3) >= DateTime.Now)
        {
            var cal = await MoodleWare.GetCalendarAsync(CurrentUser.MoodleString);
            var courses = MoodleWare.ParseCourses(cal);
            List<string> courseNames = new();

            // compose a list of the names of the courses
            foreach (Course course in courses)
            {
                if (!courseNames.Contains(course.Name))
                {
                    courseNames.Add(course.Name);
                }
            }
            var fromRepo = _repo.Courses.FindAll(c => !courseNames.Contains(c.Name));
        }

    }
    // Although these are all methods involving assignment,
    // they all redirect to the same view, Index of the Home controller.
    // this is the reason they are included in this controller.

    // =========================== Assignment Methods ===========================

    /// <summary>
    /// Retreive all the assignments that belong to the signed in user, and delete them from the database.
    /// This does not delete assignments from moodle, Only what is currently stored in our database.
    /// </summary>
    /// <returns><see cref="RedirectResult"/></returns>
    public IActionResult RemoveAllAssignments()
    {
        _repo.DeleteAllStudentAssignments();
        CurrentUser.GetAssignments.Clear();
        _userManager.UpdateAsync(CurrentUser);
        return RedirectToAction("Index");
    }

    /// <summary>
    /// Select assignment sets the <see cref="UserProfileVM.DisplayedAssignment"/> property.
    /// This property is the assignment that is displayed in the details section when an assignment is clicked.
    /// </summary>
    /// <param name="id">the id of the assignment that was clicked</param>
    /// <returns>a full refresh of the Index view.</returns>
    public IActionResult SelectAssignment(int id)
    {
        SelectedAssignment = CurrentUser.GetAssignments.Find(a => a.AssignmentId.Equals(id))!;
        UserProfileVM userProfile = new(CurrentUser) { DisplayedAssignment = SelectedAssignment };
        return View("Index", userProfile);
    }

    public IActionResult RemoveAssignment(int id)
    {
        // look for assignment in db with AssignmentId == id
        Assignment? toDelete = _repo.Assignments.First(assignment => assignment.AssignmentId.Equals(id));
        if (toDelete is not null)
        {
            _repo.DeleteAssignment(toDelete);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult UpdateAssignment(Assignment assignment)
    {
        var oldAssignment = _repo.Assignments.Find(a => a.AssignmentId.Equals(assignment.AssignmentId));
        oldAssignment!.Notes.Body += HtmlString.NewLine + assignment.Notes.Body;
        _repo.UpdateAssignment(oldAssignment);
        return RedirectToAction("Index");
    }

    #endregion
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