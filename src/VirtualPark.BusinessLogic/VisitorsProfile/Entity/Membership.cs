namespace VirtualPark.BusinessLogic.VisitorsProfile.Entity;

/// <summary>
///     Types of visitor memberships available in the park.
/// </summary>
public enum Membership
{
    /// <summary> Default membership. </summary>
    Standard,

    /// <summary> Grants extra benefits. </summary>
    Premium,

    /// <summary> Highest tier with all benefits. </summary>
    VIP
}
