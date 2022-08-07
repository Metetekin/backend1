using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class ServiceAnswerRequest : BaseRequest
    {
        public int ServiceId { get; set; }
        public string ServiceAnswersString { get; set; }
        public int ContractConfirmation { get; set; }
        public List<ServiceAnswerModel> ServiceAnswers { get; set; }
    }

    public class ServiceAnswerModel
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public string Text { get; set; }
        public int AnswerType { get; set; }///1 select 2 text
    }
}

