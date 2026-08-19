using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Courses.UpdateCourse;

public static class UpdateCourseEndpoint
{
    public static void MapUpdateCourseEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (
            int id,
            UpdateCourseRequest request,
            StudentManagementDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Code))
            {
                return Results.BadRequest(new
                {
                    message = "Code is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest(new
                {
                    message = "Name is required."
                });
            }

            var code = request.Code.Trim().ToUpperInvariant();
            var name = request.Name.Trim();

            if (code.Length > 20)
            {
                return Results.BadRequest(new
                {
                    message = "Code cannot be longer than 20 characters."
                });
            }

            if (name.Length > 200)
            {
                return Results.BadRequest(new
                {
                    message = "Name cannot be longer than 200 characters."
                });
            }

            if (request.Credits < 1 || request.Credits > 30)
            {
                return Results.BadRequest(new
                {
                    message = "Credits must be between 1 and 30."
                });
            }

            var course = await dbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course is null)
            {
                return Results.NotFound(new
                {
                    message = "Course not found."
                });
            }

            var codeExists = await dbContext.Courses
                .AnyAsync(c =>
                    c.Code == code &&
                    c.Id != id);

            if (codeExists)
            {
                return Results.Conflict(new
                {
                    message = "Course code already exists."
                });
            }

            course.Code = code;
            course.Name = name;
            course.Credits = request.Credits;

            await dbContext.SaveChangesAsync();

            return Results.Ok(course);
        });
    }

    public sealed record UpdateCourseRequest(
        string? Code,
        string? Name,
        int Credits);
}