using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class CreatePackagePaymentRequest : BaseRequest
    {
        public int PackageId { get; set; }
        public decimal PaymentAmount { get; set; }
        public int IsPayment { get; set; }
        public int ContractConfirmation { get; set; }
    }
}
