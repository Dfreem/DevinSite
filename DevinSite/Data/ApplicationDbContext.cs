
namespace DevinSite.Data;

public class ApplicationDbContext : IdentityDbContext<Student>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.GetStudent)
            .WithMany(s => s.GetEnrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.GetCourse)
            .WithMany(c => c.GetEnrollments)
            .HasForeignKey(e => e.CourseId);

        modelBuilder.Entity<Assignment>()
            .HasOne(c => c.GetCourse)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId);
    }
}

