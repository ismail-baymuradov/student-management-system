using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.CreateCourseOffering;

public static class CreateCourseOfferingEndpoint
{
    public static void MapCreateCourseOfferingEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPost("", async (
            CreateCourseOfferingRequest request,
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
                    o.Section == request.Section);

            if (duplicate)
                return Results.Conflict(new
                {
                    message =
                        "This Course already has that section in this Semester."
                });

            var offering = new CourseOffering
            {
                CourseId = request.CourseId,
                SemesterId = request.SemesterId,
                Section = request.Section,
                Capacity = request.Capacity,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            dbContext.CourseOfferings.Add(offering);

            await dbContext.SaveChangesAsync();

            return Results.Created(
                $"/course-offerings/{offering.Id}",
                new
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

    public sealed record CreateCourseOfferingRequest(
        int CourseId,
        int SemesterId,
        int Section,
        int Capacity,
        DayOfWeek DayOfWeek,
        TimeOnly StartTime,
        TimeOnly EndTime);
}