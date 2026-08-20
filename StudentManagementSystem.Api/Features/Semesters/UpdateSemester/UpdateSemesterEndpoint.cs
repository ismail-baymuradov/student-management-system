using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Semesters.UpdateSemester;

public static class UpdateSemesterEndpoint
{
    public static void MapUpdateSemesterEndpoint(this RouteGroupBuilder group)
    {
        group.MapPut("/{id:int}", async (
            int id,
            UpdateSemesterRequest request,
            StudentManagementDbContext dbContext) =>
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest(new
                {
                    message = "Name is required."
                });
            }

            var name = request.Name.Trim();

            if (name.Length > 100)
            {
                return Results.BadRequest(new
                {
                    message = "Name cannot be longer than 100 characters."
                });
            }

            var dateError = SemesterValidation.ValidateDates(
                request.StartDate,
                request.EndDate,
                request.RegistrationStart,
                request.RegistrationEnd);

            if (dateError is not null)
            {
                return Results.BadRequest(new
                {
                    message = dateError
                });
            }

            var semester = await dbContext.Semesters
                .FirstOrDefaultAsync(s => s.Id == id);

            if (semester is null)
            {
                return Results.NotFound(new
                {
                    message = "Semester not found."
                });
            }

            semester.Name = name;
            semester.StartDate = request.StartDate;
            semester.EndDate = request.EndDate;
            semester.RegistrationStart = request.RegistrationStart;
            semester.RegistrationEnd = request.RegistrationEnd;

            await dbContext.SaveChangesAsync();

            return Results.Ok(semester);
        });
    }

    public sealed record UpdateSemesterRequest(
        string? Name,
        DateOnly StartDate,
        DateOnly EndDate,
        DateOnly RegistrationStart,
        DateOnly RegistrationEnd);
}