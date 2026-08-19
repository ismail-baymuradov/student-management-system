using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.GetCourse;

public static class GetCourseEndpoint
{
    public static void MapGetCourseEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var course = await dbContext.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
            {
                return Results.NotFound(new
                {
                    message = "Course not found."
                });
            }

            return Results.Ok(course);
        });
    }
}