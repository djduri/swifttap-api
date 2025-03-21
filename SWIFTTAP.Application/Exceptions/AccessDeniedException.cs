namespace SWIFTTAP.Application.Exceptions;
public class AccessDeniedException : CodedException
{
    private AccessDeniedException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static AccessDeniedException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new AccessDeniedException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}
