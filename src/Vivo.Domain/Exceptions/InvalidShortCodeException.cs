namespace Vivo.Domain.Exceptions;

public class InvalidShortCodeException(string message, string errorCode = "INVALID_SHORT_CODE")
    : BaseException(message, errorCode, 400);