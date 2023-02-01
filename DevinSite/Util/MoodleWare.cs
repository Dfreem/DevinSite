
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DevinSite.Util;

public class MoodleWare : Calendar
{
    private string _moodleString = "https://classes.lanecc.edu/calendar/export_execute.php?userid=110123&authtoken=9688ee8ecad434630fe9e7b8120a93c9a138b350&preset_what=all&preset_time=monthnext";

    public MoodleWare()
    {
    }

    public MoodleWare(string moodleString)
    {
        _moodleString = moodleString;
    }

    public string MoodleString
    {
        get => _moodleString;
        set => _moodleString = value;
    }
    /// <summary>
    /// instead of returning the value directly, GetCalendarAsync() stores the retrieved Calendar
    /// inside this instances Calendar Property.
    /// </summary>
    public async void GetCalendarAsync()
    {
        using (var httpClient = new HttpClient())
        {
            var response = await httpClient.GetAsync(_moodleString);
            var icsData = await response.Content.ReadAsStringAsync();
            string[] splits = icsData.ToString()?.Split("VEVENT")!;
            string[] summaries = icsData.ToString()?.Split("SUMMARY")!;
            string[] descriptions = icsData.ToString()?.Split("DESCRIPTION")!;
            //for (int i = 0; i < descriptions.Length; i++)
            //{

            //    Console.WriteLine(descriptions[i]);
            //}
            Console.WriteLine("****SPLITS****" + splits[1] + "\r\n" + splits[1]);
            //Console.WriteLine("****SUMMARIES****" + summaries[1] + "\r\n");
            //Console.WriteLine("****DESCRIPTIONS****" + descriptions[1] + "\r\n");

        }
    }
}

