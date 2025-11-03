using System.ComponentModel;

namespace ElasticEmail.Mcp.Service.Models.Emails;

public class InputEmailData
{
    [Description("Email content, can be plain text or html")]
    public string? Content { get; set; }
    [Description("Email subject")]
    public string? Subject { get; set; }
    [Description("Recipients list")]
    public List<string>? To { get; set; }
    [Description("Sender email")]
    public string? From { get; set; }

    [Description("Existing template name, optional")]
    public string? TemplateName { get; set; }

    [Description("Attached files")]
    public List<File>? Attachments { get; set; }
}
