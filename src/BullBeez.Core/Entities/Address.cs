using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class Address : EntBaseAdvanced
    {
        public virtual County County { get; set; }
        public virtual CompanyAndPerson CompanyAndPersons { get;  set; }
        public virtual string AddressString { get; set; }

        public virtual string District { get; set; }

        public virtual string Locality { get; set; }

        public virtual string PostCode { get; set; }
        public virtual int AddressType { get; set; }
    }
}
