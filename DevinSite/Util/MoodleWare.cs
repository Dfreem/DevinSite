
namespace DevinSite.Util;

public static class MoodleWare
{
    public static string MoodleString { get; set; } = "https://classes.lanecc.edu/calendar/export_execute.php?userid=110123&authtoken=9688ee8ecad434630fe9e7b8120a93c9a138b350&preset_what=all&preset_time=monthnext";
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
        { AssignmentPart.Title, ("SUMMARY", "DESCRIPTION:")},
        { AssignmentPart.Details, ("DESCRIPTION:", "CLASS") },
        { AssignmentPart.CourseTitle, ("CATEGORIES:", "CLASS:" ) },
        { AssignmentPart.Instructor, ("(", "BEGIN:") },
        { AssignmentPart.DueDate, ("DTSTART", "T")}
    };

    public static async Task<List<Assignment>> GetCalendarAsync(IServiceProvider services)
    {
        using (var httpClient = new HttpClient())
        {
            var icsData = await httpClient.GetStringAsync(MoodleString);
            string[] lines = icsData.Split("\r\n");
            List<Assignment> assignments = new();
            for (int i = 0; i < lines.Length; i++)
            {
                //var due = ParsePart(in lines, AssignmentPart.DueDate, out lines);
                //int year = int.Parse(due[0..3]);
                //int month = int.Parse(due[3..5]);
                //int day = int.Parse(due[5..]);
                Console.WriteLine(lines[i]);
                Assignment assignment = new()
                {
                    Title = ParsePart(in lines, AssignmentPart.Title, out lines),
                    GetCourse = (Course)ParsePart(in lines, AssignmentPart.CourseTitle, out lines),
                    Details = ParsePart(in lines, AssignmentPart.Details, out lines),
                    //DueDate = new DateTime(year, month, day)
                };
                Console.WriteLine(assignment.ToString()); ;
                assignments.Add(assignment);
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
        parsedPart = String.Concat(lines[startIndex..endIndex]).Replace(start, "").Replace(stop, "");
        linesMinusAssignmentPart = lines[endIndex..];
        return parsedPart;
    }

}