using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Semesters.GetSemesterOfferings;

public static class GetSemesterOfferingsEndpoint
{
    public static void MapGetSemesterOfferingsEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}/course-offerings", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var semesterExists = await dbContext.Semesters
                .AnyAsync(s => s.Id == id);

            if (!semesterExists)
                return Results.NotFound(new
                {
                    message = "Semester not found."
                });

            var offerings = await dbContext.CourseOfferings
                .AsNoTracking()
                .Where(o => o.SemesterId == id)
                .OrderBy(o => o.Course.Code)
                .ThenBy(o => o.Section)
                .Select(o => new
                {
                    o.Id,
                    o.CourseId,
                    CourseCode = o.Course.Code,
                    CourseName = o.Course.Name,
                    o.Section,
                    o.Capacity,
                    o.DayOfWeek,
                    o.StartTime,
                    o.EndTime,
                    o.InstructorId,

                    InstructorName = o.Instructor == null
                        ? null
                        : o.Instructor.FirstName + " " +
                          o.Instructor.LastName
                })
                .ToListAsync();

            return Results.Ok(offerings);
        });
    }
}