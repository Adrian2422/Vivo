using Shouldly;
using Vivo.Domain.ValueObjects;

namespace Vivo.Domain.UnitTests;

public class ShortCodeTests
{
    [Fact]
    public void Constructor_WhenValueIsValidAlphaNumeric_ShouldCreateInstance()
    {
        var code = "abc1234";
        var shortCode = new ShortCode(code);

        shortCode.ShouldBeOfType<ShortCode>();
        shortCode.Value.ShouldBe(code);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    public void Constructor_WhenValueIsNullOrWhiteSpace_ShouldThrowArgumentException(string? value)
    {
        var codeWithNull = () => new ShortCode(value);
        codeWithNull.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenLengthIsShorterThanFourCharacters_ShouldThrowArgumentException()
    {
        var shortCode = () => new ShortCode("123");
        shortCode.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void Constructor_WhenLengthIsLongerThanTwelveCharacters_ShouldThrowArgumentException()
    {
        var shortCode = () => new ShortCode("1234567890abcd");
        shortCode.ShouldThrow<ArgumentException>();
    }

    [Theory]
    [InlineData("123456$")]
    [InlineData("123 456")]
    public void Constructor_WhenValueContainsSpecialCharactersOrWhitespace_ShouldThrowArgumentException(string value)
    {
        var shortCode = () => new ShortCode(value);
        shortCode.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnValue()
    {
        var code = "abc1234";
        var shortCode = new ShortCode(code);
     
        var result = shortCode.ToString();
     
        result.ShouldBe(code);
        result.ShouldBeOfType<string>();
    }

    [Fact]
    public void Equals_WhenTwoInstancesHaveSameValue_ShouldBeEqual()
    {
        var shortCode1 = new ShortCode("abc1234");
        var shortCode2 = new ShortCode("abc1234");
        
        shortCode1.ShouldBeEquivalentTo(shortCode2);
        
    }
}