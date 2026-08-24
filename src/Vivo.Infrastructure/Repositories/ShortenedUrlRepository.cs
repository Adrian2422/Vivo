using Vivo.Infrastructure.Persistence.AppDbContext;

namespace Vivo.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;

public class ShortenedUrlRepository : IShortenedUrlRepository
{
    private readonly ApplicationDbContext _context;

    public ShortenedUrlRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<ShortenedUrlEntity>> GetRecentShortenedUrls(CancellationToken cancellationToken)
    {
        return await _context.ShortenedUrls.AsNoTracking().OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync(cancellationToken);
    }

    public async Task CreateAsync(ShortenedUrlEntity shortenedUrlEntity, CancellationToken cancellationToken)
    {
        await _context.ShortenedUrls.AddAsync(shortenedUrlEntity, cancellationToken);
    }
    
    public async Task<ShortenedUrlEntity?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await _context.ShortenedUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);
    }
}