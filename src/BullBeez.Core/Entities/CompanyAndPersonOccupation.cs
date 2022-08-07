using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class CompanyAndPersonOccupation : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public int ComponyAndPersonId { get; set; }
        public int OccupationId { get; set; }
        public virtual Occupation Occupation { get; set; }

        public CompanyAndPersonOccupation()
        {
        }

        public CompanyAndPersonOccupation(
            CompanyAndPerson companyAndPerson,
            Occupation occupation)
        {
            this.CompanyAndPerson = companyAndPerson;
            this.Occupation = occupation;

        }
    }
}