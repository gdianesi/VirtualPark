using VirtualPark.ApiServiceFactory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

ServiceFactory.RegisterServices(builder.Services);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
