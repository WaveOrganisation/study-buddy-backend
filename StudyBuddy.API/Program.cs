using StudyBuddy.API.Extensions;
using StudyBuddy.API.Middlewares;
using StudyBuddy.Application.Interfaces.Auth;
using StudyBuddy.Application.Interfaces.Repositories;
using StudyBuddy.Application.Services;
using StudyBuddy.Persistence;
using StudyBuddy.Persistence.Repositories;
using StudyBuddy.Infrastructure;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddApiAuthentication(configuration);

services.AddEndpointsApiExplorer();

services.AddSwaggerGen();

services.AddTransient<ExceptionMiddleware>();

services.AddDbContext<StudyBuddyDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(StudyBuddyDbContext)));
});

services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

//services.AddScoped<ICourseRepository, CourseRepository>();
//services.AddScoped<ILessonsRepository, LessonsRepository>();
services.AddScoped<IUsersRepository, UsersRepository>();

//services.AddScoped<CoursesService>();
//services.AddScoped<LessonsService>();
services.AddScoped<UserService>();

services.AddAutoMapper(typeof(DataBaseMappings));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseAuthentication();

app.UseAuthorization();

app.AddMappedEndpoints();

app.Run();