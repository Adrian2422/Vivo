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
    public async Task GetRecentShortenedUrls_ShouldReturnOk_WithMappedResponses()
    {
        var cancellationToken = CancellationToken.None;
        var now = DateTime.UtcNow;
        
        var urls = new List<ShortenedUrlDto>
        {
            new ("aaaaaaa", "https://wp.pl", now, 0)
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
    public async Task CreateAsync_ShouldReturnCreated_WithShortUrl()
    {
        var cancellationToken = CancellationToken.None;
        var originalUrl = "https://wp.pl";
        var code = "aaaaaaa";
        DateTime? expiresAt = null;
        var request = new CreateShortenedUrlRequest(originalUrl, expiresAt);
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
}