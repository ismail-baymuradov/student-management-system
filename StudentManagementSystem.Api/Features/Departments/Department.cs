using StudentManagementSystem.Api.Features.Courses;
using StudentManagementSystem.Api.Features.Instructors;

namespace StudentManagementSystem.Api.Features.Departments;


public class Department
{
    public int Id { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public ICollection<Course> Courses { get; set; } = new List<Course>();
    public ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}