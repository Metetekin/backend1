using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class NewCommentRequest: BaseRequest
    {
        public int PostId { get; set; }
        public string Text { get; set; }
    }
}
