using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Enrollments.EnrollStudent;

public sealed record EnrollStudentRequest(
    int StudentId,
    int CourseOfferingId
);

public static class EnrollStudentEndpoint
{
    public static async Task<IResult> Handle(
        EnrollStudentRequest request,
        StudentManagementDbContext db)
    {
        if (request.StudentId <= 0 || request.CourseOfferingId <= 0)
        {
            return Results.BadRequest(new
            {
                message = "StudentId and CourseOfferingId must be greater than 0."
            });
        }

        try
        {
            await using var transaction =
                await db.Database.BeginTransactionAsync(
                    IsolationLevel.Serializable);

            var studentExists = await db.Students
                .AnyAsync(s => s.Id == request.StudentId);

            if (!studentExists)
            {
                return Results.NotFound(new
                {
                    message = "Student was not found."
                });
            }

            var offering = await db.CourseOfferings
                .Include(o => o.Semester)
                .SingleOrDefaultAsync(
                    o => o.Id == request.CourseOfferingId);

            if (offering is null)
            {
                return Results.NotFound(new
                {
                    message = "Course offering was not found."
                });
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (today < offering.Semester.RegistrationStart ||
                today > offering.Semester.RegistrationEnd)
            {
                return Results.Conflict(new
                {
                    message = "Registration is closed for this semester."
                });
            }

            var existingEnrollment = await db.Enrollments
                .SingleOrDefaultAsync(e =>
                    e.StudentId == request.StudentId &&
                    e.CourseOfferingId == request.CourseOfferingId);

            if (existingEnrollment is not null &&
                existingEnrollment.Status == EnrollmentStatus.Active)
            {
                return Results.Conflict(new
                {
                    message = "Student is already enrolled in this course offering."
                });
            }

            var currentEnrollmentCount = await db.Enrollments
                .CountAsync(e =>
                    e.CourseOfferingId == request.CourseOfferingId &&
                    e.Status == EnrollmentStatus.Active);

            if (currentEnrollmentCount >= offering.Capacity)
            {
                return Results.Conflict(new
                {
                    message = "Course offering has reached its capacity."
                });
            }

            var now = DateTimeOffset.UtcNow;

            Enrollment enrollment;
            var isNewEnrollment = existingEnrollment is null;

            if (isNewEnrollment)
            {
                enrollment = new Enrollment
                {
                    StudentId = request.StudentId,
                    CourseOfferingId = request.CourseOfferingId,
                    Status = EnrollmentStatus.Active,
                    EnrolledAt = now,
                    DroppedAt = null
                };

                db.Enrollments.Add(enrollment);
            }
            else
            {
                enrollment = existingEnrollment!;

                enrollment.Status = EnrollmentStatus.Active;
                enrollment.EnrolledAt = now;
                enrollment.DroppedAt = null;
            }

            await db.SaveChangesAsync();

            await transaction.CommitAsync();

            var response = new EnrollmentResponse(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.CourseOfferingId,
                enrollment.Status.ToString(),
                enrollment.EnrolledAt,
                enrollment.DroppedAt
            );

            if (isNewEnrollment)
            {
                return Results.Created(
                    $"/enrollments/{enrollment.Id}",
                    response);
            }

            return Results.Ok(response);
        }
        catch (DbUpdateException ex)
            when (IsUniqueConstraintViolation(ex))
        {
            return Results.Conflict(new
            {
                message = "Student is already enrolled in this course offering."
            });
        }
        catch (DbUpdateException ex)
            when (IsDeadlock(ex))
        {
            return Results.Conflict(new
            {
                message = "The enrollment changed at the same time. Please retry."
            });
        }
        catch (SqlException ex)
            when (ex.Number == 1205)
        {
            return Results.Conflict(new
            {
                message = "The enrollment changed at the same time. Please retry."
            });
        }
    }

    private static bool IsUniqueConstraintViolation(
        DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException &&
               sqlException.Number is 2601 or 2627;
    }

    private static bool IsDeadlock(
        DbUpdateException exception)
    {
        return exception.InnerException is SqlException
        {
            Number: 1205
        };
    }
}