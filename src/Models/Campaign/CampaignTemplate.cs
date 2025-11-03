namespace ElasticEmail.Mcp.Service.Models.Campaign;

using Utm;

public class CampaignTemplate
{
    public string Poolname { get; set; }
    public required string From { get; set; }
    /// <summary>
    /// To what address should the recipients reply to (e.g. John Doe &lt;email@domain.com&gt;)
    /// </summary>
    /// <example>John Doe &lt;email@domain.com&gt;</example>
    public string ReplyTo { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string Subject { get; set; }
    /// <summary>
    ///
    /// </summary>
    public string TemplateName { get; set; }
    /// <summary>
    /// Names of previously uploaded files that should be sent as downloadable attachments
    /// </summary>
    /// <example>preuploaded.jpg</example>
    public List<string> AttachFiles { get; set; }
    /// <summary>
    /// Utm marketing data to be attached to every link in this e-mail.
    /// </summary>
    public Utm Utm { get; set; }
}
