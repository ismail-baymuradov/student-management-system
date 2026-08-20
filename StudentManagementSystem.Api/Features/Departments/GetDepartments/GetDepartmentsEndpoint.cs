using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.GetDepartments;

public static class GetDepartmentsEndpoint
{
    public static void MapGetDepartmentsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (
            StudentManagementDbContext dbContext) =>
        {
            var departments = await dbContext.Departments
                .AsNoTracking()
                .OrderBy(d => d.Code)
                .Select(d => new
                {
                    d.Id,
                    d.Code,
                    d.Name
                })
                .ToListAsync();

            return Results.Ok(departments);
        });
    }
}