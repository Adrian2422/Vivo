using Vivo.Application.DTOs;

namespace Vivo.Application.Interfaces;

public interface IShortenedUrlService
{
    Task<IReadOnlyList<ShortenedUrlDto>> GetRecentShortenedUrlsAsync(CancellationToken cancellationToken);
    Task<string> CreateShortUrlAsync(string originalUrl, CancellationToken cancellationToken);
    Task<string?> ResolveOriginalUrlAsync(string code, CancellationToken cancellationToken);
}