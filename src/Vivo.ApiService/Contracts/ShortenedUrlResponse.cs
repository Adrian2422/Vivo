namespace Vivo.ApiService.Contracts;

public sealed record ShortenedUrlResponse(
    string Code,
    string OriginalUrl,
    string ShortUrl,
    DateTime CreatedAt,
    int ClickCount
);