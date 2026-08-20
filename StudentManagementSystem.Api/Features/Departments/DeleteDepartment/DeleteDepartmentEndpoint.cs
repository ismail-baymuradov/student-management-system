using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Departments.DeleteDepartment;

public static class DeleteDepartmentEndpoint
{
    public static void MapDeleteDepartmentEndpoint(this RouteGroupBuilder group)
    {
        group.MapDelete("/{id:int}", async (
            int id,
            StudentManagementDbContext dbContext) =>
        {
            var department = await dbContext.Departments
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department is null)
            {
                return Results.NotFound(new
                {
                    message = "Department not found."
                });
            }

            var hasCourses = await dbContext.Courses
                .AnyAsync(c => c.DepartmentId == id);

            if (hasCourses)
            {
                return Results.Conflict(new
                {
                    message =
                        "Department cannot be deleted because it contains courses."
                });
            }

            var hasInstructors = await dbContext.Instructors
                .AnyAsync(i => i.DepartmentId == id);

            if (hasInstructors)
            {
                return Results.Conflict(new
                {
                    message =
                        "Department cannot be deleted because it contains instructors."
                });
            }

            dbContext.Departments.Remove(department);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Results.Conflict(new
                {
                    message =
                        "Department cannot be deleted because it is in use."
                });
            }

            return Results.NoContent();
        });
    }
}