using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.CreateDepartment;

public static class CreateDepartmentEndpoint
{
    public static void MapCreateDepartmentEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            CreateDepartmentRequest request,
            StudentManagementDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                return Results.BadRequest(new { message = "Code is required." });

            if (string.IsNullOrWhiteSpace(request.Name))
                return Results.BadRequest(new { message = "Name is required." });

            var code = request.Code.Trim().ToUpperInvariant();
            var name = request.Name.Trim();

            if (code.Length > 20)
                return Results.BadRequest(new
                {
                    message = "Code cannot be longer than 20 characters."
                });

            if (name.Length > 200)
                return Results.BadRequest(new
                {
                    message = "Name cannot be longer than 200 characters."
                });

            var codeExists = await dbContext.Departments
                .AnyAsync(d => d.Code == code);

            if (codeExists)
                return Results.Conflict(new
                {
                    message = "Department code already exists."
                });

            var department = new Department
            {
                Code = code,
                Name = name
            };

            dbContext.Departments.Add(department);

            await dbContext.SaveChangesAsync();

            return Results.Created(
                $"/departments/{department.Id}",
                new
                {
                    department.Id,
                    department.Code,
                    department.Name
                });
        });
    }

    public sealed record CreateDepartmentRequest(
        string? Code,
        string? Name);
}