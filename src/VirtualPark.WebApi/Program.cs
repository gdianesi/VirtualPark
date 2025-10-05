using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<SqlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

builder.Services.AddScoped<IClockAppService, ClockAppService>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
