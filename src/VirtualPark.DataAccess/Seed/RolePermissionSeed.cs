using Microsoft.EntityFrameworkCore;

namespace VirtualPark.DataAccess.Seed;

public static class RolePermissionSeed
{
    // Visitor permissions
    private static readonly string[] VisitorPermissionsArray =
    [
        "33333333-1111-1111-1111-111111111112", // CreateTicket
        "33333333-1111-1111-1111-111111111111", // GetTicketById
        "12121212-1111-1111-1111-111111111111", // GetVisitorProfileById
        "88888888-1111-1111-1111-111111111111", // GetRanking
    ];

    // Admin permissions
    private static readonly string[] AdminPermissionsArray =
    [

        // Attractions
        "11111111-1111-1111-1111-111111111111", // CreateAttraction
        "11111111-1111-1111-1111-111111111112", // GetAttractionById
        "11111111-1111-1111-1111-111111111113", // GetAllAttractions
        "11111111-1111-1111-1111-111111111114", // UpdateAttraction
        "11111111-1111-1111-1111-111111111115", // DeleteAttraction

        // Events
        "22222222-1111-1111-1111-111111111111", // CreateEvent
        "22222222-1111-1111-1111-111111111112", // GetEventById
        "22222222-1111-1111-1111-111111111113", // GetAllEvents
        "22222222-1111-1111-1111-111111111114", // UpdateEvent
        "22222222-1111-1111-1111-111111111115", // DeleteEvent

        // Users
        "10101010-1111-1111-1111-111111111111", // GetUserById
        "10101010-1111-1111-1111-111111111112", // GetAllUsers
        "10101010-1111-1111-1111-111111111113", // UpdateUser
        "10101010-1111-1111-1111-111111111114", // DeleteUser
        "10101010-1111-1111-1111-111111111115", // CreateUser

        // Tickets
        "33333333-1111-1111-1111-111111111111", // GetTicketById
        "33333333-1111-1111-1111-111111111113", // GetAllTickets
        "33333333-1111-1111-1111-111111111114", // DeleteTicket
        "33333333-1111-1111-1111-111111111112", // CreateTicket

        // Incidences
        "66666666-1111-1111-1111-111111111111", // CreateIncidence
        "66666666-1111-1111-1111-111111111112", // GetIncidenceById
        "66666666-1111-1111-1111-111111111113", // GetAllIncidences
        "66666666-1111-1111-1111-111111111115", // DeleteIncidence
        "66666666-1111-1111-1111-111111111114", // UpdateIncidence

        // Roles
        "99999999-1111-1111-1111-111111111111", // CreateRole
        "99999999-1111-1111-1111-111111111112", // GetRoleById
        "99999999-1111-1111-1111-111111111113", // GetAllRoles
        "99999999-1111-1111-1111-111111111115", // DeleteRole
        "99999999-1111-1111-1111-111111111114", // UpdateRole

        // Permissions
        "77777777-1111-1111-1111-111111111111", // CreatePermission
        "77777777-1111-1111-1111-111111111112", // GetPermissionById
        "77777777-1111-1111-1111-111111111113", // GetAllPermissions
        "77777777-1111-1111-1111-111111111115", // DeletePermission
        "77777777-1111-1111-1111-111111111114", // UpdatePermission

        // TypeIncidences
        "44444444-1111-1111-1111-111111111111", // CreateTypeIncidence
        "44444444-1111-1111-1111-111111111112", // GetTypeIncidenceById
        "44444444-1111-1111-1111-111111111113", // GetAllTypeIncidences
        "44444444-1111-1111-1111-111111111114", // DeleteTypeIncidence
        "44444444-1111-1111-1111-111111111115", // UpdateTypeIncidence

        // Strategies
        "13131313-1111-1111-1111-111111111111", // CreateStrategy
        "13131313-1111-1111-1111-111111111112", // GetActiveStrategy
        "13131313-1111-1111-1111-111111111113", // GetAllStrategies
        "13131313-1111-1111-1111-111111111114", // UpdateStrategy
        "13131313-1111-1111-1111-111111111115", // DeleteStrategy

        // Rankings
        "88888888-1111-1111-1111-111111111111", // GetRanking
        "88888888-1111-1111-1111-111111111112", // GetAllRankings

        // VisitorProfiles
        "12121212-1111-1111-1111-111111111111", // GetVisitorProfileById
        "12121212-1111-1111-1111-111111111112" // GetAllVisitorProfiles
    ];

        // Operator permissions
    private static readonly string[] OperatorPermissionsArray =
    [

        // Attractions
        "11111111-1111-1111-1111-111111111112", // GetAttractionById
        "11111111-1111-1111-1111-111111111113", // GetAllAttractions

        // Incidences
        "66666666-1111-1111-1111-111111111111", // CreateIncidence
        "66666666-1111-1111-1111-111111111112", // GetIncidenceById
        "66666666-1111-1111-1111-111111111113", // GetAllIncidences
        "66666666-1111-1111-1111-111111111115", // DeleteIncidence
    ];

    public static void Seed(ModelBuilder modelBuilder)
    {
        // Visitor
        Guid visitorRoleId = Guid.Parse("CCCC1111-1111-1111-1111-111111111111");
        var visitorPermissions = VisitorPermissionsArray.Select(pid => new
        {
            RolesId = visitorRoleId,
            PermissionsId = Guid.Parse(pid)
        });

        // Admin
        Guid adminRoleId = Guid.Parse("AAAA1111-1111-1111-1111-111111111111");
        var adminPermissions = AdminPermissionsArray.Select(pid => new
        {
            RolesId = adminRoleId,
            PermissionsId = Guid.Parse(pid)
        });

        // Operator
        Guid operatorRoleId = Guid.Parse("BBBB1111-1111-1111-1111-111111111111");
        var operatorPermissions = OperatorPermissionsArray.Select(pid => new
        {
            RolesId = operatorRoleId,
            PermissionsId = Guid.Parse(pid)
        });

        modelBuilder.Entity("PermissionRole")
            .HasData(visitorPermissions
                .Concat(adminPermissions)
                .Concat(operatorPermissions));
    }
}
