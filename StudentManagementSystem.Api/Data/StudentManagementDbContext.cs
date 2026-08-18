using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Features.Students;

namespace StudentManagementSystem.Api.Data;

public class StudentManagementDbContext : DbContext
{
    public StudentManagementDbContext(
        DbContextOptions<StudentManagementDbContext> options)
        : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
    }
}