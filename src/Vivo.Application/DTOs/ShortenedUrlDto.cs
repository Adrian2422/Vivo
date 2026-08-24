namespace Vivo.Application.DTOs;

public sealed record ShortenedUrlDto(
    string Code,
    string OriginalUrl,
    DateTime CreatedAt,
    int ClickCount
);