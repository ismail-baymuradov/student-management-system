using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Students.DeleteStudent;

public static class DeleteStudentEndpoint
{
    public static void MapDeleteStudentEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var student = await dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return Results.NotFound(new
                {
                    message = "Student not found."
                });
            }

            dbContext.Students.Remove(student);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}