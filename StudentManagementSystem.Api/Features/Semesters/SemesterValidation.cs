namespace StudentManagementSystem.Api.Features.Semesters;

public static class SemesterValidation
{
    public static string? ValidateDates(
        DateOnly startDate,
        DateOnly endDate,
        DateOnly registrationStart,
        DateOnly registrationEnd)
    {
        if (startDate >= endDate)
        {
            return "StartDate must be before EndDate.";
        }

        if (registrationStart >= registrationEnd)
        {
            return "RegistrationStart must be before RegistrationEnd.";
        }

        if (registrationStart > startDate)
        {
            return "RegistrationStart cannot be after StartDate.";
        }

        if (registrationEnd > endDate)
        {
            return "RegistrationEnd cannot be after EndDate.";
        }

        return null;
    }
}