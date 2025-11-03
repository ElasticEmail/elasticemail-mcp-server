namespace ElasticEmail.Mcp.Service.Models.Emails;

public class EmailRecipient
{
    public string Email { get; set; }
    /// <summary>
    /// A key-value collection of merge fields which can be used in e-mail body.
    /// </summary>
    public Dictionary<string, string>? Fields { get; set; }
}
