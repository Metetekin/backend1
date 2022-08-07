using BullBeez.WebAdmin.PaymentApiCore.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BullBeez.WebAdmin.PaymentApiCore.Response
{
    /// <summary>
    /// Linkle Ödeme -> Link Gönderimi Servisi çıktı parametre alanları temsil eder.
    /// </summary> 

    public class LinkPaymentCreateResponse : BaseResponse
    {
        public string link { get; set; }
        public string linkPaymentId { get; set; }
        public string amount { get; set; }
    }
}
