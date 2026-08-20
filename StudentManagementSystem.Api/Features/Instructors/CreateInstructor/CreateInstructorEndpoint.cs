using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Instructors.CreateInstructor;

public static class CreateInstructorEndpoint
{
    public static void MapCreateInstructorEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("", async (
            CreateInstructorRequest request,
            StudentManagementDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                return Results.BadRequest(new
                {
                    message = "FirstName is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                return Results.BadRequest(new
                {
                    message = "LastName is required."
                });
            }

            if (string.IsNullOrWhiteSpace(request.EmployeeNumber))
            {
                return Results.BadRequest(new
                {
                    message = "EmployeeNumber is required."
                });
            }

            var firstName = request.FirstName.Trim();
            var lastName = request.LastName.Trim();
            var employeeNumber =
                request.EmployeeNumber.Trim().ToUpperInvariant();

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

            if (employeeNumber.Length > 20)
            {
                return Results.BadRequest(new
                {
                    message = "EmployeeNumber cannot be longer than 20 characters."
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

            var employeeNumberExists = await dbContext.Instructors
                .AnyAsync(i => i.EmployeeNumber == employeeNumber);

            if (employeeNumberExists)
            {
                return Results.Conflict(new
                {
                    message = "EmployeeNumber already exists."
                });
            }

            var instructor = new Instructor
            {
                FirstName = firstName,
                LastName = lastName,
                EmployeeNumber = employeeNumber,
                DepartmentId = request.DepartmentId
            };

            dbContext.Instructors.Add(instructor);

            await dbContext.SaveChangesAsync();

            return Results.Created(
                $"/instructors/{instructor.Id}",
                new
                {
                    instructor.Id,
                    instructor.FirstName,
                    instructor.LastName,
                    instructor.EmployeeNumber,
                    instructor.DepartmentId
                });
        });
    }

    public sealed record CreateInstructorRequest(
        string? FirstName,
        string? LastName,
        string? EmployeeNumber,
        int DepartmentId);
}