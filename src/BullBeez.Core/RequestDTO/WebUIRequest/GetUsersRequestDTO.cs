﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO.WebUIRequest
{
    public class GetUsersRequestDTO
    {
        public int take { get; set; }
        public int skip { get; set; }
        public string search { get; set; }
    }
}
