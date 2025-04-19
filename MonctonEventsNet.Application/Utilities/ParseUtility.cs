using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Utilities;

public static class ParseUtility
{
    public static Cost ParseCost(string? cost)
    {
        if (string.IsNullOrWhiteSpace(cost))
        {
            return new Cost();
        }

        cost = cost.Trim().Replace("$", string.Empty);

        var costParts = cost.Split(new[] { '-', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        var minCost = 0m;
        decimal? maxCost = null;
        string? information = null;

        if (costParts.Length == 1)
        {
            if (decimal.TryParse(costParts[0], out var parsedCost))
            {
                minCost = parsedCost;
            }
            else
            {
                information = costParts[0];
            }
        }
        else if (costParts.Length == 2)
        {
            if (!decimal.TryParse(costParts[0], out minCost))
            {
                information = costParts[0];
            }

            if (!decimal.TryParse(costParts[1], out decimal parsedMaxCost))
            {
                if (information is null)
                    information = costParts[1];
                else
                    information = string.Concat(information, " - ", costParts[1]);
            }
            else
            {
                maxCost = parsedMaxCost;
            }
        }

        return new Cost
        {
            MinCost = minCost,
            MaxCost = maxCost,
            Information = information
        };
    }
}