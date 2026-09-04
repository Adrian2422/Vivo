namespace Vivo.Domain.Exceptions;

public class InvalidOriginalUrlException(string message, string errorCode = "INVALID_ORIGINAL_URL")
    : BaseException(message, errorCode, 400);