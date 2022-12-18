using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class OperationResult<T>
    {
        public T Result { get; }
        public Exception Exception { get; }
        public object ErrorObject { get; }
        public StatusCode Status { get; }

        public OperationResult(T result, StatusCode status = StatusCode.Success, Exception error = null, object errorObject = null)
        {
            Result = result;
            Exception = error;
            ErrorObject = errorObject;
            Status = status;
        }

        public enum StatusCode
        {
            Success,
            Error
        }
    }
}
