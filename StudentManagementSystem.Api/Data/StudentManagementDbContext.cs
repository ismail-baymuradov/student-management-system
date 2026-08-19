using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Features.Students;
using StudentManagementSystem.Api.Features.Courses;

namespace StudentManagementSystem.Api.Data;

public class StudentManagementDbContext : DbContext
{
    public StudentManagementDbContext(
        DbContextOptions<StudentManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Student configuration
        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(student => student.Id);

            entity.Property(student => student.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(student => student.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(student => student.StudentNumber)
                .IsRequired()
                .HasMaxLength(20);

            entity.HasIndex(student => student.StudentNumber)
                .IsUnique();
        });

        // Course configuration
        var course = modelBuilder.Entity<Course>();

        course.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);

        course.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);

        course.HasIndex(c => c.Code)
            .IsUnique();

        course.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Courses_Credits",
                "[Credits] >= 1 AND [Credits] <= 30"));
    }
}