using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Features.Students;

namespace StudentManagementSystem.Api.Features.Students.CreateStudent;

public static class CreateStudentEndpoint
{
    public static void MapCreateStudent(
        this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            CreateStudentRequest request,
            StudentManagementDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                return Results.BadRequest(
                    "First name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                return Results.BadRequest(
                    "Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.StudentNumber))
            {
                return Results.BadRequest(
                    "Student number is required.");
            }

            var studentNumberExists =
                await dbContext.Students.AnyAsync(student =>
                    student.StudentNumber == request.StudentNumber);

            if (studentNumberExists)
            {
                return Results.Conflict(
                    "Student number already exists.");
            }

            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                StudentNumber = request.StudentNumber
            };

            dbContext.Students.Add(student);

            await dbContext.SaveChangesAsync();

            return Results.Created(
                $"/students/{student.Id}",
                student);
        });
    }
}

public record CreateStudentRequest(
    string FirstName,
    string LastName,
    string StudentNumber
);