using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.UpdateCourseOffering;

public static class UpdateCourseOfferingEndpoint
{
    public static void MapUpdateCourseOfferingEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (
            int id,
            UpdateCourseOfferingRequest request,
            StudentManagementDbContext dbContext) =>
        {
            var error = CourseOfferingValidation.Validate(
                request.CourseId,
                request.SemesterId,
                request.Section,
                request.Capacity,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);

            if (error is not null)
                return Results.BadRequest(new { message = error });

            var offering = await dbContext.CourseOfferings
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offering is null)
                return Results.NotFound(new
                {
                    message = "Course offering not found."
                });

            var courseExists = await dbContext.Courses
                .AnyAsync(c => c.Id == request.CourseId);

            if (!courseExists)
                return Results.BadRequest(new
                {
                    message = "Course does not exist."
                });

            var semesterExists = await dbContext.Semesters
                .AnyAsync(s => s.Id == request.SemesterId);

            if (!semesterExists)
                return Results.BadRequest(new
                {
                    message = "Semester does not exist."
                });

            var duplicate = await dbContext.CourseOfferings
                .AnyAsync(o =>
                    o.CourseId == request.CourseId &&
                    o.SemesterId == request.SemesterId &&
                    o.Section == request.Section &&
                    o.Id != id);

            if (duplicate)
                return Results.Conflict(new
                {
                    message =
                        "This Course already has that section in this Semester."
                });

            if (offering.InstructorId.HasValue)
            {
                var scheduleConflict =
                    await CourseOfferingValidation
                        .HasInstructorConflictAsync(
                            dbContext,
                            offering.InstructorId.Value,
                            request.SemesterId,
                            request.DayOfWeek,
                            request.StartTime,
                            request.EndTime,
                            id);

                if (scheduleConflict)
                    return Results.Conflict(new
                    {
                        message =
                            "The assigned Instructor has another offering at that time."
                    });
            }

            offering.CourseId = request.CourseId;
            offering.SemesterId = request.SemesterId;
            offering.Section = request.Section;
            offering.Capacity = request.Capacity;
            offering.DayOfWeek = request.DayOfWeek;
            offering.StartTime = request.StartTime;
            offering.EndTime = request.EndTime;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                offering.Id,
                offering.CourseId,
                offering.SemesterId,
                offering.Section,
                offering.Capacity,
                offering.DayOfWeek,
                offering.StartTime,
                offering.EndTime,
                offering.InstructorId
            });
        });
    }

    public sealed record UpdateCourseOfferingRequest(
        int CourseId,
        int SemesterId,
        int Section,
        int Capacity,
        DayOfWeek DayOfWeek,
        TimeOnly StartTime,
        TimeOnly EndTime);
}