using StudentManagementSystem.Api.Features.Courses.Prerequisites.AddPrerequisite;
using StudentManagementSystem.Api.Features.Courses.Prerequisites.GetCoursePrerequisites;
using StudentManagementSystem.Api.Features.Courses.Prerequisites.RemovePrerequisite;

namespace StudentManagementSystem.Api.Features.Courses.Prerequisites;

public static class CoursePrerequisiteEndpoints
{
    public static void MapCoursePrerequisiteEndpoints(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/courses/{courseId:int}/prerequisites/{prerequisiteCourseId:int}",
                AddPrerequisiteEndpoint.Handle)
            .WithTags("Course Prerequisites");

        app.MapDelete(
                "/courses/{courseId:int}/prerequisites/{prerequisiteCourseId:int}",
                RemovePrerequisiteEndpoint.Handle)
            .WithTags("Course Prerequisites");

        app.MapGet(
                "/courses/{courseId:int}/prerequisites",
                GetCoursePrerequisitesEndpoint.Handle)
            .WithTags("Course Prerequisites");
    }
}