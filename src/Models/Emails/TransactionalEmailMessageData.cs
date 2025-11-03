namespace ElasticEmail.Mcp.Service.Models.Emails;

public class TransactionalEmailMessageData : EmailMessageCore
{
    public required TransactionalRecipient Recipients { get; set; }
}
