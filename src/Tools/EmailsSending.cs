using ElasticEmail.Mcp.Service.Models.Emails;
using System.ComponentModel;
using ElasticEmail.Mcp.Service.Data;
using ElasticEmail.Mcp.Service.Utilities;
using ModelContextProtocol.Server;
using Serilog;

namespace ElasticEmail.Mcp.Service.Tools;

[McpServerToolType]
public class EmailSending(
    IHttpClientService httpClientService,
    McpSerializationService mcpSerializationService,
    ITemplatesDataService templatesManagement,
    IAccountDataService accountDataService)
{
    [McpServerTool, Description("Send bulk emails")]
    public async Task<string> SendBulkEmails([Description("Input email data: content, recipients list, subject")] InputEmailData emailData)
    {
        emailData = await VerifySender(emailData);

        EmailMessageData messageData = (EmailMessageData) await ApplyTemplate(emailData.ConvertToEmailData(), emailData.TemplateName);
        using var content = mcpSerializationService.ToJsonContent(messageData);

        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.PostAsync("/v4/emails", content);

        if (!result.IsSuccessStatusCode)
        {
            Log.Error(@"Sending bulk emails failed. {0}", result.StatusCode);
            result.EnsureSuccessStatusCode();
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Send transactional email")]
    public async Task<string> SendTransactionalEmail(InputEmailData data)
    {
        data = await VerifySender(data);

        using var httpClient = httpClientService.GetClient();
        TransactionalEmailMessageData emailData = (TransactionalEmailMessageData) await ApplyTemplate(data?.ConvertToTransactionalEmailData(), data?.TemplateName);

        using var jsonContent = mcpSerializationService.ToJsonContent(emailData!);
        var result = await httpClient.PostAsync("v4/email/transactional", jsonContent);

        if (!result.IsSuccessStatusCode)
        {
            Log.Error(@"Sending transactional email failed. {0}", result.StatusCode);
            result.EnsureSuccessStatusCode();
        }

        return await result.Content.ReadAsStringAsync();
    }

    private async Task<EmailMessageCore> ApplyTemplate(EmailMessageCore messageData, string? templateName)
    {
        if (string.IsNullOrEmpty(templateName))
            return messageData;

        var template = await templatesManagement.GetTemplateDetails(templateName);
        if (template is not null)
            messageData.ApplyTemplate(template);

        return messageData;
    }

    private async Task<InputEmailData> VerifySender(InputEmailData data)
    {
        if (data is null)
        {
            Log.Error("No input data provided");
            throw new ArgumentNullException(nameof(data), "No input data provided");
        }

        if (string.IsNullOrEmpty(data!.From))
        {
            var details = await accountDataService.GetDetails();
            if (details is null)
            {
                Log.Error("Failed to get account details");
                throw new Exception("Failed to get account details");
            }

            if (string.IsNullOrEmpty(details!.DefaultSender))
            {
                Log.Error("No sender provided. Please provide sender in input data or in account settings.");
                throw new Exception("Can't send email with empty sender. Please provide sender in input data or in account settings.");
            }

            data!.From = details!.DefaultSender;
        }

        return data;
    }
}

