namespace ElasticEmail.Mcp.Service.Models.Emails;

public abstract class EmailMessageCore
{
    public required EmailContent Content { get; set; }
    /// <summary>
    /// E-mail configuration
    /// </summary>
    public EmailOptions Options { get; set; }
}
