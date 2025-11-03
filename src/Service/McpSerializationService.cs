using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElasticEmail.Mcp.Service;

public class McpSerializationService
{
    private JsonSerializerOptions _options = new JsonSerializerOptions
    {
        IncludeFields = true,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        PropertyNameCaseInsensitive = true
    };

    public StringContent ToJsonContent(object obj)
    {
        var json = JsonSerializer.Serialize(obj, _options);
        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json");

        return content;
    }

    public T DeserializeWithHtml<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }
}
