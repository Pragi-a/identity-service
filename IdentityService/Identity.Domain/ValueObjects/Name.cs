namespace Identity.Domain.ValueObjects;

public sealed record Name
{
    public string Value { get; }

    public Name(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Name value should not be empty");

        Value = value;
    }
}