using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;

namespace StudentManagementSystem.Api.Features.CourseOfferings;

public static class CourseOfferingValidation
{
    public static string? Validate(
        int courseId,
        int semesterId,
        int section,
        int capacity,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime)
    {
        if (courseId <= 0)
            return "A valid CourseId is required.";

        if (semesterId <= 0)
            return "A valid SemesterId is required.";

        if (section <= 0)
            return "Section must be greater than 0.";

        if (capacity <= 0)
            return "Capacity must be greater than 0.";

        if (!Enum.IsDefined(typeof(DayOfWeek), dayOfWeek))
            return "DayOfWeek is invalid.";

        if (startTime >= endTime)
            return "StartTime must be before EndTime.";

        return null;
    }

    public static async Task<bool> HasInstructorConflictAsync(
        StudentManagementDbContext dbContext,
        int instructorId,
        int semesterId,
        DayOfWeek dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        int? excludeOfferingId = null)
    {
        var query = dbContext.CourseOfferings
            .Where(o =>
                o.InstructorId == instructorId &&
                o.SemesterId == semesterId &&
                o.DayOfWeek == dayOfWeek &&
                o.StartTime < endTime &&
                startTime < o.EndTime);

        if (excludeOfferingId.HasValue)
        {
            query = query.Where(
                o => o.Id != excludeOfferingId.Value);
        }

        return await query.AnyAsync();
    }
}