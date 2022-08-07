using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class RequestHistory : EntBaseAdvanced
    {
        public int UserId { get; set; }
        public string JsonRequest { get; set; }
        public string FunctionName { get; set; }
    }
}
