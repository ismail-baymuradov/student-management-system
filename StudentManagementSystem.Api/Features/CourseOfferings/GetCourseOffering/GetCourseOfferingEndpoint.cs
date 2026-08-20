using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings.GetCourseOffering;

public static class GetCourseOfferingEndpoint
{
    public static void MapGetCourseOfferingEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var offering = await dbContext.CourseOfferings
                .AsNoTracking()
                .Where(o => o.Id == id)
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
                .FirstOrDefaultAsync();

            if (offering is null)
                return Results.NotFound(new
                {
                    message = "Course offering not found."
                });

            return Results.Ok(offering);
        });
    }
}