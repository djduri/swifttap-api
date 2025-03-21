namespace SWIFTTAP.Application.Exceptions;
public class AuthenticationException : CodedException
{
    private AuthenticationException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static AuthenticationException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new AuthenticationException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}
