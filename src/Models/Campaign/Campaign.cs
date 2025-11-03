using ElasticEmail.Mcp.Service.Models.Campaign.Enums;

namespace ElasticEmail.Mcp.Service.Models.Campaign;

public class Campaign
{
    public Campaign()
    {
        Options = new() { DeliveryOptimization = DeliveryOptimizationType.None };
    }

    public Campaign(
        DeliveryOptimizationType deliveryOptimizationType = DeliveryOptimizationType.None,
        CampaignStatus status = CampaignStatus.Draft)
    {
        Status = status;
        Options = new() { DeliveryOptimization = deliveryOptimizationType };
    }

    public List<CampaignTemplate> Content { get; set; }

    /// <summary>
    /// Campaign name
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Campaign status
    /// </summary>
    public CampaignStatus Status { get; set; }

    /// <summary>
    /// Recipients this campaign should be sent to
    /// </summary>
    public required CampaignRecipient Recipients  { get; set; }

    /// <summary>
    /// Campaign sending options
    /// </summary>
    public CampaignOptions Options  { get; set; }
}
