namespace Shared.Results.Response;

using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TranslationKeyAttribute : Attribute
{
    public string Value { get; }
    public TranslationKeyAttribute(string value)
    {
        Value = value;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SortableAttribute : Attribute
{
    public bool Value { get; }
    public SortableAttribute(bool value = true)
    {
        Value = value;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DataTypeAttribute : Attribute
{
    public string Value { get; }
    public DataTypeAttribute(string value)
    {
        Value = value;
    }
}
