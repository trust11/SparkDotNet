using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SparkDotNet.ExceptionHandling
{
    public static class SparkApiOperationResultMapper
    {
        internal static SparkApiOperationResultCode MapHttpStatusCode(HttpStatusCode code)
        {
            switch (code)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.Created:
                    return SparkApiOperationResultCode.OK;

                case HttpStatusCode.ServiceUnavailable:
                    return SparkApiOperationResultCode.ServiceUnavailable;

                case HttpStatusCode.BadRequest:
                    return SparkApiOperationResultCode.BadRequest;

                case HttpStatusCode.Forbidden:
                    return SparkApiOperationResultCode.Forbidden;

                case HttpStatusCode.Unauthorized:
                    return SparkApiOperationResultCode.Unauthorized;

                case HttpStatusCode.NotFound:
                    return SparkApiOperationResultCode.ObjectDoesNotExist;
            }
            return SparkApiOperationResultCode.OtherError;
        }
    }
}
