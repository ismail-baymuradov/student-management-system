using StudentManagementSystem.Api.Features.Courses;
using StudentManagementSystem.Api.Features.Instructors;
using StudentManagementSystem.Api.Features.Semesters;

namespace StudentManagementSystem.Api.Features.CourseOfferings;

public class CourseOffering
{
    public int Id { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public int SemesterId { get; set; }
    public Semester Semester { get; set; } = null!;

    public int Section { get; set; }

    public int Capacity { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int? InstructorId { get; set; }
    public Instructor? Instructor { get; set; }
}