﻿
namespace DevinSite.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<Course> Courses { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

}

