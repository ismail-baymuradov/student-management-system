using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.Prerequisites.RemovePrerequisite;

public static class RemovePrerequisiteEndpoint
{
    public static async Task<IResult> Handle(
        int courseId,
        int prerequisiteCourseId,
        StudentManagementDbContext db)
    {
        var prerequisite = await db.CoursePrerequisites
            .SingleOrDefaultAsync(cp =>
                cp.CourseId == courseId &&
                cp.PrerequisiteCourseId == prerequisiteCourseId);

        if (prerequisite is null)
        {
            return Results.NotFound(new
            {
                message = "Prerequisite relationship was not found."
            });
        }

        db.CoursePrerequisites.Remove(prerequisite);

        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}