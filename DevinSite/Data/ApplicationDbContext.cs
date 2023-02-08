
using Microsoft.EntityFrameworkCore;

namespace DevinSite.Data;

public class ApplicationDbContext : IdentityDbContext<Student>
{
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

