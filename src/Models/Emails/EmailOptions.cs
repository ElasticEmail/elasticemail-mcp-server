using ElasticEmail.Mcp.Service.Models.Emails.Enums;

namespace ElasticEmail.Mcp.Service.Models.Emails;

public class EmailOptions
{
    /// <summary>
    /// By how long should an e-mail be delayed (in minutes). Maximum is 35 days.
    /// </summary>
    public int? TimeOffset { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string PoolName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string ChannelName { get; set; }
    /// <summary>
    ///
    /// </summary>
    public EncodingType Encoding { get; set; }
    /// <summary>
    ///
    /// </summary>
    public bool? TrackOpens { get; set; }
    /// <summary>
    ///
    /// </summary>
    public bool? TrackClicks { get; set; }
}
