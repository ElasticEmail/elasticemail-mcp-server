using ElasticEmail.Mcp.Service.Models.Emails;
using ElasticEmail.Mcp.Service.Models.Emails.Enums;
using ElasticEmail.Mcp.Service.Models.Templates;
using File = ElasticEmail.Mcp.Service.Models.Emails.File;

namespace ElasticEmail.Mcp.Service.Utilities;

public static class InputEmailDataExtensions
{
    public static EmailMessageData ConvertToEmailData(this InputEmailData data)
    {
        var requiredData = new EmailMessageData
        {
            Recipients = (data.To ?? []).Select(GetRecipient).ToList(),
            Content = GetContent(data),
            Options = new EmailOptions()
        };

        return requiredData;
    }

    public static TransactionalEmailMessageData ConvertToTransactionalEmailData(this InputEmailData data)
    {
        var requiredData = new TransactionalEmailMessageData
        {
            Recipients = new TransactionalRecipient { To = data.To ?? [] },
            Content = GetContent(data),
            Options = new EmailOptions()
        };

        return requiredData;
    }

    private static EmailRecipient GetRecipient(string email)
    {
        return new EmailRecipient { Email = email, Fields = new Dictionary<string, string>() };
    }

    private static EmailContent GetContent(InputEmailData data)
    {
        return new EmailContent
        {
            From = data?.From ?? string.Empty,
            Subject = data?.Subject,
            Body = GetBody(data?.Content ?? string.Empty),
            Attachments = data?.Attachments?.Select(GetAttachment).ToList() ?? new List<MessageAttachment>(),
        };
    }

    private static MessageAttachment GetAttachment(File file)
    {
        return new MessageAttachment
        {
            ContentType = "application/octet-stream",
            BinaryContent = file.Data,
            Name = file.FileName,
            Size = file.Data?.Length ?? 0
        };
    }

    private static List<BodyPart> GetBody(string content)
    {
        var part = new BodyPart
        {
            ContentType = SetContentType(content),
            Content = content
        };

        return [part];
    }

    private static BodyContentType SetContentType(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return BodyContentType.PlainText;
        }

        content = content.ToLowerInvariant();

        return content switch
        {
            var c when c.Contains("<html") ||
                       c.Contains("<!doctype html") ||
                       c.Contains("<head") ||
                       c.Contains("<body") ||
                       c.Contains("<div") ||
                       c.Contains("<p>") ||
                       c.Contains("<span") ||
                       c.Contains("<h1") ||
                       c.Contains("<h2") ||
                       c.Contains("<h3") => BodyContentType.HTML,
            _ => BodyContentType.PlainText
        };
    }
}
