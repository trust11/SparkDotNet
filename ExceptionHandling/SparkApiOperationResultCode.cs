using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkDotNet.ExceptionHandling
{
    public enum SparkApiOperationResultCode
    {
        OtherError, OK, BadRequest, Forbidden, ServiceUnavailable, InternalServerError, SessionDoesNotExist, ObjectDoesNotExist, Unauthorized
    }
}
