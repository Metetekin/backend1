using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.PaymentApiCore.Model
{
    public class Address
    {
        public int Id { get; set; }
        public virtual int CountyId { get; set; }
        public virtual int CityId { get; set; }
        public virtual string CountyName { get; set; }
        public virtual string AddressString { get; set; }

        public virtual string CityName { get; set; }
    }
}