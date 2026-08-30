using Shouldly;
using Vivo.Domain.Entities;

namespace Vivo.Domain.UnitTests;

public class ShortenedUrlEntityTests
{
    [Fact]
    public void Create_WhenValidParametersProvided_ShouldInitializeEntityWithCorrectState()
    {
        var originalUrl = "https://wp.pl";
        var code = "abc1234";

        var entity = new ShortenedUrlEntity()
        {
            OriginalUrl = originalUrl,
            Code = code
        };

        entity.OriginalUrl.ShouldBe(originalUrl);
        entity.Code.ShouldBe(code);
        entity.ClickCount.ShouldBe(0);
        entity.ExpiresAt.ShouldBe(null);
    }

    [Fact]
    public void RegisterClick_WhenCalled_ShouldIncrementClickCountByOne()
    {
        var originalUrl = "https://wp.pl";
        var code = "abc1234";

        var entity = new ShortenedUrlEntity()
        {
            OriginalUrl = originalUrl,
            Code = code
        };
        entity.ClickCount.ShouldBe(0);
        entity.RegisterClick();
        entity.ClickCount.ShouldBe(1);
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsNull_ShouldReturnFalse()
    {
        var originalUrl = "https://wp.pl";
        var code = "abc1234";

        var entity = new ShortenedUrlEntity()
        {
            OriginalUrl = originalUrl,
            Code = code
        };

        entity.IsExpired.ShouldBe(false);
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsInFuture_ShouldReturnFalse()
    {
        var originalUrl = "https://wp.pl";
        var code = "abc1234";
        var expiresInOneHour = DateTime.UtcNow.Add(TimeSpan.FromHours(1));

        var entity = new ShortenedUrlEntity()
        {
            OriginalUrl = originalUrl,
            Code = code,
            ExpiresAt = expiresInOneHour
        };

        entity.IsExpired.ShouldBe(false);
    }

    [Fact]
    public void IsExpired_WhenExpiresAtIsInPast_ShouldReturnTrue()
    {
        var originalUrl = "https://wp.pl";
        var code = "abc1234";
        var hourAgoFromNow = DateTime.UtcNow.Subtract(TimeSpan.FromHours(1));

        var entity = new ShortenedUrlEntity()
        {
            OriginalUrl = originalUrl,
            Code = code,
            ExpiresAt = hourAgoFromNow
        };

        entity.IsExpired.ShouldBe(true);
    }

}