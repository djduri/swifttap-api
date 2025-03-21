namespace SWIFTTAP.Application.Exceptions;

public class AuthorizationException : CodedException
{
    private AuthorizationException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static AuthorizationException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new AuthorizationException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}
