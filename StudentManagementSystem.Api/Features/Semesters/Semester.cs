namespace StudentManagementSystem.Api.Features.Semesters;

public class Semester
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly RegistrationStart { get; set; }

    public DateOnly RegistrationEnd { get; set; }
}