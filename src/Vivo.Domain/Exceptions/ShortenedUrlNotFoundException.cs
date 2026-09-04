namespace Vivo.Domain.Exceptions;

public class ShortenedUrlNotFoundException(string code)
    : BaseException($"Shortened URL with code '{code}' was not found.", "SHORTENED_URL_NOT_FOUND", 404);