using System.ComponentModel;
using ElasticEmail.Mcp.Service.Models.Segments;
using ModelContextProtocol.Server;
using Serilog;

namespace ElasticEmail.Mcp.Service.Tools;

[McpServerToolType]
public class SegmentsManagement(IHttpClientService httpClientService, McpSerializationService mcpSerializationService)
{
    [McpServerTool, Description("Create new segment")]
    public async Task<string> CreateSegment(Segment segmentData)
    {
        using var content = mcpSerializationService.ToJsonContent(segmentData);
        using HttpClient httpClient = httpClientService.GetClient();

        var result = await httpClient.PostAsync("/v4/segments", content);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to add segment {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get existing segments")]
    public async Task<string> GetSegments()
    {
        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/segments");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch segments {0}, {1}", result.StatusCode, result.ReasonPhrase);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get existing segment by name")]
    public async Task<string> GetSegment(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Log.Error("Can't get segment by empty name. Provide name to get segment.");
            return string.Empty;
        }

        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/segments/" + name);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch {0} segment: {1}", name, result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }
}
