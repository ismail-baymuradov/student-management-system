using StudentManagementSystem.Api.Features.Departments;
using StudentManagementSystem.Api.Features.CourseOfferings;

namespace StudentManagementSystem.Api.Features.Instructors;

public class Instructor
{
    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string EmployeeNumber { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public ICollection<CourseOffering> CourseOfferings { get; set; }
    = new List<CourseOffering>();
}