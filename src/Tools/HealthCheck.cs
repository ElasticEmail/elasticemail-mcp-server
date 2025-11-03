using System.ComponentModel;
using ModelContextProtocol.Server;

namespace ElasticEmail.Mcp.Service.Tools;

[McpServerToolType]
public class HealthCheck(IHttpClientService httpClientService)
{
    /// <summary>
    /// It will require changes or it should be removed in the future
    /// </summary>
    [McpServerTool, Description("Check readiness and connectivity with MCP server and API")]
    public async Task<string> IsReady()
    {
        try
        {
            var client = httpClientService.GetClient();
            var result = await client.GetAsync("/v4/list");
            return result.IsSuccessStatusCode ? "healthy" : "server up, api connection not healthy: " + result.StatusCode;
        }
        catch (Exception e)
        {
            return "not healthy";
        }
    }
}
