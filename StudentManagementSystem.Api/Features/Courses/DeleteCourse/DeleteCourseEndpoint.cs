using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.DeleteCourse;

public static class DeleteCourseEndpoint
{
    public static void MapDeleteCourseEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var course = await dbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
            {
                return Results.NotFound(new
                {
                    message = "Course not found."
                });
            }

            dbContext.Courses.Remove(course);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}