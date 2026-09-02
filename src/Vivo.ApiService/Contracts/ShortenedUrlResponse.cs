using System.ComponentModel.DataAnnotations;

namespace Vivo.ApiService.Contracts;

public sealed record ShortenedUrlResponse(
     [property: Required] Guid Id,
     [property: Required] string Code,
     [property: Required] string OriginalUrl,
     [property: Required] string ShortUrl,
     [property: Required] DateTime CreatedAt,
     [property: Required] int ClickCount
);