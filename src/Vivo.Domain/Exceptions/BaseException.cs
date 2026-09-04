namespace Vivo.Domain.Exceptions;

public abstract class BaseException(string message, string errorCode, int statusCode = 400)
    : Exception(message)
{
    public string ErrorCode { get; } = errorCode;
    public int StatusCode { get; } = statusCode;
}