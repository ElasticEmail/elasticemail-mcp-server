using System.ComponentModel;

namespace ElasticEmail.Mcp.Service.Models.Contacts;

public record ContactPayload
{
    [Description("Email address of the contact")]
    public string Email { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
