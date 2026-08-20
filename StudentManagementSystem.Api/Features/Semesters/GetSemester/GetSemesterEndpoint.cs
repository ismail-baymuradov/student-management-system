using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Semesters.GetSemester;

public static class GetSemesterEndpoint
{
    public static void MapGetSemesterEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var semester = await dbContext.Semesters
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (semester is null)
            {
                return Results.NotFound(new
                {
                    message = "Semester not found."
                });
            }

            return Results.Ok(semester);
        });
    }
}