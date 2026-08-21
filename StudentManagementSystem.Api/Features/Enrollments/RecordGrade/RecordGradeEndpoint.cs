using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.RecordGrade;

public sealed record RecordGradeRequest(
    decimal Grade
);

public static class RecordGradeEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        RecordGradeRequest request,
        StudentManagementDbContext db)
    {
        if (!AcademicRules.IsValidGrade(request.Grade))
        {
            return Results.BadRequest(new
            {
                message = "Grade must be between 0 and 100."
            });
        }

        var enrollment = await db.Enrollments
            .SingleOrDefaultAsync(e => e.Id == id);

        if (enrollment is null)
        {
            return Results.NotFound(new
            {
                message = "Enrollment was not found."
            });
        }

        if (enrollment.Status == EnrollmentStatus.Dropped)
        {
            return Results.Conflict(new
            {
                message =
                    "A grade cannot be recorded for a dropped enrollment."
            });
        }

        if (enrollment.Grade.HasValue)
        {
            return Results.Conflict(new
            {
                message =
                    "A grade has already been recorded. Use the change-grade operation instead."
            });
        }

        var now = DateTimeOffset.UtcNow;

        enrollment.Grade = request.Grade;
        enrollment.GradedAt = now;
        enrollment.Status = EnrollmentStatus.Completed;

        await db.SaveChangesAsync();

        return Results.Ok(new GradeResponse(
            enrollment.Id,
            enrollment.StudentId,
            enrollment.CourseOfferingId,
            enrollment.Grade.Value,
            AcademicRules.IsPassingGrade(enrollment.Grade.Value),
            enrollment.Status.ToString(),
            enrollment.GradedAt.Value
        ));
    }
}