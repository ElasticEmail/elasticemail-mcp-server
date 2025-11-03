using System.ComponentModel;
using ElasticEmail.Mcp.Service.Data;
using ElasticEmail.Mcp.Service.Models.Templates;
using ElasticEmail.Mcp.Service.Models.Templates.Enums;
using ModelContextProtocol.Server;
using Serilog;

namespace ElasticEmail.Mcp.Service.Tools;

[McpServerToolType]
public class TemplatesManagement(IHttpClientService httpClientService, ITemplatesDataService templatesDataService)
{
    [McpServerTool, Description("Get template by name")]
    public async Task<EmailTemplate?> FetchTemplate(string name)
    {
        return await templatesDataService.GetTemplateDetails(name);
    }

    [McpServerTool, Description("Get email templates list")]
    public async Task<string> FetchTemplates(List<ScopeType> scopes, List<TemplateType>? templateTypes = null)
    {
        templateTypes ??= [TemplateType.RawHTML, TemplateType.DragDropEditor, TemplateType.TemplateEditor];
        using HttpClient httpClient = httpClientService.GetClient();

        var result = await httpClient.GetAsync("/v4/templates?scopeType=" + string.Join(",", scopes)
                                                                          + "&templateTypes=" + string.Join(",", templateTypes));
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch templates. {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }
}
