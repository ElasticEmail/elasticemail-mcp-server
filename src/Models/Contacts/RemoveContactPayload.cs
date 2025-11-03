using System.ComponentModel;

namespace ElasticEmail.Mcp.Service.Models.Contacts;

public class RemoveContactPayload
{
    [Description("Email address of the contact")]
    public List<string>? Emails { get; set; }

    [Description("SQl-like rule to filter contacts")]
    public string? Rule { get; set; }

    public bool IsDataValid()
    {
        var notEmpty = !((Emails is null || Emails.Count == 0) && string.IsNullOrEmpty(Rule));
        var notBoth = !(Emails is not null && !string.IsNullOrEmpty(Rule));
        return notEmpty && notBoth;
    }
}
