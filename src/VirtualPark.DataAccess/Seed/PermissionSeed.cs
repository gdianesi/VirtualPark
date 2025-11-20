using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VirtualPark.BusinessLogic.Permissions.Entity;

namespace VirtualPark.DataAccess.Seed;

[ExcludeFromCodeCoverage]
public static class PermissionSeed
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Permission>().HasData(
            new Permission
            {
                Id = Guid.Parse("10101010-1111-1111-1111-111111111111"),
                Key = "GetUserById-User",
                Description = "Allows retrieving details of a specific user"
            },
            new Permission
            {
                Id = Guid.Parse("10101010-1111-1111-1111-111111111112"),
                Key = "GetAllUsers-User",
                Description = "Allows listing all users"
            },
            new Permission
            {
                Id = Guid.Parse("10101010-1111-1111-1111-111111111113"),
                Key = "UpdateUser-User",
                Description = "Allows updating user data"
            },
            new Permission
            {
                Id = Guid.Parse("10101010-1111-1111-1111-111111111114"),
                Key = "DeleteUser-User",
                Description = "Allows deleting a user"
            },
            new Permission
            {
                Id = Guid.Parse("10101010-1111-1111-1111-111111111115"),
                Key = "CreateUser-User",
                Description = "Allows creating new users"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Key = "GetAttractionById-Attraction",
                Description = "Allows retrieving details of a specific attraction"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111112"),
                Key = "CreateAttraction-Attraction",
                Description = "Allows creating attractions"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111113"),
                Key = "GetAllAttractions-Attraction",
                Description = "Allows listing all attractions"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111114"),
                Key = "UpdateAttraction-Attraction",
                Description = "Allows updating attraction data"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111115"),
                Key = "DeleteAttraction-Attraction",
                Description = "Allows deleting an attraction"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111119"),
                Key = "GetDeletedAttractions-Attraction",
                Description = "Allows listening all deleted attractions"
            },
            new Permission
            {
                Id = Guid.Parse("22222222-1111-1111-1111-111111111111"),
                Key = "GetEventById-Event",
                Description = "Allows retrieving details of a specific event"
            },
            new Permission
            {
                Id = Guid.Parse("22222222-1111-1111-1111-111111111112"),
                Key = "CreateEvent-Event",
                Description = "Allows creating events"
            },
            new Permission
            {
                Id = Guid.Parse("22222222-1111-1111-1111-111111111113"),
                Key = "GetAllEvents-Event",
                Description = "Allows listing all events"
            },
            new Permission
            {
                Id = Guid.Parse("22222222-1111-1111-1111-111111111114"),
                Key = "UpdateEvent-Event",
                Description = "Allows updating event data"
            },
            new Permission
            {
                Id = Guid.Parse("22222222-1111-1111-1111-111111111115"),
                Key = "DeleteEvent-Event",
                Description = "Allows deleting an event"
            },
            new Permission
            {
                Id = Guid.Parse("33333333-1111-1111-1111-111111111111"),
                Key = "GetTicketById-Ticket",
                Description = "Allows retrieving a ticket by ID"
            },
            new Permission
            {
                Id = Guid.Parse("33333333-1111-1111-1111-111111111112"),
                Key = "CreateTicket-Ticket",
                Description = "Allows creating tickets"
            },
            new Permission
            {
                Id = Guid.Parse("33333333-1111-1111-1111-111111111113"),
                Key = "GetAllTickets-Ticket",
                Description = "Allows listing all tickets"
            },
            new Permission
            {
                Id = Guid.Parse("33333333-1111-1111-1111-111111111114"),
                Key = "DeleteTicket-Ticket",
                Description = "Allows deleting a ticket"
            },
            new Permission
            {
                Id = Guid.Parse("33333333-1111-1111-1111-111111111115"),
                Key = "GetTicketsByVisitor-Ticket",
                Description = "Allows listing a ticket by visitor"
            },
            new Permission
            {
                Id = Guid.Parse("12121212-1111-1111-1111-111111111111"),
                Key = "GetVisitorProfileById-VisitorProfile",
                Description = "Allows retrieving visitor profile"
            },
            new Permission
            {
                Id = Guid.Parse("12121212-1111-1111-1111-111111111112"),
                Key = "UpdateVisitorProfile-VisitorProfile",
                Description = "Allows updating visitor profile"
            },
            new Permission
            {
                Id = Guid.Parse("66666666-1111-1111-1111-111111111111"),
                Key = "GetIncidenceById-Incidence",
                Description = "Allows retrieving an incidence"
            },
            new Permission
            {
                Id = Guid.Parse("66666666-1111-1111-1111-111111111112"),
                Key = "CreateIncidence-Incidence",
                Description = "Allows creating incidences"
            },
            new Permission
            {
                Id = Guid.Parse("66666666-1111-1111-1111-111111111113"),
                Key = "GetAllIncidences-Incidence",
                Description = "Allows listing incidences"
            },
            new Permission
            {
                Id = Guid.Parse("66666666-1111-1111-1111-111111111114"),
                Key = "UpdateIncidence-Incidence",
                Description = "Allows updating incidence info"
            },
            new Permission
            {
                Id = Guid.Parse("66666666-1111-1111-1111-111111111115"),
                Key = "DeleteIncidence-Incidence",
                Description = "Allows deleting incidences"
            },
            new Permission
            {
                Id = Guid.Parse("44444444-1111-1111-1111-111111111111"),
                Key = "GetTypeIncidenceById-TypeIncidence",
                Description = "Allows retrieving a type incidence"
            },
            new Permission
            {
                Id = Guid.Parse("44444444-1111-1111-1111-111111111112"),
                Key = "GetAllTypeIncidences-TypeIncidence",
                Description = "Allows listing all type incidences"
            },
            new Permission
            {
                Id = Guid.Parse("44444444-1111-1111-1111-111111111113"),
                Key = "CreateTypeIncidence-TypeIncidence",
                Description = "Allows creating type incidences"
            },
            new Permission
            {
                Id = Guid.Parse("44444444-1111-1111-1111-111111111114"),
                Key = "UpdateTypeIncidence-TypeIncidence",
                Description = "Allows updating type incidences"
            },
            new Permission
            {
                Id = Guid.Parse("44444444-1111-1111-1111-111111111115"),
                Key = "DeleteTypeIncidence-TypeIncidence",
                Description = "Allows deleting type incidences"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111111"),
                Key = "GetStrategyById-Strategy",
                Description = "Allows retrieving a strategy"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111112"),
                Key = "GetAllStrategies-Strategy",
                Description = "Allows listing all strategies"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111113"),
                Key = "CreateStrategy-Strategy",
                Description = "Allows creating strategies"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111114"),
                Key = "UpdateStrategy-Strategy",
                Description = "Allows updating strategies"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111115"),
                Key = "DeleteStrategy-Strategy",
                Description = "Allows deleting strategies"
            },
            new Permission
            {
                Id = Guid.Parse("88888888-1111-1111-1111-111111111111"),
                Key = "GetRankingByPeriod-Ranking",
                Description = "Allows retrieving ranking by period"
            },
            new Permission
            {
                Id = Guid.Parse("88888888-1111-1111-1111-111111111112"),
                Key = "GetAllRankings-Ranking",
                Description = "Allows listing all rankings"
            },
            new Permission
            {
                Id = Guid.Parse("99999999-1111-1111-1111-111111111111"),
                Key = "GetRoleById-Role",
                Description = "Allows retrieving a role"
            },
            new Permission
            {
                Id = Guid.Parse("99999999-1111-1111-1111-111111111112"),
                Key = "CreateRole-Role",
                Description = "Allows creating roles"
            },
            new Permission
            {
                Id = Guid.Parse("99999999-1111-1111-1111-111111111113"),
                Key = "GetAllRoles-Role",
                Description = "Allows listing roles"
            },
            new Permission
            {
                Id = Guid.Parse("99999999-1111-1111-1111-111111111114"),
                Key = "UpdateRole-Role",
                Description = "Allows updating roles"
            },
            new Permission
            {
                Id = Guid.Parse("99999999-1111-1111-1111-111111111115"),
                Key = "DeleteRole-Role",
                Description = "Allows deleting roles"
            },
            new Permission
            {
                Id = Guid.Parse("77777777-1111-1111-1111-111111111111"),
                Key = "GetPermissionById-Permission",
                Description = "Allows retrieving a permission"
            },
            new Permission
            {
                Id = Guid.Parse("77777777-1111-1111-1111-111111111112"),
                Key = "CreatePermission-Permission",
                Description = "Allows creating permissions"
            },
            new Permission
            {
                Id = Guid.Parse("77777777-1111-1111-1111-111111111113"),
                Key = "GetAllPermissions-Permission",
                Description = "Allows listing permissions"
            },
            new Permission
            {
                Id = Guid.Parse("77777777-1111-1111-1111-111111111114"),
                Key = "UpdatePermission-Permission",
                Description = "Allows updating permissions"
            },
            new Permission
            {
                Id = Guid.Parse("77777777-1111-1111-1111-111111111115"),
                Key = "DeletePermission-Permission",
                Description = "Allows deleting permissions"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111111"),
                Key = "GetRewardById-Reward",
                Description = "Allows retrieving details of a specific reward"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111112"),
                Key = "CreateReward-Reward",
                Description = "Allows creating new rewards"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111113"),
                Key = "GetAllRewards-Reward",
                Description = "Allows listing all rewards"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111114"),
                Key = "UpdateReward-Reward",
                Description = "Allows updating rewards"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111115"),
                Key = "DeleteReward-Reward",
                Description = "Allows deleting rewards"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111116"),
                Key = "GetDeletedRewards-Reward",
                Description = "Allows listing deleted rewards"
            },
            new Permission
            {
                Id = Guid.Parse("55555555-1111-1111-1111-111111111117"),
                Key = "RestoreReward-Reward",
                Description = "Allows restore a deleted reward"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-1111-1111-1111-111111111111"),
                Key = "RedeemReward-RewardRedemption",
                Description = "Allows redeeming rewards"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-1111-1111-1111-111111111112"),
                Key = "GetRewardRedemptionById-RewardRedemption",
                Description = "Allows retrieving a specific reward redemption"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-1111-1111-1111-111111111113"),
                Key = "GetAllRewardRedemptions-RewardRedemption",
                Description = "Allows listing all reward redemptions"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-1111-1111-1111-111111111114"),
                Key = "GetRewardRedemptionsByVisitor-RewardRedemption",
                Description = "Allows retrieving redemptions of a specific visitor"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111111"),
                Key = "GetAttractionsForTicket-VisitRegistration",
                Description = "Allows listing available attractions for a visitor visit"
            },
            new Permission
            {
                Id = Guid.Parse("88888888-1111-1111-1111-111111111114"),
                Key = "UpdateClock-ClockApp",
                Description = "Allows update clockApp"
            },
            new Permission
            {
                Id = Guid.Parse("13131313-1111-1111-1111-111111111116"),
                Key = "CreateActiveStrategy-Strategy",
                Description = "Allows creating active strategy"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111118"),
                Key = "ValidateEntryByNfc-Attraction",
                Description = "Allows validating entrance via NFC"
            },
            new Permission
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111117"),
                Key = "ValidateEntryByQr-Attraction",
                Description = "Allows validating entrance via QR"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111112"),
                Key = "UpToAttraction-VisitRegistration",
                Description = "Allows registering an attraction boarding for a visit"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111113"),
                Key = "DownToAttraction-VisitRegistration",
                Description = "Allows registering when a visitor leaves an attraction"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111114"),
                Key = "GetVisitorsInAttraction-VisitRegistration",
                Description = "Allows listing the visitors currently in an attraction"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111115"),
                Key = "RecordScoreEvent-VisitRegistration",
                Description = "Allows registering a score event for a visit"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111120"),
                Key = "GetHistoryById-VisitScores",
                Description = "Allows retrieving the visit score history"
            },
            new Permission
            {
                Id = Guid.Parse("56565656-2222-1111-1111-111111111121"),
                Key = "GetVisitForToday-VisitRegistration",
                Description = "Allows retrieving today's visit registration for a visitor"
            });
    }
}
