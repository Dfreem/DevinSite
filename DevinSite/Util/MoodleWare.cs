
namespace DevinSite.Util;

/// <summary>
/// This class is responsible for downloading and parsing a students schedule using a URL from Moodle. Instructions can be viewed when setting <see cref="MoodleString"/>
/// </summary>
public static class MoodleWare
{
    internal static string _moodleString = "http://classes.lanecc.edu/calendar/export_execute.php?userid=110123&authtoken=9688ee8ecad434630fe9e7b8120a93c9a138b350&preset_what=all&preset_time=weeknext";

    public enum AssignmentPart
    {
        Title,
        DueDate,
        Details,
        Course,
    }
    /// <summary>
    /// a dictionary mapping the tags to look for at the beginning and end of an Assignment sectionn.
    /// pass an <see cref="AssignmentPart"/> as the key. the returned value is in the form (start_tag, end_tag)
    /// </summary>
    static readonly Dictionary<AssignmentPart, (string, string)> AssignmentPartsDictionary = new()
    {
        { AssignmentPart.Title, ( "SUMMARY:", "DESCRIPTION:" ) },
        { AssignmentPart.Details, ("DESCRIPTION:" , "CLASS:") },
        { AssignmentPart.Course, ( "CATEGORIES:", "END:" ) },
    };

    /// <summary>
    /// Use the following steps to get the URL for your moodle calendar. Set <see cref="MoodleString"/> to your URL to download your personal calendar from moodle.
    /// <br /><b>STEPS:</b><br />
    /// - login to your Moodle account.
    /// - Navigate to your Calendar
    /// - On the calendar page, select the week or month option.
    /// - underneath the calendar element, you will find a button that reads, "Export Calendar" you should click this button.
    /// - on the next page are two sets of radio button options, select this or next WEEK and all events.
    /// - click the "Get Calendar URL" button
    /// - copy the generated URL to your clipboard, and past into your code. This URL is what the <see cref="MoodleString"/> Property should be set to.
    /// </summary>
    public static string MoodleString
    {
        get => _moodleString;
        set => _moodleString = value;
    }

    public static List<Assignment> GetCalendar()
    {
        using (var httpClient = new HttpClient())
        {
            // retrieve calendar from moodle
            string response = httpClient.GetStringAsync(_moodleString).Result;
            return ParseIcal(response);
        }
    }
    /// <summary>
    /// Given a string containing the response from moodles calendar API. Not meant for use outside the <see cref="MoodleWare"/> class.
    /// </summary>
    /// <param name="icsString">response from moodle containing a <see cref="Ical.Net.Calendar"/></param>
    /// <returns></returns>
    internal static List<Assignment> ParseIcal(string icsString)
    {
        List<Assignment> assignmentList = new();
        // split on newlines
        string[]? lines = icsString.Split("\r\n");
        int negativeId = 0;

        while (lines is not null && lines.Any())
        {
            negativeId--;
            Assignment assignment = new();
            assignment.CourseId = negativeId;
            assignment.Title = PartExtractor(AssignmentPart.Title, lines!, out lines);
            assignment.Details = PartExtractor(AssignmentPart.Details, lines!, out lines);
            
            Course course = new() { Assignments = new() { assignment } };
            course.Title = PartExtractor(AssignmentPart.Course, lines!, out lines);
           
            assignmentList.Add(assignment);
        }
        return assignmentList;
    }

    /// <summary>
    /// Extract the indicated <see cref="AssignmentPart"/> from an ICalendar .ics file.
    /// An ICalendar file is downloaded using the <see cref="MoodleWare.MoodleString"/> of a <see cref="MoodleWare"/> instance.
    /// The method looks for the 'CLASS:' and 'DESCRIPTION:' tags.
    /// Everything between the tags is concatinated and the tags discarded.
    /// The out parameter is then set to the remaining lines of text and the extracted description returned.
    /// </summary>
    /// <param name="lines">the response from http client after using <see cref="HttpClient.GetAsync(Uri?)"/> then reading with <see cref="HttpClient"/> an icalendar file.</param>
    /// <param name="lineDetailsTakenOut">the remaining line of text after the <see cref="Assignment.Details"/> is extracted.</param>
    /// <returns>the title that is extracted from the array of lines</returns>
    public static string PartExtractor(AssignmentPart toRemove, string[] lines, out string[]? linesPartTakenOut)
    {
        // extract course
        List<string> list;
        if (lines is null)
        {
            linesPartTakenOut = null;
            return "";
        }
        else list = lines.ToList();

        (string start, string end) = AssignmentPartsDictionary[toRemove];
        int courseIndex = list.FindIndex(line => line.StartsWith(start));
        int endCourse = list.FindIndex(line => line.StartsWith(end));
        if (courseIndex < 0 || endCourse <= 0)
        {
            linesPartTakenOut = null;
            return "";
        }
        string courseTitle = String.Concat(lines[courseIndex..endCourse]).Replace(start, "").Replace(end, "");
        linesPartTakenOut = lines[endCourse..];
        return courseTitle;
    }
}


