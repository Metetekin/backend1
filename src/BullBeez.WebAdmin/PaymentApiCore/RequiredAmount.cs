using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.WebAdmin.PaymentApiCore
{
    public class RequiredAmount
    {
        
        public string requiredAmount { get; set; }
        public string requiredCommissionAmount { get; set; }
        public string installment { get; set; }
        public string merchantCommissionRate { get; set; }
    }
}
