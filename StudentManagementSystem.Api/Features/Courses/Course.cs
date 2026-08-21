using StudentManagementSystem.Api.Features.Departments;
using StudentManagementSystem.Api.Features.CourseOfferings;
using StudentManagementSystem.Api.Features.Courses.Prerequisites;

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

    public ICollection<CoursePrerequisite> Prerequisites { get; set; }
        = new List<CoursePrerequisite>();

    public ICollection<CoursePrerequisite> RequiredByCourses { get; set; }
        = new List<CoursePrerequisite>();
}