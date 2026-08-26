namespace Vivo.ApiService.Contracts;

public record CreateShortenedUrlRequest(string OriginalUrl, DateTime? RequestedExpiresAt);