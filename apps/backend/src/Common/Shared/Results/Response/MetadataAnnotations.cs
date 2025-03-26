namespace Shared.Results.Response;

using System;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class TranslationKeyAttribute : Attribute
{
    public string Value { get; }
    public TranslationKeyAttribute(string value) => Value = value;
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class SortableAttribute(bool value = true) : Attribute
{
    public bool Value { get; } = value;
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DataTypeAttribute(string value) : Attribute
{
    public string Value { get; } = value;
}

[AttributeUsage(AttributeTargets.Property)]
public class FilterPathAttribute(string path) : Attribute
{
    public string Path { get; } = path;
}
