using Vivo.Domain.Exceptions;

namespace Vivo.Domain.ValueObjects;

public record ShortCode
{
    public string Value { get; }

    public ShortCode(string value)
    {

        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidShortCodeException("Short code cannot be empty.", "SHORT_CODE_CANNOT_BE_EMPTY");

        if (value.Length is < 4 or > 12)
            throw new InvalidShortCodeException("Short code length must be between 4 and 12 characters.", "SHORT_CODE_INVALID_LENGTH");

        if (!value.All(c => char.IsLetterOrDigit(c)))
            throw new InvalidShortCodeException("Short code can only contain letters and digits.", "SHORT_CODE_INVALID_COMPOSITION");

        Value = value;
    }

    public override string ToString() => Value;
}