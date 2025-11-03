using ElasticEmail.Mcp.Service.Models.Emails.Enums;

namespace ElasticEmail.Mcp.Service.Models.Emails;

public class BodyPart
{
    /// <summary>
    /// Type of the body part
    /// </summary>
    public required BodyContentType ContentType { get; set; } // only provided ones? HTML, plain text, Office Calendar, embedded images...
    /// <summary>
    /// Actual content of the body part
    /// </summary>
    public required string Content { get; set; }
    /// <summary>
    /// Text value of charset encoding for example: iso-8859-1, windows-1251, utf-8, us-ascii, windows-1250 and more...
    /// </summary>
    public string Charset { get; set; }
}
