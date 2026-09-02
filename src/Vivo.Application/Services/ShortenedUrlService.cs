namespace Vivo.Application.Services;

using DTOs;
using Domain.Entities;
using Domain.ValueObjects;
using Abstractions;
using Repositories;

public class ShortenedUrlService : IShortenedUrlService
{
    private readonly IShortenedUrlRepository _repository;
    private readonly IShortCodeGenerator _codeGenerator;

    public ShortenedUrlService(IShortenedUrlRepository repository, IShortCodeGenerator codeGenerator)
    {
        _repository = repository;
        _codeGenerator = codeGenerator;
    }

    public async Task<string> CreateShortUrlAsync(string originalUrl, DateTime? requestedExpiresAt, CancellationToken cancellationToken)
    {
        var targetUrl = new TargetUrl(originalUrl);

        string code;
        do
        {
            code = _codeGenerator.Generate();
        } while (await _repository.CodeExistsAsync(code, cancellationToken));

        var shortenedUrl = ShortenedUrlEntity.Create(new ShortCode(code), targetUrl, requestedExpiresAt);

        await _repository.CreateAsync(shortenedUrl, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return code;
    }

    public async Task<IReadOnlyList<ShortenedUrlDto>> GetRecentShortenedUrlsAsync(CancellationToken cancellationToken)
    {
        var items = await _repository.GetRecentShortenedUrls(cancellationToken);

        return items.Select(x => new ShortenedUrlDto(
            x.Id,
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

        entity.RegisterClick();
        await _repository.SaveChangesAsync(cancellationToken);

        return entity.OriginalUrl;
    }
}