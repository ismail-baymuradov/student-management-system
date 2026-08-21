using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Instructors.DeleteInstructor;

public static class DeleteInstructorEndpoint
{
    public static void MapDeleteInstructorEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var instructor = await dbContext.Instructors
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor is null)
            {
                return Results.NotFound(new
                {
                    message = "Instructor not found."
                });
            }
            var hasOfferings = await dbContext.CourseOfferings
                .AnyAsync(o => o.InstructorId == id);

            if (hasOfferings)
            {
                return Results.Conflict(new
                {
                    message =
                        "Instructor cannot be deleted because they are assigned to course offerings."
                });
            }
            dbContext.Instructors.Remove(instructor);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}