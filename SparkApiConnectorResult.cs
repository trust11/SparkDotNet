using GenericProvisioningLib;
using SparkDotNet.ExceptionHandling;

namespace SparkDotNet
{
    public class SparkApiConnectorApiOperationResult<T> : SparkApiConnectorApiOperationResult, IOperationResult<T>
    {
        public T Result { get; set; }

        public override string ToString()
        {
            var inhString = base.ToString();
            return $"{Result}\n{inhString}";
        }
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
            return $"IsSuccess:{IsSuccess}";
        }

        public new static SparkApiConnectorApiOperationResult Success => new SparkApiConnectorApiOperationResult 
        {
            IsSuccess = true,
            ResultCode = SparkApiOperationResultCode.OK
        };
    }
}
