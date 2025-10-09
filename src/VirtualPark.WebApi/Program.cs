using VirtualPark.ApiServiceFactory;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Filters.Exception;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

ServiceFactory.RegisterServices(builder.Services);

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var clockService = scope.ServiceProvider.GetRequiredService<IClockAppService>();
    ValidationServices.ClockService = clockService;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
