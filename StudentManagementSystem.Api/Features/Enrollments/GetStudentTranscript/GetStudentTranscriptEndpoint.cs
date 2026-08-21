using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.GetStudentTranscript;

public sealed record TranscriptCourseResponse(
    int EnrollmentId,
    int CourseOfferingId,
    string CourseCode,
    string CourseName,
    string SemesterName,
    int Section,
    decimal Grade,
    bool Passed,
    DateTimeOffset GradedAt
);

public sealed record StudentTranscriptResponse(
    int StudentId,
    string StudentNumber,
    string FirstName,
    string LastName,
    IReadOnlyList<TranscriptCourseResponse> Courses
);

public static class GetStudentTranscriptEndpoint
{
    public static async Task<IResult> Handle(
        int studentId,
        StudentManagementDbContext db)
    {
        var student = await db.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .Select(s => new
            {
                s.Id,
                s.StudentNumber,
                s.FirstName,
                s.LastName
            })
            .SingleOrDefaultAsync();

        if (student is null)
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
                e.Status == EnrollmentStatus.Completed &&
                e.Grade != null)
            .OrderBy(e => e.CourseOffering.Semester.StartDate)
            .ThenBy(e => e.CourseOffering.Course.Code)
            .Select(e => new TranscriptCourseResponse(
                e.Id,
                e.CourseOfferingId,
                e.CourseOffering.Course.Code,
                e.CourseOffering.Course.Name,
                e.CourseOffering.Semester.Name,
                e.CourseOffering.Section,
                e.Grade!.Value,
                e.Grade.Value >= AcademicRules.PassingGrade,
                e.GradedAt!.Value
            ))
            .ToListAsync();

        var response = new StudentTranscriptResponse(
            student.Id,
            student.StudentNumber,
            student.FirstName,
            student.LastName,
            courses
        );

        return Results.Ok(response);
    }
}