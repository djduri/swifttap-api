namespace SWIFTTAP.Domain.Base;

public class DomainException : Exception
{
    public string ExceptionCode { get; protected init; }

    protected DomainException(string message, string exceptionCode) : base(message)
    {
        ExceptionCode = exceptionCode;
    }

    public bool Is<TErrorCode>(TErrorCode errorCode) where TErrorCode : Enum
    {
        return ExceptionCode == ErrorCodeToExceptionCode(errorCode);
    }

    public static DomainException FromErrorCode<TErrorCode>(TErrorCode errorCode) where TErrorCode : Enum
    {
        var exceptionCodeValue = ErrorCodeToExceptionCode(errorCode);
        var exceptionMessage = string.Format("Invalid domain layer operation identified by: \"{0}\" occured.", exceptionCodeValue);

        return new DomainException(exceptionMessage, exceptionCodeValue);
    }

    private static string ErrorCodeToExceptionCode<TErrorCode>(TErrorCode errorCode) where TErrorCode : Enum
    {
        return string.Format("{0}_{1}", typeof(TErrorCode).Name, errorCode.ToString());
    }
}
