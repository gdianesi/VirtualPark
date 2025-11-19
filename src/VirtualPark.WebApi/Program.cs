using VirtualPark.ApiServiceFactory;
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
    options.AddPolicy("AllowAngularClient",
        policy =>
        {
            if(builder.Environment.IsDevelopment())
            {
                policy.SetIsOriginAllowed(origin =>
                    {
                        if(string.IsNullOrWhiteSpace(origin))
                        {
                            return false;
                        }

                        var uri = new Uri(origin);
                        return uri.Host == "localhost" || uri.Host == "127.0.0.1";
                    })
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
            else
            {
                policy.WithOrigins(
                        "https://prod.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
        });
});

ServiceFactory.RegisterServices(builder.Services, builder.Configuration);

WebApplication app = builder.Build();

app.UseCors("AllowAngularClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
