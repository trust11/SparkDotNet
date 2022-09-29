using GenericProvisioningLib;
using SparkDotNet.ExceptionHandling;

namespace SparkDotNet
{
    public class SparkApiConnectorApiOperationResult<T> : SparkApiConnectorApiOperationResult, IOperationResult<T>
    {
        public T Result { get; set; }

        public string NextLink { get; set; }

        public override string ToString()
        {
            var inhString = base.ToString();
            return $"{Result}\n{inhString}";
        }

        public new static SparkApiConnectorApiOperationResult<T> Success => new SparkApiConnectorApiOperationResult<T>
        {
            IsSuccess = true,
            ResultCode = SparkApiOperationResultCode.OK
        };

        public static SparkApiConnectorApiOperationResult<T> SuccessResult(T value) => new SparkApiConnectorApiOperationResult<T>
        {
            Result = value,
            IsSuccess = true,
            ResultCode = SparkApiOperationResultCode.OK
        };
    }

    public class SparkApiConnectorApiOperationResult : GenericOperationResult
    {
        public SparkErrorContent Error { get; set; }

        public override bool IsSuccess
        {
            get
            {
                return ResultCode == SparkApiOperationResultCode.OK;
            }
        }

        public SparkApiOperationResultCode ResultCode { get; set; }

        public override string ToString()
        {
            if (IsSuccess)
                return $"Success";
            else
            {
                return $"{Error}";
            }
        }

        public new static SparkApiConnectorApiOperationResult Success => new SparkApiConnectorApiOperationResult 
        {
            IsSuccess = true,
            ResultCode = SparkApiOperationResultCode.OK
        };
    }
}
