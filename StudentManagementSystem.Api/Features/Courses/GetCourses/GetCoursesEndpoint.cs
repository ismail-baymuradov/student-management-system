using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.GetCourses;

public static class GetCoursesEndpoint
{
    public static void MapGetCoursesEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (
            StudentManagementDbContext dbContext) =>
        {
            var courses = await dbContext.Courses
                .AsNoTracking()
                .OrderBy(c => c.Code)
                .ToListAsync();

            return Results.Ok(courses);
        });
    }
}