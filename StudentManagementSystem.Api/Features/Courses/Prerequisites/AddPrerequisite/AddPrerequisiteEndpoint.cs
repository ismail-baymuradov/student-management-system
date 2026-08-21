using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.Prerequisites.AddPrerequisite;

public static class AddPrerequisiteEndpoint
{
    public static async Task<IResult> Handle(
        int courseId,
        int prerequisiteCourseId,
        StudentManagementDbContext db)
    {
        if (courseId <= 0 || prerequisiteCourseId <= 0)
        {
            return Results.BadRequest(new
            {
                message = "Course IDs must be greater than 0."
            });
        }

        if (courseId == prerequisiteCourseId)
        {
            return Results.BadRequest(new
            {
                message = "A course cannot require itself."
            });
        }

        var courseExists = await db.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExists)
        {
            return Results.NotFound(new
            {
                message = "Course was not found."
            });
        }

        var prerequisiteExists = await db.Courses
            .AnyAsync(c => c.Id == prerequisiteCourseId);

        if (!prerequisiteExists)
        {
            return Results.NotFound(new
            {
                message = "Prerequisite course was not found."
            });
        }

        var alreadyExists = await db.CoursePrerequisites
            .AnyAsync(cp =>
                cp.CourseId == courseId &&
                cp.PrerequisiteCourseId == prerequisiteCourseId);

        if (alreadyExists)
        {
            return Results.Conflict(new
            {
                message = "This prerequisite already exists."
            });
        }

        var reverseRelationshipExists =
            await db.CoursePrerequisites.AnyAsync(cp =>
                cp.CourseId == prerequisiteCourseId &&
                cp.PrerequisiteCourseId == courseId);

        if (reverseRelationshipExists)
        {
            return Results.Conflict(new
            {
                message =
                    "This relationship would create a direct circular prerequisite."
            });
        }

        var prerequisite = new CoursePrerequisite
        {
            CourseId = courseId,
            PrerequisiteCourseId = prerequisiteCourseId
        };

        db.CoursePrerequisites.Add(prerequisite);

        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
            when (IsUniqueConstraintViolation(ex))
        {
            return Results.Conflict(new
            {
                message = "This prerequisite already exists."
            });
        }

        var course = await db.Courses
            .AsNoTracking()
            .Where(c => c.Id == prerequisiteCourseId)
            .Select(c => new CoursePrerequisiteResponse(
                c.Id,
                c.Code,
                c.Name
            ))
            .SingleAsync();

        return Results.Created(
            $"/courses/{courseId}/prerequisites",
            course);
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException exception)
    {
        return exception.InnerException is SqlException sqlException &&
               sqlException.Number is 2601 or 2627;
    }
}