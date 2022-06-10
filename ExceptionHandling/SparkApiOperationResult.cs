using System.Net;

namespace SparkDotNet.ExceptionHandling
{
    public class SparkApiOperationResult : GenericOperationResult
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
    }
}