
var builder = WebApplication.CreateBuilder(args);
string connection;

// if My Machine
if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
{
    connection = builder.Configuration.GetConnectionString("MySqlConnection");
}
// if Not MacOS
else
{
    connection = builder.Configuration.GetConnectionString("DefaultConnection");
}

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connection, MySqlServerVersion.Parse("mysql-8.0.30")));

builder.Services.AddTransient<ISiteRepository, SiteRepository>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// This call adds a Role manager to the services container.
builder.Services.AddIdentity<Student, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = true)
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddDefaultUI();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddAuthorization(options =>
    options.AddPolicy("Administrator", policy =>
        policy.RequireClaim("Admin")));

var app = builder.Build();

// use scoped service provider to call SeedData initialization.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Init in the static SeedData class checks for the presence of data in the database before seeding or returning.
    SeedData.Init(services);
}

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
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

