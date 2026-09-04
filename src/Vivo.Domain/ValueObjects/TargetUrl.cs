using Vivo.Domain.Exceptions;

namespace Vivo.Domain.ValueObjects;

public record TargetUrl
{
    public string Value { get; }

    public TargetUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOriginalUrlException("URL cannot be empty.", "ORIGINAL_URL_REQUIRED");

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            throw new InvalidOriginalUrlException("URL format is invalid or malformed.", "MALFORMED_ORIGINAL_URL");

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new InvalidOriginalUrlException("Only 'http' and 'https' schemes are supported.", "INVALID_URL_SCHEME");

        Value = value;
    }

    public override string ToString() => Value;
}