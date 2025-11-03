using ElasticEmail.Mcp.Service.Models.Templates;
using Serilog;

namespace ElasticEmail.Mcp.Service.Data;

public interface ITemplatesDataService
{
    Task<EmailTemplate> GetTemplateDetails(string name);
}

public class TemplatesDataService(IHttpClientService httpClientService) : ITemplatesDataService
{
    public async Task<EmailTemplate> GetTemplateDetails(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Log.Error("Template name is empty");
            return new EmailTemplate() { Name = name };
        }

        using HttpClient httpClient = httpClientService.GetClient();

        var result = await httpClient.GetAsync("/v4/templates/" + name);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch template. {0}", result.StatusCode);
            return new EmailTemplate() { Name = name };
        }

        return await EmailTemplate.FromApi(result.Content, name) ?? new EmailTemplate() { Name = name };
    }
}
