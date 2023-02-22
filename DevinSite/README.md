# Devin Site

## Purpose

This ASP.NET Web App if a student schedule and assignment organizer.
It is built for the CS 286N ASP.NET course at Lane Community College.

## Details

The app uses a URL obtained from a students personal account on their Moodle.com based school portal. This will work however for any Calendar URL that downloads an iCalendar .ics file. The `DevinSite.Util.MoodleWare` class is responsable for downloading and parsing the calendar file into `Assignment` Objects. Following you will find instructions for obtaining your personal Calendar URL and inserting it into the project.

### Calendar URL Instructions

1. Make a copy and/or change the name of the `examplesettings.json` file to `appsettings.json`
   <br/>
2. Login to your Moodle student account
   <br/>
3. Navigate to the Calendar page.
   <br/>
4. Make sure to be on the Day or Month view, You should see a button the says "Export Calendar"
   <br/>
5. select eiter this week or next week as the time frame, and all events.
   <br/>
6. push "Get Calendar URL"
   <br/>
7. A URL is generated and displayed at the bottom of the screen, copy that and insert the uid and authtoken portions into the `appsettings.json` file you copied.
   <br/>


> **Don't Forget** to run
> `$ dotnet ef database update`
> OR
> `PM> Update-Database`
