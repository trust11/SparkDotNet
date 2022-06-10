using System.Net;

namespace SparkDotNet.ExceptionHandling
{
    public class GenericOperationResult : IOperationResult
    {
        public virtual bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorDetail { get; set; }
        public SparkErrorContent Error { get; set; }

        public static GenericOperationResult Success => new GenericOperationResult { IsSuccess = true };

        public SparkApiOperationResultCode ResultCode { get; set; }

        public override string ToString()
        {
            if (IsSuccess)
            {
                return ResultCode.ToString();
            }

            return ErrorMessage + " " + (ErrorDetail ?? string.Empty);
        }
    }

    public class SparkApiConnectorApiOperationResult<T> : SparkApiConnectorApiOperationResult, IOperationResult<T>
    {
        public T Result { get; set; }

        public override string ToString()
        {
            if (IsSuccess)
                return IsSuccess.ToString();
            return Result.ToString();
        }
    }

    public class SparkApiConnectorApiOperationResult : GenericOperationResult, IOperationResult
    {
        public override bool IsSuccess
        {
            get
            {
                return ResultCode == SparkApiOperationResultCode.OK;
            }
            set
            {

            }
        }
        public override string ToString()
        {
            if (IsSuccess)
                return IsSuccess.ToString();
            return ToString();
        }
    }
}