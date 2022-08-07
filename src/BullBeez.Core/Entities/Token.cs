using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Token : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public string DeviceId { get; set; }
        public string TokenId { get; set; }
        public string FirebaseToken { get; set; }
        public DateTime BeginData { get; set; }
        public DateTime? EndData { get; set; }
    }
}
