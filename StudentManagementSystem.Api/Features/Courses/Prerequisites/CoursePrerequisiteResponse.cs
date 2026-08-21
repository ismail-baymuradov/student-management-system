namespace StudentManagementSystem.Api.Features.Courses.Prerequisites;

public sealed record CoursePrerequisiteResponse(
    int Id,
    string Code,
    string Name
);