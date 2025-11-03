namespace ElasticEmail.Mcp.Service.Models.Campaign.Enums;

public enum CampaignStatus
{
    /// <summary>
    /// Campaign is logically deleted and not returned by API or interface calls.
    /// </summary>
    Deleted = -1,

    /// <summary>
    /// Campaign is curently active and available.
    /// </summary>
    Active = 0,

    /// <summary>
    /// Campaign is currently being processed for delivery.
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Campaign is currently sending.
    /// </summary>
    Sending = 2,

    /// <summary>
    /// Campaign has completed sending.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Campaign is currently paused and not sending.
    /// </summary>
    Paused = 4,

    /// <summary>
    /// Campaign has been cancelled during delivery.
    /// </summary>
    Cancelled = 5,
    /// <summary>
    /// Campaign is save as draft and not processing.
    /// </summary>
    Draft = 6
}
