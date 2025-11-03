namespace ElasticEmail.Mcp.Service.Models.Emails;

public class MessageAttachment
{
    /// <summary>
    /// File's content as byte array (or a Base64 string)
    /// </summary>
    public byte[]? BinaryContent { get; set; }
    /// <summary>
    /// Display name of the file
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// MIME content type
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Size of the attachement in B
    /// </summary
    public int Size { get; set; }
}
