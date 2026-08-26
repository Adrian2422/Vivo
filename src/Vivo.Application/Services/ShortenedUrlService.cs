using Vivo.Domain.ValueObjects;

namespace Vivo.Application.Services;

using DTOs;
using Interfaces;
using Domain.Entities;
using Vivo.Infrastructure.Interfaces;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IShortenedUrlRepository _repository;

    public ShortenedUrlService(IShortenedUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> CreateShortUrlAsync(string originalUrl, CancellationToken cancellationToken)
    {
        var targetUrl = new TargetUrl(originalUrl);
        var shortCode = new ShortCode(originalUrl);
        
        var shortenedUrl = ShortenedUrlEntity.Create(shortCode, targetUrl);
        await _repository.CreateAsync(shortenedUrl, cancellationToken);

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