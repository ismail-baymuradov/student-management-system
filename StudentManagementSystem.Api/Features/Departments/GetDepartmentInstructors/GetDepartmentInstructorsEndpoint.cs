using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.GetDepartmentInstructors;

public static class GetDepartmentInstructorsEndpoint
{
    public static void MapGetDepartmentInstructorsEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}/instructors", async (
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

            var instructors = await dbContext.Instructors
                .AsNoTracking()
                .Where(i => i.DepartmentId == id)
                .OrderBy(i => i.EmployeeNumber)
                .Select(i => new
                {
                    i.Id,
                    i.FirstName,
                    i.LastName,
                    i.EmployeeNumber,
                    i.DepartmentId
                })
                .ToListAsync();

            return Results.Ok(instructors);
        });
    }
}