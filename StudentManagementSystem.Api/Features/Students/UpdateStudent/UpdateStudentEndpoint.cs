using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Students.UpdateStudent;

public static class UpdateStudentEndpoint
{
    public static void MapUpdateStudentEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (
            int id,
            UpdateStudentRequest request,
            StudentManagementDbContext dbContext) =>
        {
            // Validate FirstName
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                return Results.BadRequest(new
                {
                    message = "FirstName is required."
                });
            }

            // Validate LastName
            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                return Results.BadRequest(new
                {
                    message = "LastName is required."
                });
            }

            // Validate StudentNumber
            if (string.IsNullOrWhiteSpace(request.StudentNumber))
            {
                return Results.BadRequest(new
                {
                    message = "StudentNumber is required."
                });
            }

            var firstName = request.FirstName.Trim();
            var lastName = request.LastName.Trim();
            var studentNumber = request.StudentNumber.Trim();

            // Length validation
            if (firstName.Length > 100)
            {
                return Results.BadRequest(new
                {
                    message = "FirstName cannot be longer than 100 characters."
                });
            }

            if (lastName.Length > 100)
            {
                return Results.BadRequest(new
                {
                    message = "LastName cannot be longer than 100 characters."
                });
            }

            if (studentNumber.Length > 20)
            {
                return Results.BadRequest(new
                {
                    message = "StudentNumber cannot be longer than 20 characters."
                });
            }

            // Find the Student
            var student = await dbContext.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student is null)
            {
                return Results.NotFound(new
                {
                    message = "Student not found."
                });
            }

            // Check StudentNumber uniqueness.
            // Exclude the Student currently being updated.
            var studentNumberExists = await dbContext.Students
                .AnyAsync(s =>
                    s.StudentNumber == studentNumber &&
                    s.Id != id);

            if (studentNumberExists)
            {
                return Results.Conflict(new
                {
                    message = "StudentNumber already exists."
                });
            }

            // Update the tracked entity
            student.FirstName = firstName;
            student.LastName = lastName;
            student.StudentNumber = studentNumber;

            await dbContext.SaveChangesAsync();

            return Results.Ok(student);
        });
    }

    public sealed record UpdateStudentRequest(
        string? FirstName,
        string? LastName,
        string? StudentNumber);
}