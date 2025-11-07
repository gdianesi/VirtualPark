using VirtualPark.ApiServiceFactory;
using VirtualPark.WebApi.Filters.Exception;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200",
                    "http://localhost:51931")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

ServiceFactory.RegisterServices(builder.Services);

WebApplication app = builder.Build();

app.UseCors("AllowAngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
