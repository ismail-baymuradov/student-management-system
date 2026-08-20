using StudentManagementSystem.Api.Features.Enrollments;

namespace StudentManagementSystem.Api.Features.Students;


public class Student
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string StudentNumber { get; set; } = string.Empty;

    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}