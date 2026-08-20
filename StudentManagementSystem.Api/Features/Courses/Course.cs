using StudentManagementSystem.Api.Features.Departments;
using StudentManagementSystem.Api.Features.CourseOfferings;

namespace StudentManagementSystem.Api.Features.Courses;

public class Course
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int Credits { get; set; }
public int DepartmentId { get; set; }

public Department Department { get; set; } = null!;

public ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
    
}