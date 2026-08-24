using Vivo.Domain.Entities;

namespace Vivo.Domain.Interfaces;

public interface IShortenedUrlRepository
{
    Task<IReadOnlyList<ShortenedUrlEntity>> GetRecentShortenedUrls(CancellationToken cancellationToken);
    Task CreateAsync(ShortenedUrlEntity item, CancellationToken cancellationToken);
    Task<ShortenedUrlEntity?> GetByCodeAsync(string code, CancellationToken cancellationToken);
}