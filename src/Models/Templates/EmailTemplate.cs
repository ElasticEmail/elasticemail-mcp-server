using ElasticEmail.Mcp.Service.Models.Emails;
using ElasticEmail.Mcp.Service.Models.Templates.Enums;
using Newtonsoft.Json;
using Serilog;

namespace ElasticEmail.Mcp.Service.Models.Templates;

public class EmailTemplate
{
    public required string Name { get; set; }
    public string Subject { get; set; }
    public List<BodyPart> Body { get; set; }
    public DateTime?  Added { get; set; }
    public TemplateType Type { get; set; }
    public ScopeType Scope { get; set; }

    public static async Task<EmailTemplate?> FromApi(HttpContent content, string name)
    {
        EmailTemplate? result = null;
        if (string.IsNullOrEmpty(name))
            return result;

        try
        {
            var jsonContent = await content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<EmailTemplate>(jsonContent);
            if (string.IsNullOrEmpty(result.Name))
                result.Name = name;
        }
        catch (Exception e)
        {
            Log.Error("Can't read email template from API response. {0}", e.Message);
            return null;
        }

        return result;
    }
}
