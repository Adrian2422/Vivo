namespace Vivo.Application.Repositories;

using Domain.Entities;

public interface IShortenedUrlRepository
{
    Task<IReadOnlyList<ShortenedUrlEntity>> GetRecentShortenedUrls(CancellationToken cancellationToken);
    Task CreateAsync(ShortenedUrlEntity item, CancellationToken cancellationToken);
    Task<ShortenedUrlEntity?> GetByCodeAsync(string code, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken cancellationToken);
    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken);
}