using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UploadImageRequest : BaseRequest
    {
        public string Base64Image { get; set; }
    }
}
