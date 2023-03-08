
using System.Text.RegularExpressions;

namespace DevinSite.Util;

public static class MoodleWare
{
    /// <summary>
    /// timeframe options for retreiving VCALENDAR object<br />
    /// - this week <br />
    /// - week week <br />
    /// - this month <br />
    /// - next month <br />
    /// </summary>
    public enum MoodleOptions
    {
        NextMonth,
        ThisMonth,
        NextWeek,
        ThisWeek
    }
    /// <summary>
    /// Convert MoodleOptions to URL encoded Option portiomn of the MoodleString
    /// </summary>
    public static readonly Dictionary<MoodleOptions, string> Options = new()
    {
        { MoodleOptions.NextMonth, "monthnow" },
        { MoodleOptions.ThisMonth, "monththis" },
        { MoodleOptions.ThisWeek, "weeknow" },
        { MoodleOptions.NextWeek, "weeknext" }
    };
    /// <summary>
    /// Options indicating which part of the assignment to look for while paring.
    /// </summary>
    public enum AssignmentPart
    {
        Title,
        Details,
        DueDate,
        CourseTitle,
        Instructor
    }
    /// <summary>
    /// Convert <see cref="AssignmentPart"/> into text dilimiters marking the start and end of the text to extract.
    /// </summary>
    static readonly Dictionary<AssignmentPart, (string, string)> Dilimiters = new()
    {
        { AssignmentPart.Title, ("SUMMARY:", "DESCRIPTION:")},
        { AssignmentPart.Details, ("DESCRIPTION:", "CLASS:") },
        { AssignmentPart.DueDate, ("DTSTART:", "DTEND:")},
        { AssignmentPart.CourseTitle, ("CATEGORIES:", "END:" ) },

    };
    /// <summary>
    /// Using a calendar download URL obtained from a student's personal moodle account,
    /// this method retrieves the students calendar from the website,
    /// then parses the response into individual <see cref="Assignment"/>'s.
    /// To add your own connection string, see instructions in README or on the Views.Account.Index View. 
    /// </summary>
    /// <param name="moodleString">the URL used to download the VCALENDAR containing .ics file from Moodle.</param>
    /// <returns></returns>
    public static async Task<List<Assignment>> GetCalendarAsync(string moodleString)
    {
        // Create assignments list the will hold the assignments and be returned from the method.
        List<Assignment> assignments = new();

        // create new HttpClient seperate from the web app's HttpClient.
        string icsData = await GetCalendarAsStringAsync(moodleString);

        // Split response string into seperate lines of text.
        string[] lines = icsData.Split("\r\n");

        // search each line in the response.
        for (int i = 0; i < lines.Length; i++)
        {
            // if it's not the end of the calendar, parse the assignment.
            if (!lines[i].Contains("END:VCALENDAR"))
            {
                // the matter in which these assignment parts are parsed does matter.
                // First - DueDate
                string dt = ParsePart(in lines, AssignmentPart.DueDate, out lines);

                // seperate time and date parts
                var holdDT = dt.Split('T');
                string date = holdDT[0];
                string time = holdDT[^1];

                // Second - Course Title (Not the Assignment title)
                var courseTitle = ParsePart(in lines, AssignmentPart.CourseTitle, out lines).Replace(')', ' ').Split('(');

                // Parse the rest of this chunck of the response into an assignment object.
                Assignment assignment = new()
                {
                    Title = ParsePart(in lines, AssignmentPart.Title, out lines),
                    Details = ParsePart(in lines, AssignmentPart.Details, out lines),
                    DueDate = new DateTime(int.Parse(date[..4]), int.Parse(date[4..6]), int.Parse(date[6..]), int.Parse(time[0..2]), int.Parse(time[2..4]), 00)
                };
                // Course Title contains title and instructor.
                // Create new Course Object using the Title and instructor.
                Course parsedCourse = new()
                {
                    Name = courseTitle[0],
                    Instructor = courseTitle[1].Split(" ")[0],
                    Assignments = new()
                };
                assignment.GetCourse = parsedCourse;
                // add this assignment to the list and return.
                assignments.Add(assignment);
            }
        }
        int index = assignments.Count - 1;
        assignments.RemoveAt(index);
        foreach (var item in assignments)
        {
            Console.WriteLine(item.ToString());
            item.Details = Regex.Replace(item.Details!, @"\\r|\\n|\\r\\n?", HtmlString.NewLine.ToString());
            item.Title = Regex.Replace(item.Title, @"\\r|\\n|\\r\\n?", HtmlString.NewLine.ToString());
            item.Title = String.Concat(
                item.Title.Split('\\',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries));
        }

        return assignments;
    }

    /// <summary>
    /// Parse out the specified portion an <see cref="Assignment"/> from an array containing each line of an icalendar file.
    /// </summary>
    /// <param name="lines">icalendar file broken into seperate lines of text.</param>
    /// <param name="partToParse">an <see cref="AssignmentPart"/>an Enum indicating what part of the assignment is being parsed currently.</param>
    /// <param name="linesMinusAssignmentPart">the icalendar text after the parsed part has been removed.</param>
    /// <returns>the text of the part that was parsed or an empty string.</returns>
    static string ParsePart(in string[] lines, AssignmentPart partToParse, out string[] linesMinusAssignmentPart)
    {
        // Dilimiters holds the beginning and end point markers for each related part of an assignment within a VCALENDAR object.
        (string start, string stop) = Dilimiters[partToParse];
        string parsedPart = "";

        // I like lists...
        List<string> lineList = lines.ToList();
        int startIndex = lineList.FindIndex(l => l.StartsWith(start));
        int endIndex = lineList.FindIndex(l => l.StartsWith(stop));
        if (startIndex < 0 || endIndex < 0)
        {
            linesMinusAssignmentPart = Array.Empty<string>();
            return "";
        }
        // safety checking to confirm that the two indexes are in the proper order.
        if (startIndex <= endIndex && startIndex >= 0)
        {
            // removing the delimiters from the text
            parsedPart = String.Concat(lines[startIndex..endIndex]).Replace(start, "").Replace(stop, "");

            // Remove parsed item from icalendar
            linesMinusAssignmentPart = lines[endIndex..];
            return parsedPart;

        }
        //if we made it here there are probably no items left to parse
        linesMinusAssignmentPart = lines;
        return "";
    }

    public static async Task<string> GetCalendarAsStringAsync(string source)
    {
        try
        {
            return await File.ReadAllTextAsync(source);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine("not a filepath\n" + e.Message);
            using (var httpClient = new HttpClient())
            {
                // user moodle string to GET the students calendar from Moodle, store as a string.
                return await httpClient.GetStringAsync(source);
            }
        }
    }

    public static List<Course> ParseCourses(List<Assignment> getAssignments)
    {

        List<Course> courses = new();
        List<string> names = new();
        
        foreach (Assignment assignment in getAssignments)
        {
            if (!names.Contains(assignment.GetCourse!.Name))
            {
                names.Add(assignment.GetCourse.Name);
                courses.Add(assignment.GetCourse!);
            }
        }
        return courses;
    }
}