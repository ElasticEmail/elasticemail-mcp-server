namespace ElasticEmail.Mcp.Service.Models.Emails;

public class EmailMessageData : EmailMessageCore
{
    public required List<EmailRecipient> Recipients { get; set; }
}
