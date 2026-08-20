using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.GetDepartment;

public static class GetDepartmentEndpoint
{
    public static void MapGetDepartmentEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var department = await dbContext.Departments
                .AsNoTracking()
                .Where(d => d.Id == id)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name
                })
                .FirstOrDefaultAsync();

            return department is null
                ? Results.NotFound(new { message = "Department not found." })
                : Results.Ok(department);
        });
    }
}