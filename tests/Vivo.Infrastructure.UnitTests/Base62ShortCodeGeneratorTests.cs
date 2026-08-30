using Shouldly;
using Vivo.Infrastructure.Services;

namespace Vivo.Infrastructure.UnitTests;

public class Base62ShortCodeGeneratorTests
{
    private readonly Base62ShortCodeGenerator _generator;
    
    public Base62ShortCodeGeneratorTests()
    {
        _generator = new Base62ShortCodeGenerator();
    }
    
    [Fact]
    public void Generate_WhenCalled_ShouldReturnStringWithSevenCharacters()
    {
        var result = _generator.Generate();

        result.ShouldNotBeNullOrWhiteSpace();
        result.Length.ShouldBe(7);
    }

    [Fact]
    public void Generate_WhenCalled_ShouldContainOnlyBase62Characters()
    {
        var result = _generator.Generate();

        result.ShouldMatch("^[0-9a-zA-Z]+$");
    }

    [Fact]
    public void Generate_WhenCalledMultipleTimes_ShouldReturnDifferentCodes()
    {
        const int count = 100;

        var codes = Enumerable.Range(0, count)
            .Select(_ => _generator.Generate())
            .ToList();

        codes.ShouldBeUnique();
    }
}