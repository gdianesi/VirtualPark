using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.Permissions.Service;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.BusinessLogic.RewardRedemptions.Service;
using VirtualPark.BusinessLogic.Rewards.Service;
using VirtualPark.BusinessLogic.Roles.Service;
using VirtualPark.BusinessLogic.Sessions.Service;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets.Service;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.Validations.Services;
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
        services.AddScoped<IAttractionService, AttractionService>();
        services.AddScoped<IIncidenceService, IncidenceService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRankingService, RankingService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ITypeIncidenceService, TypeIncidenceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IVisitorProfileService, VisitorProfileService>();
        services.AddScoped<IStrategy, AttractionPointsStrategy>();
        services.AddScoped<IStrategy, ComboPointsStrategy>();
        services.AddScoped<IStrategy, EventPointsStrategy>();
        services.AddScoped<IStrategyService, ActiveStrategyService>();
        services.AddScoped<IStrategyFactory, StrategyFactory>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IVisitRegistrationService, VisitRegistrationService>();
        services.AddScoped<IRewardService, RewardService>();
        services.AddScoped<IRewardRedemptionService, RewardRedemptionService>();
        services.BuildServiceProvider().GetRequiredService<IClockAppService>();
        ValidationServices.ClockService = services.BuildServiceProvider().GetRequiredService<IClockAppService>();
    }
}
