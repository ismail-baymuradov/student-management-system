using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Instructors.GetInstructor;

public static class GetInstructorEndpoint
{
    public static void MapGetInstructorEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var instructor = await dbContext.Instructors
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => new
                {
                    i.Id,
                    i.FirstName,
                    i.LastName,
                    i.EmployeeNumber,
                    i.DepartmentId
                })
                .FirstOrDefaultAsync();

            if (instructor is null)
            {
                return Results.NotFound(new
                {
                    message = "Instructor not found."
                });
            }

            return Results.Ok(instructor);
        });
    }
}