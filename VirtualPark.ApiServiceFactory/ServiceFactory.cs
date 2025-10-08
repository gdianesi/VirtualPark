using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.Permissions.Service;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.BusinessLogic.Roles.Service;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets.Service;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.DataAccess;
using VirtualPark.Repository;

namespace VirtualPark.ApiServiceFactory;

public static class ServiceFactory
{
    [ExcludeFromCodeCoverage]
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddDbContext<SqlContext>(options =>
            options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

        services.AddScoped<DbContext>(sp => sp.GetRequiredService<SqlContext>());

        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped(typeof(IReadOnlyRepository<>), typeof(GenericRepository<>));

        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IClockAppService, ClockAppService>();
        services.AddScoped<AttractionService>();
        services.AddScoped<IncidenceService>();
        services.AddScoped<PermissionService>();
        services.AddScoped<RankingService>();
        services.AddScoped<RoleService>();
        services.AddScoped<TicketService>();
        services.AddScoped<TypeIncidenceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVisitorProfileService, VisitorProfileService>();
        services.AddScoped<VisitRegistrationService>();
        services.AddScoped<ActiveStrategyService>();
        services.AddScoped<IStrategy, AttractionPointsStrategy>();
        services.AddScoped<IStrategy, ComboPointsStrategy>();
        services.AddScoped<IStrategy, EventPointsStrategy>();
        services.AddScoped<IStrategyFactory, StrategyFactory>();
        services.AddScoped<ISessionService, SessionService>();
    }
}
