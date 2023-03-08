

var builder = WebApplication.CreateBuilder(args);
builder.Logging.SetMinimumLevel(LogLevel.Critical).ClearProviders();
string connection = builder.Configuration.GetConnectionString("AZURE_MYSQL_CONNECTION")!;

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connection, MySqlServerVersion.Parse("mysql-8.0")));


builder.Services.AddTransient<ISiteRepository, SiteRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddHttpClient();
// This call adds a Role manager to the services container.
builder.Services.AddIdentity<Student, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
        //.AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
builder.Services
    .AddNotyf(config =>
    {
        config.DurationInSeconds = 6;
        config.IsDismissable = true;
        config.Position = NotyfPosition.TopRight;
    });
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
app.UseNotyf();
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
    //Init in the static SeedData class checks for the presence of data in the database before seeding or returning.
    SeedData.Init(services, app.Configuration);
    await SeedRoles.SeedStudentRole(services);
    await SeedRoles.SeedAdminRole(services);
    
}

app.Run();

