using Shared.Specifications.Filtering;
using System.Text.RegularExpressions;

namespace Shared.Utils;
public static class BracketNotationParser
{
    private static readonly Regex BracketPattern = new(@"^([^\[\]]+)(?:\[([^\[\]]+)\])+$");

    public static FilterDefinition ParseQueryString(IDictionary<string, string> queryParams)
    {
        var filterDefinition = new FilterDefinition();
        var rootGroup = filterDefinition.RootGroup;

        var paramGroups = GroupParametersByRootProperty(queryParams);

        foreach (var group in paramGroups)
        {
            ProcessParameterGroup(group.Key, group.Value, rootGroup);
        }

        return filterDefinition;
    }

    private static Dictionary<string, Dictionary<string, string>> GroupParametersByRootProperty(
        IDictionary<string, string> queryParams)
    {
        var result = new Dictionary<string, Dictionary<string, string>>();

        foreach (var param in queryParams)
        {
            var match = BracketPattern.Match(param.Key);

            if (match.Success)
            {
                var rootProperty = match.Groups[1].Value;

                if (!result.ContainsKey(rootProperty))
                {
                    result[rootProperty] = new Dictionary<string, string>();
                }

                result[rootProperty][param.Key] = param.Value;
            }
            else if (param.Key.Contains("[") && param.Key.Contains("]"))
            {
                // Handle simpler bracket notations like "field[operator]=value"
                var parts = param.Key.Split('[', 2);
                var rootProperty = parts[0];

                if (!result.ContainsKey(rootProperty))
                {
                    result[rootProperty] = new Dictionary<string, string>();
                }

                result[rootProperty][param.Key] = param.Value;
            }
        }

        return result;
    }

    private static void ProcessParameterGroup(
        string rootProperty,
        Dictionary<string, string> parameters,
        FilterGroup parentGroup)
    {
        // Process simple case like "field[eq]=value"
        foreach (var param in parameters)
        {
            var path = param.Key;
            var value = param.Value;

            var parts = ExtractPartsFromBracketNotation(path);

            if (parts.Count > 0)
            {
                // If there's only one part after the root property, it's likely an operator
                if (parts.Count == 1 && FilterOperator.SupportedOperators.Contains(parts[0].ToLower()))
                {
                    parentGroup.Criteria.Add(new FilterCriteria
                    {
                        Path = rootProperty,
                        Operator = parts[0].ToLower(),
                        Value = value
                    });
                }

                else if (parts.Count >= 1)
                {
                    // Check if the last part is an operator
                    var lastPart = parts[parts.Count - 1].ToLower();

                    if (FilterOperator.SupportedOperators.Contains(lastPart))
                    {
                        // Remove the operator from the parts to build the path
                        var pathParts = parts.Take(parts.Count - 1).ToList();
                        var fullPath = BuildPropertyPath(rootProperty, pathParts);

                        parentGroup.Criteria.Add(new FilterCriteria
                        {
                            Path = fullPath,
                            Operator = lastPart,
                            Value = value
                        });
                    }
                    else
                    {
                        // No explicit operator, use equals by default
                        var fullPath = BuildPropertyPath(rootProperty, parts);

                        parentGroup.Criteria.Add(new FilterCriteria
                        {
                            Path = fullPath,
                            Operator = FilterOperator.Equals,
                            Value = value
                        });
                    }
                }
            }
        }
    }

    private static List<string> ExtractPartsFromBracketNotation(string key)
    {
        var parts = new List<string>();
        var matches = Regex.Matches(key, @"\[([^\[\]]+)\]");

        foreach (Match match in matches)
        {
            parts.Add(match.Groups[1].Value);
        }

        return parts;
    }

    private static string BuildPropertyPath(string rootProperty, List<string> parts)
    {
        var path = rootProperty;

        foreach (var part in parts)
        {
            // Skip array indexers (numeric parts)
            if (!int.TryParse(part, out _))
            {
                path += "." + part;
            }
        }

        return path;
    }
}
