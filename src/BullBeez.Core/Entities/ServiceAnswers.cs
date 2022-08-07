using BullBeez.Core.BaseEntities;

using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.Entities
{
    public class ServiceAnswers : EntBaseAdvanced
    {
        public virtual CompanyAndPerson CompanyAndPerson { get; set; }
        public int ServiceId { get; set; }
        public int QuestionoptionsId { get; set; }
        public string TextData { get; set; }
        public int IsPayment { get; set; }
        public Decimal Amount { get; set; }
        public Guid Guid { get; set; }
        public int ContractConfirmation { get; set; }
    }
}
