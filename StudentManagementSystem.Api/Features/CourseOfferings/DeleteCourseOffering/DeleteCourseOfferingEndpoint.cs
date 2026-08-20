using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.DeleteCourseOffering;

public static class DeleteCourseOfferingEndpoint
{
    public static void MapDeleteCourseOfferingEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (
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

            dbContext.CourseOfferings.Remove(offering);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}