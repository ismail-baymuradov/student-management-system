using StudentManagementSystem.Api.Features.Enrollments.DropCourse;
using StudentManagementSystem.Api.Features.Enrollments.EnrollStudent;
using StudentManagementSystem.Api.Features.Enrollments.GetCourseStudents;
using StudentManagementSystem.Api.Features.Enrollments.GetStudentCourses;
using StudentManagementSystem.Api.Features.Enrollments.ChangeGrade;
using StudentManagementSystem.Api.Features.Enrollments.GetStudentTranscript;
using StudentManagementSystem.Api.Features.Enrollments.RecordGrade;

namespace StudentManagementSystem.Api.Features.Enrollments;

public static class EnrollmentEndpoints
{
    public static void MapEnrollmentEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app
            .MapGroup("/enrollments")
            .WithTags("Enrollments");

        group.MapPost(
            "",
            EnrollStudentEndpoint.Handle);

        group.MapPut(
            "/{id:int}/drop",
            DropCourseEndpoint.Handle);

        app.MapGet(
                "/students/{studentId:int}/courses",
                GetStudentCoursesEndpoint.Handle)
            .WithTags("Enrollments");

        app.MapGet(
                "/course-offerings/{courseOfferingId:int}/students",
                GetCourseStudentsEndpoint.Handle)
            .WithTags("Enrollments");

group.MapPost(
"/{id:int}/grade",
RecordGradeEndpoint.Handle);

        group.MapPut(
            "/{id:int}/grade",
            ChangeGradeEndpoint.Handle);

        app.MapGet(
                "/students/{studentId:int}/transcript",
                GetStudentTranscriptEndpoint.Handle)
            .WithTags("Enrollments");
    }
}