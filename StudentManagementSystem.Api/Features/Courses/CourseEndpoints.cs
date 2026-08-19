using StudentManagementSystem.Api.Features.Courses.CreateCourse;
using StudentManagementSystem.Api.Features.Courses.DeleteCourse;
using StudentManagementSystem.Api.Features.Courses.GetCourse;
using StudentManagementSystem.Api.Features.Courses.GetCourses;
using StudentManagementSystem.Api.Features.Courses.UpdateCourse;

namespace StudentManagementSystem.Api.Features.Courses;

public static class CourseEndpoints
{
    public static void MapCourseEndpoints(this IEndpointRouteBuilder app)
    {
        var courseGroup = app.MapGroup("/courses");

        courseGroup.MapCreateCourseEndpoint();
        courseGroup.MapGetCourseEndpoint();
        courseGroup.MapGetCoursesEndpoint();
        courseGroup.MapUpdateCourseEndpoint();
        courseGroup.MapDeleteCourseEndpoint();
    }
}