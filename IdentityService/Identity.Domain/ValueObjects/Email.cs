namespace Identity.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }

    public Email(string value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        value = Normalize(value);
        
        Validate(value);
        Value = value;
    }

    private static void Validate(string value)
    {
        var parts = value.Split('@');
        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
        {
            throw new ArgumentException("Invalid email address", nameof(value));
        }
    }

    private static string Normalize(string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}