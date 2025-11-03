using ElasticEmail.Mcp.Service.Models.Campaign.Enums;

namespace ElasticEmail.Mcp.Service.Models.Campaign;

public class CampaignOptions
{
    /// <summary>
    /// How to order email delivery - by recipients' engagement score or by the time they open the most of the emails that were sent to them
    /// </summary>
    public DeliveryOptimizationType DeliveryOptimization { get; set; }
    public bool? TrackOpens { get; set; }
    public bool? TrackClicks { get; set; }
    /// <summary>
    /// Date when this Campaign is scheduled to be sent on
    /// </summary>
    public DateTime? ScheduleFor { get; set; }

    /// <summary>
    /// How often (in minutes) to send the campaign
    /// </summary>
    public double TriggerFrequency { get; set; }

    /// <summary>
    /// How many times send the campaign
    /// </summary>
    public int TriggerCount { get; set; }

    /// <summary>
    /// Optional options for A/X split campaigns. Will be ignored if only one template content was provided
    /// </summary>
    public SplitOptions SplitOptions  { get; set; }
}
