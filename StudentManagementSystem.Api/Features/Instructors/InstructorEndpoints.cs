using StudentManagementSystem.Api.Features.Instructors.CreateInstructor;
using StudentManagementSystem.Api.Features.Instructors.DeleteInstructor;
using StudentManagementSystem.Api.Features.Instructors.GetInstructor;
using StudentManagementSystem.Api.Features.Instructors.GetInstructors;
using StudentManagementSystem.Api.Features.Instructors.UpdateInstructor;

namespace StudentManagementSystem.Api.Features.Instructors;

public static class InstructorEndpoints
{
    public static void MapInstructorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/instructors");

        group.MapCreateInstructorEndpoint();
        group.MapGetInstructorEndpoint();
        group.MapGetInstructorsEndpoint();
        group.MapUpdateInstructorEndpoint();
        group.MapDeleteInstructorEndpoint();
    }
}