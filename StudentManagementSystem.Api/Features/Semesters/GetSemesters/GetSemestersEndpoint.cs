using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Semesters.GetSemesters;

public static class GetSemestersEndpoint
{
    public static void MapGetSemestersEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("", async (
            StudentManagementDbContext dbContext) =>
        {
            var semesters = await dbContext.Semesters
                .AsNoTracking()
                .OrderBy(s => s.StartDate)
                .ToListAsync();

            return Results.Ok(semesters);
        });
    }
}