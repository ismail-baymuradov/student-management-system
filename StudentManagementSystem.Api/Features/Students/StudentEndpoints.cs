using StudentManagementSystem.Api.Features.Students.CreateStudent;
using StudentManagementSystem.Api.Features.Students.GetStudent;
using StudentManagementSystem.Api.Features.Students.GetStudents;
using StudentManagementSystem.Api.Features.Students.UpdateStudent;
using StudentManagementSystem.Api.Features.Students.DeleteStudent;


namespace StudentManagementSystem.Api.Features.Students;

public static class StudentEndpoints
{
    public static void MapStudentEndpoints(
        this WebApplication app)
    {
        var group = app.MapGroup("/students");

        group.MapCreateStudent();
        group.MapGetStudent();
        group.MapGetStudents();
        group.MapUpdateStudentEndpoint();
        group.MapDeleteStudentEndpoint();
    }
}