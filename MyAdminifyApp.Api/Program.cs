

using MyAdminifyApp.Api.Extensions;
using MyAdminifyApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);


var app = builder.Build();
app.MapControllers();


app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();
