using ElasticEmail.Mcp.Service.Models.Campaign.Enums;

namespace ElasticEmail.Mcp.Service.Models.Campaign;

public class SplitOptions
{
    /// <summary>
    /// Type of results by which to determine the winner template (content)
    /// </summary>
    public SplitOptimizationType OptimizeFor;

    /// <summary>
    /// For how long should the results be measured until determining the winner template (content)
    /// </summary>
    public int OptimizePeriodMinutes {get; set;}
}
