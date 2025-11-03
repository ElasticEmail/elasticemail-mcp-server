using System.Text.Json;
using ElasticEmail.Mcp.Service.Models.Account;
using Serilog;

namespace ElasticEmail.Mcp.Service.Data;

public interface IAccountDataService
{
    Task<AccountDetails?> GetDetails();
}

public class AccountDataService(IHttpClientService httpClientService) : IAccountDataService
{
    /// <summary>
    /// API v4 can't currently provide account details
    /// </summary>
    /// <returns>Account information</returns>
    public async Task<AccountDetails?> GetDetails()
    {
        using HttpClient httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v2/account/load");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch account details. {0}", result.StatusCode);
            return null;
        }

        try
        {
            var jsonContent = await result.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(jsonContent);
            var account = new AccountDetails();
            account.DefaultSender = doc.RootElement.GetProperty("data").GetProperty("defaultsender").GetString();

            return account;
        }
        catch (Exception e)
        {
            Log.Error("Can't read account details from API response. {0}", e.Message);
            return null;
        }
    }
}
