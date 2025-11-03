using System.ComponentModel;
using System.Net;
using System.Net.Http.Headers;
using ElasticEmail.Mcp.Service.Models.Contacts;
using ModelContextProtocol.Server;
using Serilog;

namespace ElasticEmail.Mcp.Service.Tools;

[McpServerToolType]
public class ContactsManagement(IHttpClientService httpClientService, McpSerializationService mcpSerializationService)
{
    [McpServerTool, Description("Fetch existing contacts. By default get 20 contacts but number may be changed.")]
    public async Task<string> FetchContacts(int limit = 20)
    {
        using var httpClient = httpClientService.GetClient();

        var result = await httpClient.GetAsync("/v4/contacts?limit=" + limit);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch contacts. {0}", result.StatusCode);
        }

        Log.Information("Contacts fetched! " + await result.Content.ReadAsStringAsync());
        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get contact history by email address")]
    public async Task<string> FetchContactHistory(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            Log.Error("Can't fetch contact history. Email is empty.");
            return string.Empty;
        }

        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/contacts/" + email + "/history");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch contact history. {0} {1}", result.StatusCode, result.ReasonPhrase);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Fetch existing lists")]
    public async Task<string> FetchLists()
    {
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync("/v4/list");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to fetch lists. {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get list by name")]
    public async Task<string> FetchList(string listName)
    {
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync(@"/v4/list/" + listName);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error(@"Failed to get list {0}. {1}", listName, result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Get contacts from list by list name")]
    public async Task<string> FetchListContacts(string listName)
    {
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.GetAsync(@"/v4/list/" + listName + "/contacts");
        if (!result.IsSuccessStatusCode)
        {
            Log.Error(@"Failed to get list {0} contacts. {1}", listName, result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Add contacts to existing list")]
    public async Task<string> AddContactsToList(string listName, List<string> emails)
    {
        var payload = new
        {
            Emails = emails
        };

        using var content = mcpSerializationService.ToJsonContent(payload);
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.PostAsync("/v4/list/" + listName + "/contacts/", content);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error(@"Failed to add contacts to list. {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Remove contacts from existing list. Provide rule or list of emails, never both in the same time.")]
    public async Task<bool> RemoveContactsFromList(string listName, List<string>? contacts, string? rule)
    {
        if (string.IsNullOrEmpty(rule))
        {
            rule = null;
        }

        if (string.IsNullOrEmpty(listName))
        {
            Log.Error("List name is empty");
            return false;
        }

        RemoveContactPayload payload = new()
        {
            Rule = rule,
            Emails = contacts
        };

        if (!payload.IsDataValid())
        {
            Log.Error("Not all required data provided or both rule and contacts provided.");
            return false;
        }

        using var content = mcpSerializationService.ToJsonContent(payload);
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.PostAsync("/v4/list/" + listName + "/contacts/remove", content);

        return result.IsSuccessStatusCode;
    }

    [McpServerTool, Description("Create new list with provided contacts")]
    public async Task<string> CreateList(string listName, List<string> emails)
    {
        var payload = new
        {
            ListName = listName,
            Emails = emails
        };

        using var content = mcpSerializationService.ToJsonContent(payload);
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.PostAsync("/v4/list/", content);
        if (!result.IsSuccessStatusCode)
        {
            Log.Error("Failed to add contacts to list. {0}", result.StatusCode);
        }

        return await result.Content.ReadAsStringAsync();
    }

    [McpServerTool, Description("Add new simple contact")]
    public async Task<bool> AddContact(string email, string? firstName = "", string? lastName = "")
    {
        if (string.IsNullOrEmpty(email))
        {
            Log.Error("Can't add contact. Email is empty.");
            return false;
        }

        try
        {
            var payload = new List<ContactPayload>() { new ContactPayload() { Email = email, FirstName = firstName, LastName = lastName }};
            using var content = mcpSerializationService.ToJsonContent(payload);
            using var httpClient = httpClientService.GetClient();
            var result = await httpClient.PostAsync("/v4/contact", content);
            if (result.IsSuccessStatusCode) return true;
            Log.Error("Failed to add contact. {0}", result.StatusCode);
            return false;
        }
        catch (Exception e)
        {
            Log.Error("Failed to add contact. {0}", e.Message);
            throw;
        }
    }

    [McpServerTool, Description("Delete contact. Provide rule or list of emails, never both in the same time.")]
    public async Task<bool> DeleteContacts(List<string>? contacts, string? rule)
    {
        if (string.IsNullOrEmpty(rule) || rule.Length == 0)
        {
            rule = null;
        }

        RemoveContactPayload payload = new()
        {
            Rule = rule,
            Emails = contacts
        };

        if (!payload.IsDataValid())
        {
            Log.Error("Not all required data to remove contacts from list is provided or both rule and contacts provided.");
            return false;
        }

        using var content = mcpSerializationService.ToJsonContent(payload);
        using var httpClient = httpClientService.GetClient();
        var result = await httpClient.PostAsync("/v4/contact/delete", content);

        return result.IsSuccessStatusCode;
    }

    [McpServerTool, Description("Uploads/import contacts from file")]
    public async Task<string> UploadContacts(
        string fileName,
        string base64Content,
        string listName)
    {
        Log.Information("Post request created");

        if (!string.IsNullOrEmpty(listName))
        {
            var list = await FetchList(listName);
            if (string.IsNullOrEmpty(list) || list.Contains("Error"))
            {
                await CreateList(listName, new List<string>());
            }
        }

        using var httpClient = httpClientService.GetClient();
        byte[] fileBytes = Convert.FromBase64String(base64Content);

        var byteContent = new ByteArrayContent(fileBytes);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue(GetContentType(fileName));

        using var multipartContent = new MultipartFormDataContent();
        multipartContent.Add(byteContent, "file", fileName);
        multipartContent.Add(new StringContent(listName), "listName");

        var result = await httpClient.PostAsync("/v4/contact/import", multipartContent);
        if (result.StatusCode != HttpStatusCode.Accepted)
        {
            Log.Error("Failed to import contact from file {0} to list {1}. {2}", fileName, listName, result.StatusCode);
            return "Failed to import contact from file " + fileName + " to list " + listName + ". " + result.StatusCode;
        }

        return "Success";
    }

    private static string GetContentType(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return "application/octet-stream";

        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".csv" => "text/csv",
            ".txt" => "text/plain",
            ".json" => "application/json",
            ".zip" => "application/zip",
            _ => "application/octet-stream"
        };
    }
}
