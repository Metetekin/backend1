using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO
{
    public class ApiResult<T>
    {
        public string Message { get; set; }

        public string StatusCode { get; set; }
        public bool IsError { get; set; }

        public T Data { get; set; }
    }
}
