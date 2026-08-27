namespace Vivo.ApiService.Controllers;

using Swashbuckle.AspNetCore.Annotations;
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
    [SwaggerOperation(
        Summary = "Get recently shortened urls",
        Description = "Retrieve 10 recently shortened urls",
        OperationId = "GetRecentShortenedUrls",
        Tags = ["ShortenedUrls"]
    )]
    public async Task<ActionResult<List<ShortenedUrlResponse>>> GetRecentShortenedUrls(CancellationToken cancellationToken)
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
    [SwaggerOperation(
        Summary = "Create new shortened url",
        Description = "Retrieve url, shorten it and store in database",
        OperationId = "CreateAsync",
        Tags = ["ShortenedUrls"]
    )]
    public async Task<ActionResult<CreateShortenedUrlResponse>> CreateAsync(
        [FromBody] CreateShortenedUrlRequest request,
        CancellationToken cancellationToken)
    {
        var code = await _service.CreateShortUrlAsync(request.OriginalUrl, request.RequestedExpiresAt, cancellationToken);
        var shortUrl = $"{Request.Scheme}://{Request.Host}/{code}";

        return Created(shortUrl, new CreateShortenedUrlResponse(shortUrl));
    }

    [HttpGet("/{code}")]
    [SwaggerOperation(
        Summary = "Get url and redirect",
        Description = "Retrieve url, read its original url and redirect",
        OperationId = "RedirectToOriginal",
        Tags = ["ShortenedUrls"]
    )]
    [Produces("text/html")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RedirectToOriginal(
        [FromRoute] string code,
        CancellationToken cancellationToken)
    {
        var originalUrl = await _service.ResolveOriginalUrlAsync(code, cancellationToken);
        if (originalUrl is null)
            return NotFound();

        return Redirect(originalUrl);
    }
}