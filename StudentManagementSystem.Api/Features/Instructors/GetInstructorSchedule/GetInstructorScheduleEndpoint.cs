using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Instructors.GetInstructorSchedule;

public static class GetInstructorScheduleEndpoint
{
    public static void MapGetInstructorScheduleEndpoint(
        this RouteGroupBuilder group)
    {
        group.MapGet("/{id:int}/schedule", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var instructorExists = await dbContext.Instructors
                .AnyAsync(i => i.Id == id);

            if (!instructorExists)
                return Results.NotFound(new
                {
                    message = "Instructor not found."
                });

            var schedule = await dbContext.CourseOfferings
                .AsNoTracking()
                .Where(o => o.InstructorId == id)
                .OrderBy(o => o.Semester.StartDate)
                .ThenBy(o => o.DayOfWeek)
                .ThenBy(o => o.StartTime)
                .Select(o => new
                {
                    o.Id,

                    o.CourseId,
                    CourseCode = o.Course.Code,
                    CourseName = o.Course.Name,

                    o.SemesterId,
                    SemesterName = o.Semester.Name,

                    o.Section,
                    o.DayOfWeek,
                    o.StartTime,
                    o.EndTime
                })
                .ToListAsync();

            return Results.Ok(schedule);
        });
    }
}