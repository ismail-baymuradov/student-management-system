using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Instructors.UpdateInstructor;

public static class UpdateInstructorEndpoint
{
    public static void MapUpdateInstructorEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (
            int id,
            UpdateInstructorRequest request,
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

            var instructor = await dbContext.Instructors
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor is null)
            {
                return Results.NotFound(new
                {
                    message = "Instructor not found."
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
                .AnyAsync(i =>
                    i.EmployeeNumber == employeeNumber &&
                    i.Id != id);

            if (employeeNumberExists)
            {
                return Results.Conflict(new
                {
                    message = "EmployeeNumber already exists."
                });
            }

            instructor.FirstName = firstName;
            instructor.LastName = lastName;
            instructor.EmployeeNumber = employeeNumber;
            instructor.DepartmentId = request.DepartmentId;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                instructor.Id,
                instructor.FirstName,
                instructor.LastName,
                instructor.EmployeeNumber,
                instructor.DepartmentId
            });
        });
    }

    public sealed record UpdateInstructorRequest(
        string? FirstName,
        string? LastName,
        string? EmployeeNumber,
        int DepartmentId);
}