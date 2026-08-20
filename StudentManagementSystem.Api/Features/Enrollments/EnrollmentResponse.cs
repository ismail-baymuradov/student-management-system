namespace StudentManagementSystem.Api.Features.Enrollments;

public sealed record EnrollmentResponse(
    int Id,
    int StudentId,
    int CourseOfferingId,
    string Status,
    DateTimeOffset EnrolledAt,
    DateTimeOffset? DroppedAt
);