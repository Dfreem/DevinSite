global using DevinSite;
global using DevinSite.Repositories;
global using DevinSite.ViewModels;
global using DevinSite.Util;
global using static DevinSite.Util.MoodleWare;
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

global using AspNetCoreHero.ToastNotification;

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

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Html; 
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.WebUtilities;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.Identity.Client.Extensions.Msal;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;

global using NodaTime.Extensions;
// ========== Home ==========
// TODO link course buttons and use course names for values
// TODO this week / Next week buttons
// TODO Format User Profile Page.
// TODO Note Pad
// TODO Add Course parsing to MoodleWare
// TODO Fix search by date
// TODO fix size of input for notes
// TODO Classes list
// TODO style assignments
// TODO format assignment partial
// TODO replace delete trash can with done checkbox
// TODO change selected assignment into partial
// TODO Test no assignments
