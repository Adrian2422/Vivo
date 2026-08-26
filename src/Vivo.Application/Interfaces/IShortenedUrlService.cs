namespace Vivo.Application.Interfaces;

using DTOs;

public interface IShortenedUrlService
{
    Task<IReadOnlyList<ShortenedUrlDto>> GetRecentShortenedUrlsAsync(CancellationToken cancellationToken);
    Task<string> CreateShortUrlAsync(string originalUrl, CancellationToken cancellationToken);
    Task<string?> ResolveOriginalUrlAsync(string code, CancellationToken cancellationToken);
}