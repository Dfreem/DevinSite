global using DevinSite;
global using DevinSite.ViewModels;
global using DevinSite.Util;
global using MoodleWare = DevinSite.Util.MoodleWare;
global using CalendarHelper = DevinSite.Util.CalendarHelpers;
global using DevinSite.Data;
global using DevinSite.Models;
global using DevinSite.Controllers;

global using Ical;
global using Ical.Net;
global using Ical.Net.Serialization;
global using Ical.Net.Evaluation;
global using Ical.Net.CalendarComponents;
global using Ical.Net.DataTypes;

global using System;
global using System.Text;
global using System.Linq;
global using System.Threading;
global using System.Diagnostics;
global using System.Threading.Tasks;
global using System.Text.Encodings.Web;
global using System.Collections.Generic;
global using System.Runtime.InteropServices;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;

global using IServiceProvider = System.IServiceProvider;

global using Microsoft.EntityFrameworkCore;

global using Microsoft.Extensions.Logging;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Html; 
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// ========== Home ==========
// TODO link course buttons and use course names for values
// TODO this week / Next week buttons
// TODO User Profile page functionality: change user info, submit button for changing password.
// TODO Note Pad