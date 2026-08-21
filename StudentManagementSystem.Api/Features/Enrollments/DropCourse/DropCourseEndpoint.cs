using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.DropCourse;

public static class DropCourseEndpoint
{
    public static async Task<IResult> Handle(
        int id,
        StudentManagementDbContext db)
    {
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
                message = "This enrollment has already been dropped."
            });
        }

        if (enrollment.Status == EnrollmentStatus.Completed)
        {
            return Results.Conflict(new
            {
                message = "A completed enrollment cannot be dropped."
            });
        }

        enrollment.Status = EnrollmentStatus.Dropped;
        enrollment.DroppedAt = DateTimeOffset.UtcNow;

        await db.SaveChangesAsync();

        return Results.Ok(
            new EnrollmentResponse(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.CourseOfferingId,
                enrollment.Status.ToString(),
                enrollment.EnrolledAt,
                enrollment.DroppedAt
            ));
    }
}