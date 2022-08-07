using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class MailApprovedCode : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public int Code { get; set; }
        public string EmailAddress { get; set; }
    }
}
