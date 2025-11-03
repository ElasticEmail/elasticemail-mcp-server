namespace ElasticEmail.Mcp.Service.Models.Emails;

public class TransactionalRecipient
{
    /// <summary>
    /// List of recipients (visible to others)
    /// </summary>
    public required List<string> To { get; set; }
    /// <summary>
    /// List of Carbon Copy recipients (visible to others)
    /// </summary>
    public List<string> CC { get; set; }
    /// <summary>
    /// List of Blind Carbon Copy recipients (hidden from other recipients)
    /// </summary>
    public List<string> BCC { get; set; }
}
