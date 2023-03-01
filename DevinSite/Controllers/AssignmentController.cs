using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevinSite.Controllers;

[Authorize]
public class AssignmentController : Controller
{
    private readonly ISiteRepository _repo;
    private Student CurrentUser { get; }
    private readonly UserManager<Student> _userManager;

    public AssignmentController(ISiteRepository repo, SignInManager<Student> signInManager)
    {
        _repo = repo;
        _userManager = signInManager.UserManager;
        string signedInUser = signInManager.Context.User.Identity!.Name!;
        CurrentUser = _userManager.FindByNameAsync(signedInUser).Result!;
    }

    [HttpPost]
    public IActionResult UpdateNotes(UserProfileVM uvm)
    {
        var assignment = uvm.DisplayedAssignment!;
        var oldAssignment = _repo.Assignments.Find(a => a.AssignmentId.Equals(assignment.AssignmentId));
        oldAssignment!.Notes = assignment.Notes;
        uvm.DisplayedAssignment = oldAssignment;
        _repo.UpdateAssignment(oldAssignment);        
        _userManager.UpdateAsync(CurrentUser).Wait();
        return RedirectToAction("SelectAssignment", "Home", new { id = oldAssignment.AssignmentId });
    }
}
