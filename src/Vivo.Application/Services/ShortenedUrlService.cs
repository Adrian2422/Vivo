using Vivo.Domain.ValueObjects;

namespace Vivo.Application.Services;

using DTOs;
using Interfaces;
using Domain.Entities;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IShortenedUrlRepository _repository;

    public ShortenedUrlService(IShortenedUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> CreateShortUrlAsync(string originalUrl, DateTime? requestedExpiresAt, CancellationToken cancellationToken)
    {
        var targetUrl = new TargetUrl(originalUrl);
        var shortCode = new ShortCode("dolor");

        var shortenedUrl = ShortenedUrlEntity.Create(shortCode, targetUrl, requestedExpiresAt);
        await _repository.CreateAsync(shortenedUrl, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        
        return originalUrl;
    }
    
    public async Task<IReadOnlyList<ShortenedUrlDto>> GetRecentShortenedUrlsAsync(CancellationToken cancellationToken)
    {
        var items = await _repository.GetRecentShortenedUrls(cancellationToken);

        return items.Select(x => new ShortenedUrlDto(
            x.Code,
            x.OriginalUrl,
            x.CreatedAt,
            x.ClickCount))
            .ToList();
    }
    
    public async Task<string?> ResolveOriginalUrlAsync(string code, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByCodeAsync(code, cancellationToken);
        if (entity is null || entity.IsExpired)
            return null;

        return entity.OriginalUrl;
    }
}