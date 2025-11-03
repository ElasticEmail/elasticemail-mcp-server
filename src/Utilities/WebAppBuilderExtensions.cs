using ElasticEmail.Mcp.Service.Authorization;
using ElasticEmail.Mcp.Service.Data;
using ElasticEmail.Mcp.Service.Tools;

namespace ElasticEmail.Mcp.Service.Utilities;

public static class WebAppBuilderExtensions
{
    public static WebApplication BuildMcpWebApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddHttpClient();
        builder.Services.AddScoped<IHttpClientService, HttpClientService>();
        builder.Services.AddScoped<McpSerializationService>();
        builder.Services.AddScoped<ITemplatesDataService, TemplatesDataService>();
        builder.Services.AddScoped<IAccountDataService, AccountDataService>();

        builder.Services.AddMemoryCache();
        builder.Services
            .AddMcpServer()
            .WithHttpTransport()
            .WithTools<EmailSending>()
            .WithTools<ContactsManagement>()
            .WithTools<CampaignManagement>()
            .WithTools<TemplatesManagement>()
            .WithTools<SegmentsManagement>()
            .WithTools<HealthCheck>();

        var app = builder.Build();

        app.UseMiddleware<AuthorizationMiddleware>();
        app.MapMcp();

        return app;
    }
}
