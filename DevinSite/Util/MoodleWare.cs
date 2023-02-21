
namespace DevinSite.Util;

public static class MoodleWare
{
    public enum MoodleOptions
    {
        NextMonth,
        ThisMonth,
        NextWeek,
        ThisWeek
    }
    public static readonly Dictionary<Enum, string> Options = new()
    {
        { MoodleOptions.NextMonth, "monthnow" },
        { MoodleOptions.ThisMonth, "monththis" },
        { MoodleOptions.ThisWeek, "weeknow" },
        { MoodleOptions.NextWeek, "weeknext" }
    };

    public enum AssignmentPart
    {
        Title,
        Details,
        DueDate,
        CourseTitle,
        Instructor
    }
    public static readonly Dictionary<AssignmentPart, (string, string)> Dilimiters = new()
    {
        { AssignmentPart.Title, ("SUMMARY:", "DESCRIPTION:")},
        { AssignmentPart.Details, ("DESCRIPTION:", "CLASS:") },
        { AssignmentPart.DueDate, ("DTSTART:", "DTEND:")},
        { AssignmentPart.CourseTitle, ("CATEGORIES:", "END:" ) },

    };

    public static async Task<List<Assignment>> GetCalendarAsync(string moodleString)
    {
        // create new HttpClient seperate from the web app's HttpClient.
        using (var httpClient = new HttpClient())
        {
            // user moodle string to GET the students calendar from Moodle, store as a string.
            var icsData = await httpClient.GetStringAsync(moodleString);

            // Split response string into seperate lines of text.
            string[] lines = icsData.Split("\r\n");

            // Create assignments list the will hold the assignments and be returned from the method.
            List<Assignment> assignments = new();

            // search each line in the response.
            for (int i = 0; i < lines.Length; i++)
            {
                // if it's not the end of the calendar, parse the assignment.
                if (!lines[i].Contains("END:VCALENDAR"))
                {
                    // the matter in which these assignment parts are parsed does matter.
                    // First - DueDate
                    string dt = ParsePart(in lines, AssignmentPart.DueDate, out lines);

                    // splits out the time, only uses the date.
                    string date = dt.Split('T')[0];

                    // Second - Course Title (Not the Assignment title)
                    var courseTitle = ParsePart(in lines, AssignmentPart.CourseTitle, out lines).Replace(')', ' ').Split('(');

                    // Course Title contains title and instructor.
                    // Create new Course Object using the Title and instructor.
                    Course parsedCourse = new()
                    {
                        Name = courseTitle[0],
                        Instructor = courseTitle[1].Split(" ")[0],
                        Assignments = new()
                    };
                    // Parse the rest of this chunck of the response into an assignment object.
                    Assignment assignment = new()
                    {
                        Title = ParsePart(in lines, AssignmentPart.Title, out lines),
                        GetCourse = parsedCourse,
                        Details = ParsePart(in lines, AssignmentPart.Details, out lines),
                        DueDate = new DateTime(int.Parse(date[..4]), int.Parse(date[4..6]), int.Parse(date[6..]))
                    };
                    // add this assignment to the list and return.
                    assignments.Add(assignment);
                }
            }
            return assignments;
        }
    }

    public static string ParsePart(in string[] lines, AssignmentPart partToParse, out string[] linesMinusAssignmentPart)
    {
        (string start, string stop) = Dilimiters[partToParse];
        string parsedPart = "";
        List<string> lineList = lines.ToList();
        int startIndex = lineList.FindIndex(l => l.StartsWith(start));
        int endIndex = lineList.FindIndex(l => l.StartsWith(stop));
        if (startIndex <= endIndex && startIndex > 0)
        {
            parsedPart = String.Concat(lines[startIndex..endIndex]).Replace(start, "").Replace(stop, "");
            linesMinusAssignmentPart = lines[endIndex..];
            return parsedPart;

        }
        linesMinusAssignmentPart = lines;
        return "";
    }
    public static string AssembleMoodleString(string options, IConfiguration config)
    {
        return config["Moodle:BASE"] +
                "userid=" + config["Moodle:UID"] +
                "&" + config["Moodle:TOKEN"] +
                config["Moodle:OPTIONS"] + options;
    }
    // TODO move this to home controller
    //public static List<Assignment>? SearchAssignmentsByDate(DateTime toSearch, List<Assignment> assignments)
    //{
    //    assignments.Sort((x, y) => x.DueDate.CompareTo(y.DueDate));
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
}