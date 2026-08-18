using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Students.GetStudent;

public static class GetStudentEndpoint
{
    public static void MapGetStudent(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var student = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (student is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(student);
        });
    }
}