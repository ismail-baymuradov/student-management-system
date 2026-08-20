using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.UpdateDepartment;

public static class UpdateDepartmentEndpoint
{
    public static void MapUpdateDepartmentEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (
            int id,
            UpdateDepartmentRequest request,
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

            var department = await dbContext.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department is null)
                return Results.NotFound(new
                {
                    message = "Department not found."
                });

            var codeExists = await dbContext.Departments
                .AnyAsync(d => d.Code == code && d.Id != id);

            if (codeExists)
                return Results.Conflict(new
                {
                    message = "Department code already exists."
                });

            department.Code = code;
            department.Name = name;

            await dbContext.SaveChangesAsync();

            return Results.Ok(new
            {
                department.Id,
                department.Code,
                department.Name
            });
        });
    }

    public sealed record UpdateDepartmentRequest(
        string? Code,
        string? Name);
}