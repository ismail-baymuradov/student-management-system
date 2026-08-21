namespace StudentManagementSystem.Api.Features.Enrollments;

public static class AcademicRules
{
    public const decimal MinimumGrade = 0m;
    public const decimal MaximumGrade = 100m;
    public const decimal PassingGrade = 60m;

    public static bool IsValidGrade(decimal grade)
    {
        return grade >= MinimumGrade &&
               grade <= MaximumGrade;
    }

    public static bool IsPassingGrade(decimal grade)
    {
        return grade >= PassingGrade;
    }
}