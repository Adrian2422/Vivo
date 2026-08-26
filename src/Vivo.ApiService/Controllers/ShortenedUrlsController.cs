namespace Vivo.ApiService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Contracts;
using Application.Interfaces;

[Route("api/shortened-url")]
public class ShortenedUrlsController : ControllerBase
{

    private readonly IShortenedUrlService _service;

    public ShortenedUrlsController(IShortenedUrlService service)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetRecentShortenedUrls(CancellationToken cancellationToken)
    {
        var items = await _service.GetRecentShortenedUrlsAsync(cancellationToken);
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        var response = items.Select(x => new ShortenedUrlResponse(
            x.Code,
            x.OriginalUrl,
            $"{baseUrl}/{x.Code}",
            x.CreatedAt,
            x.ClickCount));

        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateShortenedUrlRequest request,
        CancellationToken cancellationToken)
    {
        var code = await _service.CreateShortUrlAsync(request.OriginalUrl, cancellationToken);
        var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";


        return Created(shortUrl, new CreateShortenedUrlResponse(shortUrl));
    }
    
    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectToOriginal(
        string code,
        CancellationToken cancellationToken)
    {
        var originalUrl = await _service.ResolveOriginalUrlAsync(code, cancellationToken);
        if (originalUrl is null)
            return NotFound();

        return Redirect(originalUrl);
    }
}