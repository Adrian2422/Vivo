using Shouldly;
using Vivo.Domain.Exceptions;
using Vivo.Domain.ValueObjects;

namespace Vivo.Domain.UnitTests;

public class TargetUrlTests
{
    [Fact]
    public void Constructor_WhenValidHttpOrHttpsUrl_ShouldCreateInstance()
    {
        var url = "https://wp.pl";
        var targetUrl = new TargetUrl(url);

        targetUrl.ShouldBeOfType<TargetUrl>();
        targetUrl.Value.ShouldBe(url);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WhenValueIsNullOrWhiteSpace_ShouldThrowArgumentException(string? value)
    {
        var result = () => new TargetUrl(value!);
        result.ShouldThrow<InvalidOriginalUrlException>();
    }


    [Fact]
    public void Constructor_WhenValueIsNotWellFormedUrl_ShouldThrowArgumentException()
    {
        var url = "some-random-text";

        var result = () => new TargetUrl(url);
        result.ShouldThrow<InvalidOriginalUrlException>();
    }

    [Fact]
    public void Constructor_WhenSchemeIsNotHttpOrHttps_ShouldThrowArgumentException()
    {
        var url = "lorem://wp.pl";

        var result = () => new TargetUrl(url);
        result.ShouldThrow<InvalidOriginalUrlException>();
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnValue()
    {
        var url = "https://wp.pl";
        var targetUrl = new TargetUrl(url);

        var result = targetUrl.ToString();
        result.ShouldBe(url);
    }
}