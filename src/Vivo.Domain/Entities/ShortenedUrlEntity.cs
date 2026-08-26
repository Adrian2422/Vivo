using Vivo.Domain.ValueObjects;

namespace Vivo.Domain.Entities;

public class ShortenedUrlEntity : BaseEntity
{
    public required string Code { get; init; }
    
    public required string OriginalUrl { get; init; }
    
    public DateTime? ExpiresAt { get; init; }

    public int ClickCount { get; set; }
    
    public static ShortenedUrlEntity Create(ShortCode code, TargetUrl originalUrl)
    {
        return new ShortenedUrlEntity
        {
            Code = code.Value,
            OriginalUrl = originalUrl.Value
        };
    }

    public void RegisterClick() => ClickCount++;
    
    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow;
}