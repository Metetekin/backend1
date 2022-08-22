
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UserInfoByUsernameRequest : BaseRequest
    {
        public string UserName { get; set; }
    }
}
