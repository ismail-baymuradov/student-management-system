using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.GetDepartmentCourses;

public static class GetDepartmentCoursesEndpoint
{
    public static void MapGetDepartmentCoursesEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}/courses", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var departmentExists = await dbContext.Departments
                .AnyAsync(d => d.Id == id);

            if (!departmentExists)
            {
                return Results.NotFound(new
                {
                    message = "Department not found."
                });
            }

            var courses = await dbContext.Courses
                .AsNoTracking()
                .Where(c => c.DepartmentId == id)
                .OrderBy(c => c.Code)
                .Select(c => new
                {
                    c.Id,
                    c.Code,
                    c.Name,
                    c.Credits,
                    c.DepartmentId
                })
                .ToListAsync();

            return Results.Ok(courses);
        });
    }
}