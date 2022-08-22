using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class DeleteByIdRequest : BaseRequest
    {
        public int Id { get; set; }
    }
}
