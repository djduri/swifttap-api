namespace SWIFTTAP.Application.Exceptions;

public class EntityUpdateException : CodedException
{
    private EntityUpdateException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static EntityUpdateException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new EntityUpdateException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}