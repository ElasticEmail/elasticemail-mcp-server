namespace ElasticEmail.Mcp.Service.Models.Campaign;

public class CampaignRecipient
{
    /// <summary>
    /// Names of lists from your Account to read recipients from
    /// </summary>
    public List<string> ListNames {get; set;}
    /// <summary>
    /// Names of segments from your Account to read recipients from
    /// </summary>
    public List<string> SegmentNames {get; set;}
}
