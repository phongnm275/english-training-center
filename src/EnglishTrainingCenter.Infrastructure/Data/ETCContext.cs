using Microsoft.EntityFrameworkCore;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Infrastructure.Data;

/// <summary>
/// Database context for English Training Center
/// </summary>
public class ETCContext : DbContext
{
    public ETCContext(DbContextOptions<ETCContext> options) : base(options) { }

    // ============================================================================
    // Users & Roles
    // ============================================================================
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    // ============================================================================
    // Students
    // ============================================================================
    public DbSet<Student> Students { get; set; } = null!;
    public DbSet<StudentCourse> StudentCourses { get; set; } = null!;

    // ============================================================================
    // Courses & Classes
    // ============================================================================
    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Class> Classes { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;

    // ============================================================================
    // Instructors
    // ============================================================================
    public DbSet<Instructor> Instructors { get; set; } = null!;
    public DbSet<InstructorQualification> InstructorQualifications { get; set; } = null!;

    // ============================================================================
    // Payments
    // ============================================================================
    public DbSet<Invoice> Invoices { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;

    // ============================================================================
    // Evaluation
    // ============================================================================
    public DbSet<Assignment> Assignments { get; set; } = null!;
    public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; } = null!;
    public DbSet<Exam> Exams { get; set; } = null!;
    public DbSet<Grade> Grades { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships and constraints
        // This will be populated with fluent API configurations

        // Example: Configure Student-User relationship
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure indexes
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email);

        modelBuilder.Entity<StudentCourse>()
            .HasIndex(sc => new { sc.StudentId, sc.ClassId })
            .IsUnique();

        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.Status);

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.Status);

        modelBuilder.Entity<Grade>()
            .HasIndex(g => new { g.StudentId, g.ExamId })
            .IsUnique();
    }

    /// <summary>
    /// Override SaveChanges to set modification dates
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedDate = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
