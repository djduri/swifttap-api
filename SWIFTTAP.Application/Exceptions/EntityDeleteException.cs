namespace SWIFTTAP.Application.Exceptions;

public class EntityDeleteException : CodedException
{
    private EntityDeleteException(string exceptionCode, string message) : base(exceptionCode, message)
    {
    }

    public static EntityDeleteException FromErrorCode<TErrorCode>(TErrorCode errorCode, string? message = null) where TErrorCode : Enum =>
        new EntityDeleteException(GetExceptionCode(errorCode), GetExceptionMessage(errorCode, message));
}