using System.Net;

namespace SparkDotNet.ExceptionHandling
{
    public interface IOperationResult
    {
        bool IsSuccess { get; set; }

        string ErrorMessage { get; set; }

        string ErrorDetail { get; set; }

        SparkApiOperationResultCode ResultCode { get; set; }
    }

    public interface IOperationResult<T> : IOperationResult
    {
        T Result { get; set; }
    }
}