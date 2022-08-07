﻿using BullBeez.WebAdmin.PaymentApiCore.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BullBeez.WebAdmin.Controllers
{
    public class BaseController : Controller
    {
        public Settings settings = new Settings
        {
            PublicKey = "ABWH03DXUDQ58V9", //"Public Magaza Anahtarı - size mağaza başvurunuz sonucunda gönderilen publik key (açık anahtar) bilgisini kullanınız.",
            PrivateKey = "ABWH03DXUDQ58V9HYW30N3MRX", //"Private Magaza Anahtarı  - size mağaza başvurunuz sonucunda gönderilen privaye key (gizli anahtar) bilgisini kullanınız.",
            BaseUrl = "https://api.ipara.com/", //iPara web servisleri API url'lerinin başlangıç bilgisidir. Restful web servis isteklerini takip eden kodlar halinde bulacaksınız.
                                                // Örneğin "https://api.ipara.com/" + "/rest/payment/auth"  = "https://api.ipara.com/rest/payment/auth"
            ThreeDInquiryUrl = "https://www.ipara.com/3dgate", // iPara 3D güvenlik sorgusun servisinin URL bilgisidir. Bakınız: IPara.DeveloperPortal.Core.Request#CreateThreeDPaymentForm(ThreeDPaymentInitRequest request, Settings options)
            Version = "1.0", // Kullandığınız iPara API versiyonudur.
            Mode = "P", // Test -> T, entegrasyon testlerinin sırasında "T" modunu, canlı sisteme entegre olarak ödeme almaya başlamak için ise Prod -> "P" modunu kullanınız.
            HashString = string.Empty // Kullanacağınız hash bilgisini, bağlanmak istediğiniz web servis bilgisine göre doldurulmalıdır. Bu bilgileri Entegrasyon rehberinin ilgili web servise ait bölümde bulabilirsiniz.
        };

}
}