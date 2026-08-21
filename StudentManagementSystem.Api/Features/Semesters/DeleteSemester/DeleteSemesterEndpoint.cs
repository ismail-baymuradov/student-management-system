using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Semesters.DeleteSemester;

public static class DeleteSemesterEndpoint
{
    public static void MapDeleteSemesterEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var semester = await dbContext.Semesters
                .FirstOrDefaultAsync(s => s.Id == id);

            if (semester is null)
            {
                return Results.NotFound(new
                {
                    message = "Semester not found."
                });
            }
            var hasOfferings = await dbContext.CourseOfferings
        .AnyAsync(o => o.SemesterId == id);

            if (hasOfferings)
            {
                return Results.Conflict(new
                {
                    message =
                        "Semester cannot be deleted because it has course offerings."
                });
            }
            dbContext.Semesters.Remove(semester);

            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}