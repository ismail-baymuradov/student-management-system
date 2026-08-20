using StudentManagementSystem.Api.Features.Semesters.CreateSemester;
using StudentManagementSystem.Api.Features.Semesters.DeleteSemester;
using StudentManagementSystem.Api.Features.Semesters.GetSemester;
using StudentManagementSystem.Api.Features.Semesters.GetSemesters;
using StudentManagementSystem.Api.Features.Semesters.UpdateSemester;

namespace StudentManagementSystem.Api.Features.Semesters;

public static class SemesterEndpoints
{
    public static void MapSemesterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/semesters");

        group.MapCreateSemesterEndpoint();
        group.MapGetSemesterEndpoint();
        group.MapGetSemestersEndpoint();
        group.MapUpdateSemesterEndpoint();
        group.MapDeleteSemesterEndpoint();
    }
}