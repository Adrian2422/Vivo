namespace Vivo.Application.DTOs;

public sealed record ShortenedUrlDto(
    Guid Id,
    string Code,
    string OriginalUrl,
    DateTime CreatedAt,
    int ClickCount
);