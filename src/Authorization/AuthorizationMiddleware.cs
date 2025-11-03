using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Serilog;

namespace ElasticEmail.Mcp.Service.Authorization;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApiKeyName = "X-Auth-Token";
    private const int NotAuthorizedResponseCode = 401;
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public AuthorizationMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://api.elasticemail.com");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
        {
            context.Response.StatusCode = NotAuthorizedResponseCode;
            await context.Response.WriteAsync("API Key was not provided");
            return;
        }

        if (!(await ApiKeyIsValid(extractedApiKey)))
        {
            context.Response.StatusCode = NotAuthorizedResponseCode;
            await context.Response.WriteAsync("Unauthorized client");
            return;
        }

        var sessionKey = $"apikey_{extractedApiKey}";
        if (!_cache.TryGetValue(sessionKey, out var sessionData))
        {
            _cache.Set(sessionKey, extractedApiKey,TimeSpan.FromMinutes(30));
        }

        await _next(context);
    }

    /// <summary>
    /// Confirms api key is valid and allowed to access api
    /// </summary>
    /// <param name="clientApiKey">Api key from mcp (sse) request header</param>
    /// <returns>True if api key has access</returns>
    private async Task<bool> ApiKeyIsValid(string? clientApiKey)
    {
        if (!string.IsNullOrWhiteSpace(clientApiKey))
        {
            _httpClient.DefaultRequestHeaders.Remove(ApiKeyName);
            _httpClient.DefaultRequestHeaders.Add(ApiKeyName, clientApiKey);
            try
            {
                var result = await _httpClient.GetAsync("/v4/security/apikeys");
                Log.Information("Security call status: " + result.StatusCode);
                if (result.StatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                var content = await result.Content.ReadAsStringAsync();
                var apiKeys = JToken.Parse(content);
                if (apiKeys.Type == JTokenType.Array)
                {
                    return apiKeys.Any();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        return false;
    }
}
