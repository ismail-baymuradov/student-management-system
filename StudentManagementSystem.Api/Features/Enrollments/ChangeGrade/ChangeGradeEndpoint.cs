using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.ChangeGrade;

public sealed record ChangeGradeRequest(
    decimal Grade
);

public static class ChangeGradeEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        ChangeGradeRequest request,
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

        if (!enrollment.Grade.HasValue)
        {
            return Results.Conflict(new
            {
                message =
                    "No grade has been recorded yet. Use the record-grade operation first."
            });
        }

        if (enrollment.Status == EnrollmentStatus.Dropped)
        {
            return Results.Conflict(new
            {
                message =
                    "A dropped enrollment cannot have its grade changed."
            });
        }

        enrollment.Grade = request.Grade;
        enrollment.GradedAt = DateTimeOffset.UtcNow;
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