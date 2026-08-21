using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Features.Students;
using StudentManagementSystem.Api.Features.Courses;
using StudentManagementSystem.Api.Features.Departments;
using StudentManagementSystem.Api.Features.Semesters;
using StudentManagementSystem.Api.Features.Instructors;
using StudentManagementSystem.Api.Features.CourseOfferings;
using StudentManagementSystem.Api.Features.Enrollments;
using StudentManagementSystem.Api.Features.Courses.Prerequisites;

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
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<CoursePrerequisite> CoursePrerequisites => Set<CoursePrerequisite>();

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

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.EnrolledAt)
                .IsRequired();

            entity.HasIndex(e => new
            {
                e.StudentId,
                e.CourseOfferingId
            })
            .IsUnique();

            entity.HasIndex(e => new
            {
                e.CourseOfferingId,
                e.Status
            });

            entity.HasIndex(e => new
            {
                e.StudentId,
                e.Status
            });

            entity.Property(e => e.Grade)
                .HasPrecision(5, 2);

            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Enrollments_Grade",
                    "[Grade] IS NULL OR ([Grade] >= 0 AND [Grade] <= 100)");
            });


            entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.CourseOffering)
                .WithMany(o => o.Enrollments)
                .HasForeignKey(e => e.CourseOfferingId)
                .OnDelete(DeleteBehavior.Restrict);


        });

        modelBuilder.Entity<CoursePrerequisite>(entity =>
        {
            entity.HasKey(cp => new
            {
                cp.CourseId,
                cp.PrerequisiteCourseId
            });

            entity.HasOne(cp => cp.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(cp => cp.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(cp => cp.PrerequisiteCourse)
                .WithMany(c => c.RequiredByCourses)
                .HasForeignKey(cp => cp.PrerequisiteCourseId)
                .OnDelete(DeleteBehavior.Restrict);
        });




    }


}