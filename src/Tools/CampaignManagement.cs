using System.ComponentModel;
using ElasticEmail.Mcp.Service.Models.Campaign;
using ElasticEmail.Mcp.Service.Utilities;
using ModelContextProtocol.Server;
using Serilog;

namespace ElasticEmail.Mcp.Service.Tools;

[McpServerToolType]
public class CampaignManagement(IHttpClientService httpClientService, McpSerializationService mcpSerializationService)
{
    [McpServerTool, Description("Creates and sends a new campaign. " +
                                "By default, the campaign status is set to active, and the campaign is sent immediately upon creation." +
                                "To delay sending, set the status to paused." +
                                "You can also create draft campaign and finish creation proccess later.")]
    public async Task<string> CreateCampaign(Campaign campaignData)
    {
        if (!campaignData.AllRequiredDataProvided())
        {
            Log.Error("Not all required data to create campaign is provided.");
            return string.Empty;
        }

        using var content = mcpSerializationService.ToJsonContent(campaignData);
        using HttpClient httpClient = httpClientService.GetClient();

        var result = await httpClient.PostAsync("/v4/campaigns", content);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to add campaign. {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("List existing campaigns")]
    public async Task<string> ListCampaigns()
    {
        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/campaigns");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch campaigns. {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get existing campaign")]
    public async Task<string> GetCampaign(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Log.Error("Campaign name is empty - can't get details.");
            return string.Empty;
        }

        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/campaigns/" + name);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to get campaign {0}. {1}", name, result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Pause selected capaign")]
    public async Task<bool> PauseCampaign(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Log.Error("Can't pause campaign - name is empty");
            return false;
        }

        using HttpClient httpClient = httpClientService.GetClient();
        var content = new StringContent(string.Empty);
        var result = await httpClient.PutAsync($"/v4/campaigns/{name}/pause", content);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to pause campaign {0}. {1}", name, result.StatusCode);
            return false;
        }

        return true;
    }

    [McpServerTool, Description("Update selected capaign")]
    public async Task<string> UpdateCampaign(Campaign campaignData)
    {
        if (!campaignData.AllRequiredDataProvided())
        {
            Log.Error("Not all required data to create campaign is provided.");
            return "Not updated";
        }

        Log.Information("data provided");

        using HttpClient httpClient = httpClientService.GetClient();
        var content = mcpSerializationService.ToJsonContent(campaignData);
        var result = await httpClient.PutAsync($"/v4/campaigns/{campaignData.Name}", content);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to update campaign {0}. {1} {2}", campaignData.Name, result.StatusCode, await result.Content.ReadAsStringAsync());
            return "Not updated";
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get campaign selected by name statistics")]
    public async Task<string> GetCampaignStatistics(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Log.Error("Campaign name can't be empty to get statistics.");
            return string.Empty;
        }

        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync($"/v4/statistics/campaigns/{name}");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to get campaign {0} statistics. {1} {2}", name, result.StatusCode, result.ReasonPhrase);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get all campaigns statistics")]
    public async Task<string> GetAllCampaignStatistics()
    {
        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/statistics/campaigns");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to get campaign statistics. {0} {1}", result.StatusCode, result.ReasonPhrase);
        }

        return await result.Content.ReadAsStringAsync();
    }
}
