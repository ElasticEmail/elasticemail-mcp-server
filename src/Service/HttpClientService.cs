namespace ElasticEmail.Mcp.Service;

public interface IHttpClientService
{
    HttpClient GetClient();
}

public class HttpClientService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : IHttpClientService
{
    private const string ClientName = "McpToolsClient";

    public HttpClient GetClient()
    {
        var client = httpClientFactory.CreateClient(ClientName);
        var httpContext = httpContextAccessor.HttpContext;
        string? authToken = httpContext?.Request.Headers["X-Auth-Token"].FirstOrDefault();

        client.BaseAddress = new Uri("https://api.elasticemail.com");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        client.DefaultRequestHeaders.Remove("X-Auth-Token");
        client.DefaultRequestHeaders.Add("X-Auth-Token", authToken);

        return client;
    }
}
