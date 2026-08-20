using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.AssignInstructor;

public static class AssignInstructorEndpoint
{
    public static void MapAssignInstructorEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}/instructor", async (
            int id,
            AssignInstructorRequest request,
            StudentManagementDbContext dbContext) =>
        {
            if (request.InstructorId <= 0)
                return Results.BadRequest(new
                {
                    message = "A valid InstructorId is required."
                });

            var offering = await dbContext.CourseOfferings
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offering is null)
                return Results.NotFound(new
                {
                    message = "Course offering not found."
                });

            var instructorExists = await dbContext.Instructors
                .AnyAsync(i => i.Id == request.InstructorId);

            if (!instructorExists)
                return Results.BadRequest(new
                {
                    message = "Instructor does not exist."
                });

            var conflict =
                await CourseOfferingValidation
                    .HasInstructorConflictAsync(
                        dbContext,
                        request.InstructorId,
                        offering.SemesterId,
                        offering.DayOfWeek,
                        offering.StartTime,
                        offering.EndTime,
                        offering.Id);

            if (conflict)
                return Results.Conflict(new
                {
                    message =
                        "Instructor already has another offering at that time."
                });

            offering.InstructorId = request.InstructorId;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                offering.Id,
                offering.InstructorId
            });
        });
    }

    public sealed record AssignInstructorRequest(
        int InstructorId);
}