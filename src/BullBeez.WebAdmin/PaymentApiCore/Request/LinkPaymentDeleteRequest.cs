﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;

using BullBeez.WebAdmin.PaymentApiCore.Base;
using BullBeez.WebAdmin.PaymentApiCore.Response;

using Newtonsoft.Json;

namespace BullBeez.WebAdmin.PaymentApiCore.Request
{
    /// <summary>
    /// Linkle Ödeme -> Link Silme Servisi içerisinde kullanılacak alanları temsil eder.
    /// </summary>
    public class LinkPaymentDeleteRequest : BaseRequest
    {
        public string linkId { get; set; }
        public string clientIp { get; set; }
        public static LinkPaymentDeleteResponse Execute(LinkPaymentDeleteRequest request, Settings options)
        {
            options.TransactionDate = Helper.GetTransactionDateString();
            options.HashString = options.PrivateKey + request.clientIp + options.TransactionDate;
            LinkPaymentDeleteResponse response = RestHttpCaller.Create().PostJson<LinkPaymentDeleteResponse>(options.BaseUrl + "corporate/merchant/linkpayment/delete", Helper.GetHttpHeaders(options, Helper.application_json), request);
            return response;
        }
    }
}

