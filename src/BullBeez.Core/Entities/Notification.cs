using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Notification : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public string Message { get; set; }
        public int IsShow { get; set; }
        public string ProfileImage { get; set; }
        public int ToUserId { get; set; }
        public int ProfileType { get; set; }
    }
}
