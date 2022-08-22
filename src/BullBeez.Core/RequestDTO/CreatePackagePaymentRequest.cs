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
        public string orderId { get; set; }
        public string packageName { get; set; }
        public string productId { get; set; }
        public string purchaseState { get; set; }
        public string purchaseToken { get; set; }
        public string quantity { get; set; }
        public string autoRenewing { get; set; }
        public string acknowledged { get; set; }
        public long originalTransactionDateIOS { get; set; }
        public string originalTransactionIdentifierIOS { get; set; }
        public string quantityIOS { get; set; }
        public long transactionDate { get; set; }
        public string transactionId { get; set; }
        public string transactionReceipt { get; set; }

    }
}
