using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BullBeez.WebAdmin.ResponseDTO
{
    public class ServiceListResponse
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public Decimal Amount { get; set; }
        public Decimal OldAmount { get; set; }
        public string ServiceIcon { get; set; }
        public float DiscountPercentage { get; set; }
        public string Description { get; set; }
        public List<QuestionModel> QuestionList { get; set; }
    }


    public class QuestionModel
    {
        public int ServiceId { get; set; }
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public int QuestionType { get; set; }
        public List<OptionModel> OptionList { get; set; }
    }

    public class OptionModel
    {
        public int ServiceId2 { get; set; }
        public int QuestionId2 { get; set; }
        public int OptionId { get; set; }
        public string Option { get; set; }
    }
}