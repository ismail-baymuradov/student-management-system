using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.Prerequisites.GetCoursePrerequisites;

public static class GetCoursePrerequisitesEndpoint
{
    public static async Task<IResult> Handle(
        int courseId,
        StudentManagementDbContext db)
    {
        var courseExists = await db.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
        {
            return Results.NotFound(new
            {
                message = "Course was not found."
            });
        }

        var prerequisites = await db.CoursePrerequisites
            .AsNoTracking()
            .Where(cp => cp.CourseId == courseId)
            .OrderBy(cp => cp.PrerequisiteCourse.Code)
            .Select(cp => new CoursePrerequisiteResponse(
                cp.PrerequisiteCourse.Id,
                cp.PrerequisiteCourse.Code,
                cp.PrerequisiteCourse.Name
            ))
            .ToListAsync();

        return Results.Ok(prerequisites);
    }
}