namespace ElasticEmail.Mcp.Service.Models.Templates.Enums;

public enum TemplateType
{
    /// <summary>
    /// Template supports any valid HTML
    /// </summary>
    RawHTML = 0,
    /// <summary>
    /// Template is created for email and can only be modified in the drag and drop email editor
    /// </summary>
    DragDropEditor = 1,
    /// <summary>
    /// Template is created for email, and can be modified only by block editor.
    /// </summary>
    TemplateEditor = 3,
}
