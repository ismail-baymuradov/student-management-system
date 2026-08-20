using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.GetStudentCourses;

public sealed record StudentCourseResponse(
    int EnrollmentId,
    int CourseOfferingId,
    string CourseCode,
    string CourseName,
    int Section,
    string SemesterName,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? InstructorName,
    DateTimeOffset EnrolledAt
);

public static class GetStudentCoursesEndpoint
{
    public static async Task<IResult> Handle(
        int studentId,
        StudentManagementDbContext db)
    {
        var studentExists = await db.Students
            .AnyAsync(s => s.Id == studentId);

        if (!studentExists)
        {
            return Results.NotFound(new
            {
                message = "Student was not found."
            });
        }

        var courses = await db.Enrollments
            .AsNoTracking()
            .Where(e =>
                e.StudentId == studentId &&
                e.Status == EnrollmentStatus.Active)
            .OrderBy(e => e.CourseOffering.Course.Code)
            .Select(e => new StudentCourseResponse(
                e.Id,
                e.CourseOfferingId,
                e.CourseOffering.Course.Code,
                e.CourseOffering.Course.Name,
                e.CourseOffering.Section,
                e.CourseOffering.Semester.Name,
                e.CourseOffering.DayOfWeek,
                e.CourseOffering.StartTime,
                e.CourseOffering.EndTime,
                e.CourseOffering.Instructor == null
                    ? null
                    : e.CourseOffering.Instructor.FirstName
                      + " "
                      + e.CourseOffering.Instructor.LastName,
                e.EnrolledAt
            ))
            .ToListAsync();

        return Results.Ok(courses);
    }
}