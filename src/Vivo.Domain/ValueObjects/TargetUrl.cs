namespace Vivo.Domain.ValueObjects;

public record TargetUrl
{
    public string Value { get; }

    public TargetUrl(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("URL cannot be empty.", nameof(value));

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            throw new ArgumentException("URL is not well-formed.", nameof(value));

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            throw new ArgumentException("Only http and https schemes are allowed.", nameof(value));

        Value = value;
    }

    public override string ToString() => Value;
}