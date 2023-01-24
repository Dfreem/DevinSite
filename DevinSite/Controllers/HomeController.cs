﻿

namespace DevinSite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //DDBContext _repo { get; set; }
    private readonly ISiteRepository _repo;
   
    public HomeController(ILogger<HomeController> logger, ISiteRepository repo)
    {
        // injected dependencies. 
        _logger = logger;
        _repo = repo;  
    }

    // if navigated to by a search, deteremine if search string is date.
    [Authorize()]
    public IActionResult Index(string searchString)
    {
        // retrieve all assignments in db.
        var assignments = from m in _repo.Assignments.Include(a => a.Course)
                          select m;
        DateOnly searchDate;

        // try parse search string into a Date
        bool didParse = DateOnly.TryParse(searchString, out searchDate);
        if (didParse)
        {
            // if date -> search by due date
            assignments = assignments.Where(a => a.DueDate.Equals(searchDate));
        }
        else if (searchString is not null)
        {
            // if not date -> search by title
            assignments = assignments.Where(a => a.Title.Contains(searchString));
        }
        return View(assignments.ToList());
    }

    public IActionResult AddNewAssignment()
    {
        Assignment assignment = new();
        return View(assignment);
    }

    [HttpPost]
    public IActionResult AddNewAssignment(Assignment assignment)
    {
        // add assignment to Assignments table.
        _repo.AddAssignment(assignment);
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

