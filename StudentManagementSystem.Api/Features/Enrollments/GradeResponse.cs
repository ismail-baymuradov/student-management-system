namespace StudentManagementSystem.Api.Features.Enrollments;

public sealed record GradeResponse(
    int EnrollmentId,
    int StudentId,
    int CourseOfferingId,
    decimal Grade,
    bool Passed,
    string Status,
    DateTimeOffset GradedAt
);