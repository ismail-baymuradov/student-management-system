using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Students.GetStudents;

public static class GetStudentsEndpoint
{
    public static void MapGetStudents(
        this RouteGroupBuilder group)
    {
        group.MapGet("/", async (
            StudentManagementDbContext dbContext) =>
        {
            var students = await dbContext.Students.AsNoTracking().ToListAsync();

            return Results.Ok(students);
        });
    }
}