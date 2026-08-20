using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Instructors.GetInstructors;

public static class GetInstructorsEndpoint
{
    public static void MapGetInstructorsEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", async (
            StudentManagementDbContext dbContext) =>
        {
            var instructors = await dbContext.Instructors
                .AsNoTracking()
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