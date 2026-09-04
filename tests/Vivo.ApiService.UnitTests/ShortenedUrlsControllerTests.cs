using Vivo.ApiService.Contracts;

namespace Vivo.ApiService.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Services;
using Moq;
using Shouldly;
using Controllers;

public class ShortenedUrlsControllerTests
{
    private readonly Mock<IShortenedUrlService> _serviceMock;
    private readonly ShortenedUrlsController _controller;

    public ShortenedUrlsControllerTests()
    {
        _serviceMock = new Mock<IShortenedUrlService>();
        _controller = new ShortenedUrlsController(_serviceMock.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.Request.Scheme = "https";
        httpContext.Request.Host = new HostString("localhost:5001");

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task GetRecentShortenedUrls_WhenUrlsExist_ShouldReturn200OkWithMappedResponses()
    {
        var cancellationToken = CancellationToken.None;
        var now = DateTime.UtcNow;
        var id = Guid.NewGuid();

        var urls = new List<ShortenedUrlDto>
        {
            new (id,"aaaaaaa", "https://wp.pl", now, 0)
        };

        _serviceMock
            .Setup(s => s.GetRecentShortenedUrlsAsync(cancellationToken))
            .ReturnsAsync(urls);

        var result = await _controller.GetRecentShortenedUrls(cancellationToken);

        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);

        _serviceMock.Verify(s => s.GetRecentShortenedUrlsAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetRecentShortenedUrls_WhenEmpty_ShouldReturn200OkWithEmptyList()
    {
        var cancellationToken = CancellationToken.None;

        List<ShortenedUrlDto> urls = [];

        _serviceMock
            .Setup(s => s.GetRecentShortenedUrlsAsync(cancellationToken))
            .ReturnsAsync(urls);

        var result = await _controller.GetRecentShortenedUrls(cancellationToken);

        var okResult = result.Result.ShouldBeOfType<OkObjectResult>();
        var items = okResult.Value.ShouldBeOfType<List<ShortenedUrlResponse>>();
        items.ShouldBeEmpty();
        okResult.StatusCode.ShouldBe(StatusCodes.Status200OK);

        _serviceMock.Verify(s => s.GetRecentShortenedUrlsAsync(cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenValidRequest_ShouldReturn201CreatedWithFullShortenedUrl()
    {
        var cancellationToken = CancellationToken.None;
        var originalUrl = "https://wp.pl";
        var code = "aaaaaaa";
        DateTime? expiresAt = null;

        var request = new CreateShortenedUrlRequest()
        {
            OriginalUrl = originalUrl,
            RequestedExpiresAt = expiresAt
        };
        var response = new CreateShortenedUrlResponse($"https://localhost:5001/{code}");

        _serviceMock
            .Setup(s => s.CreateShortUrlAsync(originalUrl, expiresAt, cancellationToken))
            .ReturnsAsync(code);

        var result = await _controller.CreateAsync(request, cancellationToken);

        var createdResult = result.Result.ShouldBeOfType<CreatedResult>();
        createdResult.StatusCode.ShouldBe(StatusCodes.Status201Created);
        createdResult.Value.ShouldBe(response);

        _serviceMock.Verify(s => s.CreateShortUrlAsync(originalUrl, expiresAt, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RedirectToOriginal_WhenCodeExistsAndValid_ShouldReturn302RedirectToOriginalUrl()
    {
        var cancellationToken = CancellationToken.None;
        var code = "aaaaaaa";
        var originalUrl = "https://wp.pl";

        _serviceMock.Setup(s => s.ResolveOriginalUrlAsync(code, cancellationToken))
            .ReturnsAsync(originalUrl);

        var result = await _controller.RedirectToOriginal(code, cancellationToken);

        var response = result.ShouldBeOfType<RedirectResult>();
        response.Url.ShouldBe(originalUrl);

        _serviceMock.Verify(s => s.ResolveOriginalUrlAsync(code, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RedirectToOriginal_WhenCodeNotFoundOrExpired_ShouldReturn404NotFound()
    {
        var cancellationToken = CancellationToken.None;
        var code = "aaaaaaa";
        string? nonexistentUrl = null;

        _serviceMock.Setup(s => s.ResolveOriginalUrlAsync(code, cancellationToken))
            .ReturnsAsync(nonexistentUrl);

        var result = await _controller.RedirectToOriginal(code, cancellationToken);

        var response = result.ShouldBeOfType<NotFoundResult>();
        response.StatusCode.ShouldBe(StatusCodes.Status404NotFound);

        _serviceMock.Verify(s => s.ResolveOriginalUrlAsync(code, cancellationToken), Times.Once);
    }
}