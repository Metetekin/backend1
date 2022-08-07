using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class CompanyType : EntBaseAdvanced
    {
       
        public string Name { get; set; }
        public bool IsVenture { get; set; }
    }
}
