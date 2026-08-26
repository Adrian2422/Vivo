using Bogus;
using Microsoft.EntityFrameworkCore;
using Vivo.Domain.Entities;
using Vivo.Infrastructure.Persistence.AppDbContext;

namespace Vivo.Infrastructure.Persistence;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;

    public DatabaseSeeder(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task SeedAsync()
    {
        if (await _context.ShortenedUrls.AnyAsync())
        {
            return;
        }

        var fixedUrl = new ShortenedUrlEntity()
        {
            Code = "lorem",
            OriginalUrl = "https://wp.pl",
            ExpiresAt = DateTime.UtcNow.AddYears(1)
        };

        var totalUrls = new Random().Next(10, 21);

        var urlsFaker = new Faker<ShortenedUrlEntity>()
            .RuleFor(x => x.OriginalUrl,
                f => $"{f.Internet.Protocol()}://{f.Internet.DomainName()}")
            .RuleFor(x => x.Code, f => f.Random.AlphaNumeric(8))
            .RuleFor(x => x.ExpiresAt, f => f.Date.Future(1).ToUniversalTime());
        
        var urls = urlsFaker.Generate(totalUrls);
        
        await _context.ShortenedUrls.AddAsync(fixedUrl);
        _context.ShortenedUrls.AddRange(urls);
        await _context.SaveChangesAsync();
    }
}