using System.Net;
using ElasticEmail.Mcp.Service.Utilities;
using Serilog;
using Serilog.Events;

string logFileName = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Name ?? "service";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logFileName + ".log", rollingInterval: RollingInterval.Day, retainedFileTimeLimit: TimeSpan.FromDays(30), retainedFileCountLimit: null)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
    serverOptions.AddServerHeader = false;

    serverOptions.Listen(IPAddress.Parse("0.0.0.0"), 5001);
    Log.Information("Listening on port 5001. HTTP enabled.");
});

builder.Services.AddSerilog();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("mcp_service.log", rollingInterval: RollingInterval.Day));

builder.Services.AddSerilog();

var app = builder.BuildMcpWebApplication();



await app.RunAsync();
