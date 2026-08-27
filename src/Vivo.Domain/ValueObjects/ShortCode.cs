namespace Vivo.Domain.ValueObjects;

public record ShortCode
{
    public string Value { get; }

    public ShortCode(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Short code cannot be empty.", nameof(value));

        if (value.Length is < 4 or > 12)
            throw new ArgumentException("Short code length must be between 4 and 12 characters.", nameof(value));

        if (!value.All(c => char.IsLetterOrDigit(c)))
            throw new ArgumentException("Short code can only contain letters and digits.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}