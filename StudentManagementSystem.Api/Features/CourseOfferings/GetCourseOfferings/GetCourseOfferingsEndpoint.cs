using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.GetCourseOfferings;

public static class GetCourseOfferingsEndpoint
{
    public static void MapGetCourseOfferingsEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("", async (
            StudentManagementDbContext dbContext) =>
        {
            var offerings = await dbContext.CourseOfferings
                .AsNoTracking()
                .OrderBy(o => o.Semester.StartDate)
                .ThenBy(o => o.Course.Code)
                .ThenBy(o => o.Section)
                .Select(o => new
                {
                    o.Id,
                    o.CourseId,
                    CourseCode = o.Course.Code,
                    CourseName = o.Course.Name,

                    o.SemesterId,
                    SemesterName = o.Semester.Name,

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