namespace VirtualPark.BusinessLogic.Rankings;

/// <summary>
/// Periods used for ranking and scoring calculations.
/// </summary>
public enum Period
{
    /// <summary>
    /// Ranking or scoring calculated on a daily basis.
    /// </summary>
    Daily,

    /// <summary>
    /// Ranking or scoring calculated on a weekly basis.
    /// </summary>
    Weekly,

    /// <summary>
    /// Ranking or scoring calculated on a monthly basis.
    /// </summary>
    Monthly
}
