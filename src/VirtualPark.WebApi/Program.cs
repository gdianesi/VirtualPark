using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.DataAccess;
using VirtualPark.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IReadOnlyRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IClockAppService, ClockAppService>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
