
namespace DevinSite.Util;

public static class MoodleWare
{
    public static string MoodleString { get; set; } = "https://classes.lanecc.edu/calendar/export_execute.php?userid=110123&authtoken=9688ee8ecad434630fe9e7b8120a93c9a138b350&preset_what=all&preset_time=monthnext";
    public static async Task GetCalendarAsync(IServiceProvider services)
    {
        using (var httpClient = new HttpClient())
        {
            var icsData = await httpClient.GetStringAsync(MoodleString);

            string[] splitOnVevent = icsData.Split(":VEVENT");
            foreach (var vevent in splitOnVevent)
            {
                if (!vevent.StartsWith("BEGIN"))
                {
                    Console.WriteLine("###" + vevent);
                }
            }
        }
  
    }
}

