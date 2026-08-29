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

    [Fact]
    public void Constructor_WhenValueIsNullOrWhiteSpace_ShouldThrowArgumentException()
    {
        var codeWithNull = () => new ShortCode(null);
        codeWithNull.ShouldThrow<ArgumentException>();
        
        var codeWithSpace = () => new ShortCode("ab 1234");
        codeWithSpace.ShouldThrow<ArgumentException>();

        var emptyCode = () => new ShortCode("");
        emptyCode.ShouldThrow<ArgumentException>();
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

    [Fact]
    public void Constructor_WhenValueContainsSpecialCharactersOrWhitespace_ShouldThrowArgumentException()
    {
        var shortCode = () => new ShortCode("123456$");
        shortCode.ShouldThrow<ArgumentException>();
    }

    [Fact]
    public void ToString_WhenCalled_ShouldReturnValue()
    {
        var shortCode = new ShortCode("abc1234").ToString();
        
        shortCode.ShouldBeOfType<string>();
        shortCode.ShouldBe("abc1234");
    }

    [Fact]
    public void Equals_WhenTwoInstancesHaveSameValue_ShouldBeEqual()
    {
        var shortCode1 = new ShortCode("abc1234");
        var shortCode2 = new ShortCode("abc1234");
        
        shortCode1.ShouldBeEquivalentTo(shortCode2);
        
    }
}