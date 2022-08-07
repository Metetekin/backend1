using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class PackagePayments : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public int PackageId { get; set; }
        public string TextData { get; set; }
        public int IsPayment { get; set; }
        public decimal Amount { get; set; }
        public Guid Guid { get; set; }
        public int ContractConfirmation { get; set; }
    }
}
