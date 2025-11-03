using ElasticEmail.Mcp.Service.Models.Emails;
using ElasticEmail.Mcp.Service.Models.Templates;

namespace ElasticEmail.Mcp.Service.Utilities;

public static class EmailTemplateExtensions
{
    public static void ApplyTemplate(this EmailMessageCore? messageData, EmailTemplate? template)
    {
        if (template is null || messageData is null || messageData?.Content is null)
            return;

        if (messageData!.Content!.Body == null)
        {
            messageData!.Content!.Body = [];
        }

        messageData!.Content!.Body!.AddRange(template!.Body);

        if (string.IsNullOrWhiteSpace(messageData!.Content!.Subject))
        {
            messageData!.Content!.Subject = template!.Subject;
        }
    }
}
