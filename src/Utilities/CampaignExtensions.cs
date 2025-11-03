using ElasticEmail.Mcp.Service.Models.Campaign;
using Serilog;

namespace ElasticEmail.Mcp.Service.Utilities;

public static class CampaignExtensions
{
    public static bool AllRequiredDataProvided(this Campaign data)
    {
        bool isAllRequiredData = true;
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(data.Name))
        {
            Log.Error("Campaign name cannot be empty");
            isAllRequiredData = false;
        }

        if (data.Recipients.ListNames.Count == 0 && data.Recipients.SegmentNames.Count == 0)
        {
            Log.Error("At least one of the Recipients.ListNames or Recipients.SegmentNames must be specified");
            isAllRequiredData = false;
        }

        if (data.Content != null && data.Content.Count > 0)
        {
            foreach (var content in data.Content)
            {
                if (string.IsNullOrWhiteSpace(content.Subject))
                {
                    Log.Error("Subject cannot be empty");
                    isAllRequiredData = false;
                }

                if (string.IsNullOrWhiteSpace(content.From))
                {
                    Log.Error("From cannot be empty");
                    isAllRequiredData = false;
                }

                if (string.IsNullOrEmpty(content.ReplyTo))
                {
                    content.ReplyTo = content.From;
                }

                if (string.IsNullOrWhiteSpace(content.Poolname))
                {
                    Log.Error("Poolname cannot be empty");
                }
            }
        }

        return isAllRequiredData;
    }
}
