using VirtualPark.ApiServiceFactory;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Filters.Exception;
using VirtualPark.WebApi.Filters.ResponseNormalization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
    options.Filters.Add<ResponseNormalizationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

ServiceFactory.RegisterServices(builder.Services, builder.Configuration);

WebApplication app = builder.Build();

ValidationServices.ScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

app.UseCors("AllowAngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
