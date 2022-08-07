using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UploadCvRequest : BaseRequest
    {
        public IFormFile File { get; set; }
    }
}
