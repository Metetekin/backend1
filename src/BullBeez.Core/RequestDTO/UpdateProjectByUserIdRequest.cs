using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UpdateProjectByUserIdRequest : BaseRequest
    {
        public string ProjectListStr { get; set; }
    }
}
