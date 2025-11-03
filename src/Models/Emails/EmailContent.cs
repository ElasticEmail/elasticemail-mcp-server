namespace ElasticEmail.Mcp.Service.Models.Emails;

using Utm;

public class EmailContent
{
    /// <summary>
    /// List of e-mail body parts, with user-provided MIME types (text/html, text/plain etc)
    /// </summary>
    public List<BodyPart>? Body { get; set; }
    /// <summary>
    /// A key-value collection of custom merge fields, shared between recipients. Should be used in e-mail body like so: {firstname}, {lastname} etc.
    /// </summary>
    public Dictionary<string, string>? Merge { get; set; }
    /// <summary>
    /// Attachments provided by sending binary data
    /// </summary>
    public List<MessageAttachment>? Attachments { get; set; }
    /// <summary>
    /// A key-value collection of custom e-mail headers.
    /// </summary>
    public Dictionary<string, string>? Headers { get; set; }
    /// <summary>
    /// Postback header.
    /// </summary>
    public string? Postback { get; set; }
    /// <summary>
    /// E-mail with an optional name to be used as the envelope from address (e.g.: John Doe &lt;email@domain.com&gt;)
    /// </summary>
    /// <example>John Doe &lt;email@domain.com&gt;</example>
    public string? EnvelopeFrom { get; set; } // here, as we don't want to show this in Campaigns
    /// <summary>
    /// Your e-mail with an optional name (e.g.: John Doe &lt;email@domain.com&gt;)
    /// </summary>
    /// <example>John Doe &lt;email@domain.com&gt;</example>
    public required string From { get; set; }
    /// <summary>
    /// To what address should the recipients reply to (e.g. John Doe &lt;email@domain.com&gt;)
    /// </summary>
    /// <example>John Doe &lt;email@domain.com&gt;</example>
    public string? ReplyTo { get; set; }

    /// <summary>
    /// Subject of the email.
    /// </summary>
    public string? Subject { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? TemplateName { get; set; }
    /// <summary>
    /// Names of previously uploaded files that should be sent as downloadable attachments
    /// </summary>
    /// <example>preuploaded.jpg</example>
    public List<string>? AttachFiles { get; set; }
    /// <summary>
    /// Utm marketing data to be attached to every link in this e-mail.
    /// </summary>
    public Utm? Utm { get; set; }
}
