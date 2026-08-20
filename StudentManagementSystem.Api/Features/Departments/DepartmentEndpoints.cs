using StudentManagementSystem.Api.Features.Departments.CreateDepartment;
using StudentManagementSystem.Api.Features.Departments.DeleteDepartment;
using StudentManagementSystem.Api.Features.Departments.GetDepartment;
using StudentManagementSystem.Api.Features.Departments.GetDepartments;
using StudentManagementSystem.Api.Features.Departments.GetDepartmentCourses;
using StudentManagementSystem.Api.Features.Departments.UpdateDepartment;

namespace StudentManagementSystem.Api.Features.Departments;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/departments");

        group.MapCreateDepartmentEndpoint();
        group.MapGetDepartmentEndpoint();
        group.MapGetDepartmentsEndpoint();
        group.MapUpdateDepartmentEndpoint();
        group.MapDeleteDepartmentEndpoint();
        group.MapGetDepartmentCoursesEndpoint();
    }
}