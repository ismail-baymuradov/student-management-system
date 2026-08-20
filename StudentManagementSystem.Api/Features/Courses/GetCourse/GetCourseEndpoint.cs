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
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Name,
                    c.Credits,
                    c.DepartmentId
                })
                .FirstOrDefaultAsync();

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