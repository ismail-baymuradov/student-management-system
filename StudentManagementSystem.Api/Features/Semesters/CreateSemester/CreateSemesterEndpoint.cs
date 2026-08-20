using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.Semesters.CreateSemester;

public static class CreateSemesterEndpoint
{
    public static void MapCreateSemesterEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("", async (
            CreateSemesterRequest request,
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

            var semester = new Semester
            {
                Name = name,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                RegistrationStart = request.RegistrationStart,
                RegistrationEnd = request.RegistrationEnd
            };

            dbContext.Semesters.Add(semester);

            await dbContext.SaveChangesAsync();

            return Results.Created(
                $"/semesters/{semester.Id}",
                semester);
        });
    }

    public sealed record CreateSemesterRequest(
        string? Name,
        DateOnly StartDate,
        DateOnly EndDate,
        DateOnly RegistrationStart,
        DateOnly RegistrationEnd);
}