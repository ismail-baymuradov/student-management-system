using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Features.Students;
using StudentManagementSystem.Api.Features.Courses;
using StudentManagementSystem.Api.Features.Departments;
using StudentManagementSystem.Api.Features.Semesters;
using StudentManagementSystem.Api.Features.Instructors;
using StudentManagementSystem.Api.Features.CourseOfferings;

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
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<CourseOffering> CourseOfferings => Set<CourseOffering>();
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

                course.HasOne(c => c.Department)
    .WithMany(d => d.Courses)
    .HasForeignKey(c => c.DepartmentId)
    .OnDelete(DeleteBehavior.Restrict);

        // Department configuration
        var department = modelBuilder.Entity<Department>();

department.Property(d => d.Code)
    .IsRequired()
    .HasMaxLength(20);

department.Property(d => d.Name)
    .IsRequired()
    .HasMaxLength(200);

department.HasIndex(d => d.Code)
    .IsUnique();
    
    var semester = modelBuilder.Entity<Semester>();

semester.Property(s => s.Name)
    .IsRequired()
    .HasMaxLength(100);

semester.Property(s => s.StartDate)
    .HasColumnType("date");

semester.Property(s => s.EndDate)
    .HasColumnType("date");

semester.Property(s => s.RegistrationStart)
    .HasColumnType("date");

semester.Property(s => s.RegistrationEnd)
    .HasColumnType("date");

semester.ToTable(t =>
{
    t.HasCheckConstraint(
        "CK_Semesters_StartBeforeEnd",
        "[StartDate] < [EndDate]");

    t.HasCheckConstraint(
        "CK_Semesters_RegistrationStartBeforeEnd",
        "[RegistrationStart] < [RegistrationEnd]");

    t.HasCheckConstraint(
        "CK_Semesters_RegistrationStartsBeforeSemester",
        "[RegistrationStart] <= [StartDate]");

    t.HasCheckConstraint(
        "CK_Semesters_RegistrationEndsWithinSemester",
        "[RegistrationEnd] <= [EndDate]");
});
    

    var instructor = modelBuilder.Entity<Instructor>();

instructor.Property(i => i.FirstName)
    .IsRequired()
    .HasMaxLength(100);

instructor.Property(i => i.LastName)
    .IsRequired()
    .HasMaxLength(100);

instructor.Property(i => i.EmployeeNumber)
    .IsRequired()
    .HasMaxLength(20);

instructor.HasIndex(i => i.EmployeeNumber)
    .IsUnique();

instructor.HasOne(i => i.Department)
    .WithMany(d => d.Instructors)
    .HasForeignKey(i => i.DepartmentId)
    .OnDelete(DeleteBehavior.Restrict);
    
    var offering = modelBuilder.Entity<CourseOffering>();

offering.HasOne(o => o.Course)
    .WithMany(c => c.CourseOfferings)
    .HasForeignKey(o => o.CourseId)
    .OnDelete(DeleteBehavior.Restrict);

offering.HasOne(o => o.Semester)
    .WithMany(s => s.CourseOfferings)
    .HasForeignKey(o => o.SemesterId)
    .OnDelete(DeleteBehavior.Restrict);

offering.HasOne(o => o.Instructor)
    .WithMany(i => i.CourseOfferings)
    .HasForeignKey(o => o.InstructorId)
    .OnDelete(DeleteBehavior.Restrict);

offering.HasIndex(o => new
{
    o.CourseId,
    o.SemesterId,
    o.Section
})
.IsUnique();

offering.Property(o => o.StartTime)
    .HasColumnType("time");

offering.Property(o => o.EndTime)
    .HasColumnType("time");

offering.ToTable(t =>
{
    t.HasCheckConstraint(
        "CK_CourseOfferings_Section",
        "[Section] > 0");

    t.HasCheckConstraint(
        "CK_CourseOfferings_Capacity",
        "[Capacity] > 0");

    t.HasCheckConstraint(
        "CK_CourseOfferings_StartBeforeEnd",
        "[StartTime] < [EndTime]");
});
    
    }

    
}