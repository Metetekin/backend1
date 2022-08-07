using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class CreatePayment : BaseRequest
    {
        public Guid CreatePaymentId { get; set; }
    }
}

