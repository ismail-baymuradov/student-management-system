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

            if (request.DepartmentId <= 0)
            {
                return Results.BadRequest(new
                {
                    message = "A valid DepartmentId is required."
                });
            }

            var departmentExists = await dbContext.Departments
                .AnyAsync(d => d.Id == request.DepartmentId);

            if (!departmentExists)
            {
                return Results.BadRequest(new
                {
                    message = "Department does not exist."
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
            course.DepartmentId = request.DepartmentId;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                course.Id,
                course.Code,
                course.Name,
                course.Credits,
                course.DepartmentId
            });
        });
    }

    public sealed record UpdateCourseRequest(
        string? Code,
        string? Name,
        int Credits,
        int DepartmentId);
}