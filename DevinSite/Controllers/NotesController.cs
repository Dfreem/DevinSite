using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DevinSite.Controllers;

[Authorize]
public class NotesController : Controller
{
    private readonly ISiteRepository _repo;
    private Student CurrentUser { get; }
    private readonly UserManager<Student> _userManager;

    public NotesController(ISiteRepository repo, SignInManager<Student> signInManager)
    {
        _repo = repo;

        // userManager is a property on signInManager so only one needs injecting.
        _userManager = signInManager.UserManager;

        // Get the currently signed in user name. Get user from manager using user name
        string signedInUser = signInManager.Context.User.Identity!.Name!;
        CurrentUser = _userManager.FindByNameAsync(signedInUser).Result!;
    }
    /// <summary>
    /// Update the content of a note object owned by a <see cref="UserProfileVM"/>
    /// </summary>
    /// <param name="uvm">the user profile view encapsulating the Student Model.</param>
    /// <returns>Redirects to SelectAssignment with a parametert of the is of the assisgnment that was just changed.</returns>
    [HttpPost]
    public IActionResult UpdateNotes(UserProfileVM uvm)
    {
        //get the encapsulated refernce to the assignment to change.
        var assignment = uvm.DisplayedAssignment!;

        // use the id of the assignment that was retrieved to get the entity reference from the database.
        var oldAssignment = _repo.Assignments.Find(a => a.AssignmentId.Equals(assignment.AssignmentId));

        // concat new note onto the body of the existing note of the assignment we are changing.
        oldAssignment!.Notes.Body += HtmlHelper.AnonymousObjectToHtmlAttributes("<li>" + assignment.Notes.Body + "</li>");

        // replace displayed with updated assignment and updatein database.
        uvm.DisplayedAssignment = oldAssignment;
        _repo.UpdateAssignment(oldAssignment);
        _userManager.UpdateAsync(CurrentUser).Wait();
        return RedirectToAction("SelectAssignment", "Home", new { id = oldAssignment.AssignmentId });
    }
    /// <summary>
    /// Delete a note completely from an <see cref="Assignment"/>
    /// </summary>
    /// <param name="id">the NoteId of the note to be deleted.</param>
    /// <returns>redirects to  SelectAssignment view on the home controller.</returns>
    public IActionResult DeleteNotes(int id)
    {
        var assignment = _repo.Assignments.Find(a => a.AssignmentId.Equals(id))!;
        if (assignment is not null)
        {
            _repo.DeleteNotes(assignment.Notes);
        }
        return RedirectToAction("SelectAssignment", "Home", new { id = assignment!.AssignmentId });

    }
}
