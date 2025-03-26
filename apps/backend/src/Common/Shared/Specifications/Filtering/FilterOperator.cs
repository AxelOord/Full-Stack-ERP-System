namespace Shared.Specifications.Filtering;
public class FilterOperator
{
    // TODO: should be enums
    // Equality operators
    public new const string Equals = "eq";
    public const string NotEquals = "neq";

    // Comparison operators
    public const string GreaterThan = "gt";
    public const string GreaterThanOrEqual = "gte";
    public const string LessThan = "lt";
    public const string LessThanOrEqual = "lte";

    // String operators
    public const string Contains = "contains";
    public const string StartsWith = "startswith";
    public const string EndsWith = "endswith";

    // Collection operators
    public const string Any = "any";
    public const string All = "all";

    // Null checking
    public const string IsNull = "isnull";
    public const string IsNotNull = "isnotnull";

    // Range operators
    public const string Between = "between";
    public const string In = "in";

    public static List<string> SupportedOperators => new()
        {
            Equals, NotEquals,
            GreaterThan, GreaterThanOrEqual, LessThan, LessThanOrEqual,
            Contains, StartsWith, EndsWith,
            Any, All,
            IsNull, IsNotNull,
            Between, In
        };
}