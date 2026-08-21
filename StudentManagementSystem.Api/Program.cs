using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Api.Data;
using StudentManagementSystem.Api.Features.Students;
using StudentManagementSystem.Api.Features.Courses;
using StudentManagementSystem.Api.Features.Departments;
using StudentManagementSystem.Api.Features.Semesters;
using StudentManagementSystem.Api.Features.Instructors;
using StudentManagementSystem.Api.Features.CourseOfferings;
using StudentManagementSystem.Api.Features.Enrollments;
using StudentManagementSystem.Api.Features.Courses.Prerequisites;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<StudentManagementDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return "Student Management System API is running";
});



app.MapStudentEndpoints();
app.MapCourseEndpoints();
app.MapDepartmentEndpoints();
app.MapSemesterEndpoints();
app.MapInstructorEndpoints();
app.MapCourseOfferingEndpoints();
app.MapEnrollmentEndpoints();
app.MapCoursePrerequisiteEndpoints();

app.Run();
