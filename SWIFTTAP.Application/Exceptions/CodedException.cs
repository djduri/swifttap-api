namespace SWIFTTAP.Application.Exceptions;

public class CodedException : Exception
{
    public string ExceptionCode { get; private init; }

    protected CodedException(string exceptionCode, string message) : base(message) =>
        ExceptionCode = exceptionCode;

    protected static string GetExceptionCode<TErrorCode>(TErrorCode errorCode) where TErrorCode : Enum =>
        string.Format("{0}_{1}", typeof(TErrorCode).Name, errorCode.ToString()).ToUpper();

    protected static string GetExceptionMessage<TErrorCode>(TErrorCode errorCode, string? message) where TErrorCode : Enum =>
            message ?? $"An error occurred with the code \"{errorCode}\". Please refer to this code for troubleshooting.";
}
