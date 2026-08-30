using Vivo.Application.DTOs;
using Vivo.Domain.Entities;

namespace Vivo.Application.UnitTests;

using Abstractions;
using Repositories;
using Services;
using Moq;
using Shouldly;

public class ShortenedUrlServiceTests
{
    private readonly Mock<IShortenedUrlRepository> _repositoryMock;
    private readonly Mock<IShortCodeGenerator> _codeGeneratorMock;
    private readonly ShortenedUrlService _service;

    public ShortenedUrlServiceTests()
    {
        _repositoryMock = new Mock<IShortenedUrlRepository>();
        _codeGeneratorMock = new Mock<IShortCodeGenerator>();
        _service = new ShortenedUrlService(_repositoryMock.Object, _codeGeneratorMock.Object);
    }

    [Trait("Category", "CreateShortUrlAsync")]
    [Fact]
    public async Task CreateShortUrlAsync_WhenValidUrlProvided_ShouldGenerateCodeSaveEntityAndReturnCode()
    {
        var originalUrl = "https://wp.pl";
        var code = "aaaaaaa";
        var cancellationToken = CancellationToken.None;
        var entity = new ShortenedUrlEntity()
        {
            Code = code,
            OriginalUrl = originalUrl
        };

        _codeGeneratorMock.Setup(g => g.Generate()).Returns(code);
        _repositoryMock
            .Setup(r => r.CreateAsync(entity, cancellationToken));

        var result = await _service.CreateShortUrlAsync(originalUrl, null, cancellationToken);

        result.ShouldBeOfType<String>();
        result.ShouldBe(code);
    }

    [Trait("Category", "CreateShortUrlAsync")]
    [Fact]
    public async Task CreateShortUrlAsync_WhenGeneratedCodeAlreadyExists_ShouldRegenerateCodeUntilUnique()
    {
        var originalUrl = "https://wp.pl";
        var existingCode = "aaaaaaa";
        var uniqueCode = "bbbbbbb";
        var cancellationToken = CancellationToken.None;

        _codeGeneratorMock
            .SetupSequence(g => g.Generate())
            .Returns(existingCode)
            .Returns(uniqueCode);

        _repositoryMock
            .Setup(r => r.CodeExistsAsync(existingCode, cancellationToken))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.CodeExistsAsync(uniqueCode, cancellationToken))
            .ReturnsAsync(false);

        var result = await _service.CreateShortUrlAsync(originalUrl, null, cancellationToken);

        result.ShouldBe(uniqueCode);

        _codeGeneratorMock.Verify(g => g.Generate(), Times.Exactly(2));
        _repositoryMock.Verify(r => r.CodeExistsAsync(existingCode, cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.CodeExistsAsync(uniqueCode, cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.CreateAsync(It.Is<ShortenedUrlEntity>(e => e.Code == uniqueCode && e.OriginalUrl == originalUrl), cancellationToken), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Trait("Category", "CreateShortUrlAsync")]
    [Fact]
    public async Task CreateShortUrlAsync_WhenInvalidUrlProvided_ShouldThrowArgumentException()
    {
        var invalidUrl = "invalid-url";
        var cancellationToken = CancellationToken.None;

        var result = () => _service.CreateShortUrlAsync(invalidUrl, null, cancellationToken);

        await result.ShouldThrowAsync<ArgumentException>();

        _repositoryMock.VerifyNoOtherCalls();
        _codeGeneratorMock.VerifyNoOtherCalls();
    }

    [Trait("Category", "ResolveOriginalUrlAsync")]
    [Fact]
    public async Task ResolveOriginalUrlAsync_WhenCodeExistsAndIsNotExpired_ShouldRegisterClickSaveChangesAndReturnOriginalUrl()
    {
        var code = "aaaaaaa";
        var originalUrl = "https://wp.pl";
        var cancellationToken = CancellationToken.None;
        var entity = new ShortenedUrlEntity()
        {
            Code = code,
            OriginalUrl = originalUrl
        };

        _repositoryMock
            .Setup(r => r.GetByCodeAsync(code, cancellationToken))
            .ReturnsAsync(entity);

        var result = await _service.ResolveOriginalUrlAsync(code, cancellationToken);

        result.ShouldBeOfType<string>();
        entity.ClickCount.ShouldBe(1);

        _repositoryMock.Verify(s => s.GetByCodeAsync(code, cancellationToken), Times.Once);
        _repositoryMock.Verify(s => s.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Trait("Category", "ResolveOriginalUrlAsync")]
    [Fact]
    public async Task ResolveOriginalUrlAsync_WhenCodeDoesNotExist_ShouldReturnNullAndNotSaveChanges()
    {
        var code = "aaaaaaa";
        var cancellationToken = CancellationToken.None;
        ShortenedUrlEntity? notFoundEntity = null;

        _repositoryMock
            .Setup(r => r.GetByCodeAsync(code, cancellationToken))
            .ReturnsAsync(notFoundEntity);

        var result = await _service.ResolveOriginalUrlAsync(code, cancellationToken);

        result.ShouldBe(null);

        _repositoryMock.Verify(s => s.GetByCodeAsync(code, cancellationToken), Times.Once);
        _repositoryMock.Verify(s => s.SaveChangesAsync(cancellationToken), Times.Never);
    }

    [Trait("Category", "ResolveOriginalUrlAsync")]
    [Fact]
    public async Task ResolveOriginalUrlAsync_WhenCodeIsExpired_ShouldReturnNullAndNotRegisterClick()
    {
        var code = "aaaaaaa";
        var originalUrl = "https://wp.pl";
        var cancellationToken = CancellationToken.None;
        var entity = new ShortenedUrlEntity()
        {
            Code = code,
            OriginalUrl = originalUrl,
            ExpiresAt = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1))
        };

        _repositoryMock
            .Setup(r => r.GetByCodeAsync(code, cancellationToken))
            .ReturnsAsync(entity);

        var result = await _service.ResolveOriginalUrlAsync(code, cancellationToken);

        result.ShouldBe(null);

        _repositoryMock.Verify(s => s.GetByCodeAsync(code, cancellationToken), Times.Once);
        _repositoryMock.Verify(s => s.SaveChangesAsync(cancellationToken), Times.Never);
    }

    [Trait("Category", "GetRecentShortenedUrlsAsync")]
    [Fact]
    public async Task GetRecentShortenedUrlsAsync_WhenUrlsExist_ShouldReturnMappedDtoList()
    {
        var cancellationToken = CancellationToken.None;
        var items = new List<ShortenedUrlEntity>()
        {
            new ShortenedUrlEntity()
            {
                Code = "aaaaaaa",
                OriginalUrl = "https://wp.pl"
            }
        };

        _repositoryMock
            .Setup(r => r.GetRecentShortenedUrls(cancellationToken))
            .ReturnsAsync(items);

        var result = await _service.GetRecentShortenedUrlsAsync(cancellationToken);

        result.ShouldBeOfType<List<ShortenedUrlDto>>();
        result.Count.ShouldBe(1);
    }

    [Trait("Category", "GetRecentShortenedUrlsAsync")]
    [Fact]
    public async Task GetRecentShortenedUrlsAsync_WhenNoUrlsExist_ShouldReturnEmptyList()
    {
        var cancellationToken = CancellationToken.None;
        List<ShortenedUrlEntity> items = [];

        _repositoryMock
            .Setup(r => r.GetRecentShortenedUrls(cancellationToken))
            .ReturnsAsync(items);

        var result = await _service.GetRecentShortenedUrlsAsync(cancellationToken);

        result.ShouldBeOfType<List<ShortenedUrlDto>>();
        result.Count.ShouldBe(0);
    }
}