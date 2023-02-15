
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
        using (var httpClient = new HttpClient())
        {
            var icsData = await httpClient.GetStringAsync(moodleString);
            string[] lines = icsData.Split("\r\n");
            List<Assignment> assignments = new();
            for (int i = 0; i < lines.Length; i++)
            {
                if (!lines[i].Contains("END:VCALENDAR"))
                {
                    string dt = ParsePart(in lines, AssignmentPart.DueDate, out lines);
                    string date = dt.Split('T')[0];
                    //Console.WriteLine(int.Parse(date[..4]) + "\n" + int.Parse(date[4..6]) + "\n" + int.Parse(date[6..]));

                    Assignment assignment = new()
                    {
                        Title = ParsePart(in lines, AssignmentPart.Title, out lines),
                        GetCourse = (Course)ParsePart(in lines, AssignmentPart.CourseTitle, out lines),
                        Details = ParsePart(in lines, AssignmentPart.Details, out lines),
                        DueDate = new DateTime(int.Parse(date[..4]), int.Parse(date[4..6]), int.Parse(date[6..]))
                    };
                    Console.WriteLine(assignment.ToString()); ;
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

    public static List<Assignment>? SearchAssignmentsByDate(DateTime toSearch, List<Assignment> assignments)
    {
        assignments.Sort((x, y) => x.DueDate.CompareTo(y.DueDate));
        string moodleString = "";
        if (DateTime.Today < toSearch)
        {
            moodleString += MoodleWare.MoodleOptions.NextWeek;
        }
        else
        {
            moodleString += MoodleWare.MoodleOptions.ThisWeek;
        }
        return assignments.FindAll(a => a.DueDate.Day.Equals(toSearch.Day));
    }
}