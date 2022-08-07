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
    /// Linkle Ödeme -> Link Silme Servisi çıktı parametre alanları temsil eder.
    /// </summary>
    public class LinkPaymentDeleteResponse : BaseResponse
    {
        public List<PaymentLink> linkPaymentRecordList { get; set; }

        public string pageIndex { get; set; }
        public string pageSize { get; set; }
        public string pageCount { get; set; }

    }
}
