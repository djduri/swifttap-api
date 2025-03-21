namespace SWIFTTAP.Application.Exceptions;

public class EntityCreateException : CodedException
{
    private EntityCreateException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static EntityCreateException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new EntityCreateException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}
