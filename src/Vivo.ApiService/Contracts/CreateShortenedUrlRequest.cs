using System.ComponentModel.DataAnnotations;

namespace Vivo.ApiService.Contracts;

public record CreateShortenedUrlRequest
{
    [Required] public required string OriginalUrl { get; init; }

    public DateTime? RequestedExpiresAt { get; init; }
};