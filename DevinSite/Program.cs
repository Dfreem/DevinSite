
var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("SQL_SERVER_CONNECTION");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));


builder.Services.AddTransient<ISiteRepository, SiteRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();
// This call adds a Role manager to the services container.
builder.Services.AddIdentity<Student, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
        //.AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseResponseCaching();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
// use scoped service provider to call SeedData initialization.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var cal = await MoodleWare.GetCalendarAsync(services);
    await context.Assignments.AddRangeAsync(cal);
    await context.SaveChangesAsync();
    //Init in the static SeedData class checks for the presence of data in the database before seeding or returning.
    //await SeedData.Init(services);
}

app.Run();

