namespace SWIFTTAP.Application.Exceptions;

public class EntityNotFoundException : CodedException
{
    private EntityNotFoundException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static EntityNotFoundException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new EntityNotFoundException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}