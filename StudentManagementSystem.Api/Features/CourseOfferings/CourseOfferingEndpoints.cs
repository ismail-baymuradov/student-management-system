using StudentManagementSystem.Api.Features.CourseOfferings.AssignInstructor;
using StudentManagementSystem.Api.Features.CourseOfferings.CreateCourseOffering;
using StudentManagementSystem.Api.Features.CourseOfferings.DeleteCourseOffering;
using StudentManagementSystem.Api.Features.CourseOfferings.GetCourseOffering;
using StudentManagementSystem.Api.Features.CourseOfferings.GetCourseOfferings;
using StudentManagementSystem.Api.Features.CourseOfferings.RemoveInstructor;
using StudentManagementSystem.Api.Features.CourseOfferings.UpdateCourseOffering;

namespace StudentManagementSystem.Api.Features.CourseOfferings;

public static class CourseOfferingEndpoints
{
    public static void MapCourseOfferingEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/course-offerings");

        group.MapCreateCourseOfferingEndpoint();
        group.MapGetCourseOfferingEndpoint();
        group.MapGetCourseOfferingsEndpoint();
        group.MapUpdateCourseOfferingEndpoint();
        group.MapDeleteCourseOfferingEndpoint();
        group.MapAssignInstructorEndpoint();
        group.MapRemoveInstructorEndpoint();
    }
}