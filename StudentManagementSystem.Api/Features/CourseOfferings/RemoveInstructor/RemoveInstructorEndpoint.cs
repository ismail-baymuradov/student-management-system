using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.RemoveInstructor;

public static class RemoveInstructorEndpoint
{
    public static void MapRemoveInstructorEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}/instructor", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var offering = await dbContext.CourseOfferings
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offering is null)
                return Results.NotFound(new
                {
                    message = "Course offering not found."
                });

            offering.InstructorId = null;

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}