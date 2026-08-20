using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.GetCourseStudents;

public sealed record CourseStudentResponse(
    int EnrollmentId,
    int StudentId,
    string StudentNumber,
    string FirstName,
    string LastName,
    DateTimeOffset EnrolledAt
);

public static class GetCourseStudentsEndpoint
{
    public static async Task<IResult> Handle(
        int courseOfferingId,
        StudentManagementDbContext db)
    {
        var offeringExists = await db.CourseOfferings
            .AnyAsync(o => o.Id == courseOfferingId);

        if (!offeringExists)
        {
            return Results.NotFound(new
            {
                message = "Course offering was not found."
            });
        }

        var students = await db.Enrollments
            .AsNoTracking()
            .Where(e =>
                e.CourseOfferingId == courseOfferingId &&
                e.Status == EnrollmentStatus.Active)
            .OrderBy(e => e.Student.StudentNumber)
            .Select(e => new CourseStudentResponse(
                e.Id,
                e.StudentId,
                e.Student.StudentNumber,
                e.Student.FirstName,
                e.Student.LastName,
                e.EnrolledAt
            ))
            .ToListAsync();

        return Results.Ok(students);
    }
}