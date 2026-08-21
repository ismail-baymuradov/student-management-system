using StudentManagementSystem.Api.Features.CourseOfferings;
using StudentManagementSystem.Api.Features.Students;

namespace StudentManagementSystem.Api.Features.Enrollments;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public Student Student { get; set; } = null!;

    public int CourseOfferingId { get; set; }

    public CourseOffering CourseOffering { get; set; } = null!;

    public EnrollmentStatus Status { get; set; }
        = EnrollmentStatus.Active;

    public DateTimeOffset EnrolledAt { get; set; }

    public DateTimeOffset? DroppedAt { get; set; }

    public decimal? Grade { get; set; }

    public DateTimeOffset? GradedAt { get; set; }
}