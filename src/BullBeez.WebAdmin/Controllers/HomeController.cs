using BullBeez.WebAdmin.PaymentApiCore;
using BullBeez.WebAdmin.PaymentApiCore.Base;
using BullBeez.WebAdmin.PaymentApiCore.Model;
using BullBeez.WebAdmin.PaymentApiCore.Request;
using BullBeez.WebAdmin.PaymentApiCore.Response;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BullBeez.WebAdmin.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OdemeSayfasi(int serviceId, int userId, string serviceInsertId)
        {
            try
            {
                CardDetailModel response = new CardDetailModel();
                var DB = new db();
                response = DB.GetUser(userId, serviceId);
                response.CreatePaymentId = new Guid(serviceInsertId);
                return View(response);
            }
            catch (Exception ex)
            {
                CardDetailModel response = new CardDetailModel();
                response.ServiceName = JsonConvert.SerializeObject(ex);
                return View(response);
            }

        }

        [HttpGet]
        public ActionResult PaketOdemeSayfasi(int packageId, int userId)
        {
            try
            {
                CardDetailModel response = new CardDetailModel();
                var DB = new db();
                response = DB.GetUserPackage(userId, packageId);
                return View(response);
            }
            catch (Exception ex)
            {
                CardDetailModel response = new CardDetailModel();
                response.ServiceName = JsonConvert.SerializeObject(ex);
                return View(response);
            }

        }

        public ActionResult OdemeYap(string nameSurname, string cardNumber, string cvc, string monthAndYear, string userId, string cardId, string installment, string kontrol, int serviceId, Guid? createPaymentId, bool odemeOnaySecimi,int hizmetTipi)//hizmet tipi 1  servis ödemesi 2 paket alımı
        {
            var DB = new db();
            //3d iki aşamalı bir işlemdir. İlk adımda 3D güvenlik sorgulaması yapılmalıdır. 
            CardDetailModel response = new CardDetailModel();
            if (hizmetTipi == 1)
            {
                response = DB.GetUser(Convert.ToInt32(userId), serviceId);
            }
            else
            {
                response = DB.GetUserPackage(Convert.ToInt32(userId), serviceId);
            }
            response.CreatePaymentId = createPaymentId.Value;
            response.ContractConfirmation = odemeOnaySecimi;

            var request = new ThreeDPaymentInitRequest();
            request.OrderId = Guid.NewGuid().ToString();
            request.Echo = "Echo";
            request.Mode = settings.Mode;
            request.Version = settings.Version;
            request.Amount = response.Amount.ToString().Replace(",", "");
            request.CardOwnerName = nameSurname;
            request.CardNumber = cardNumber.Replace(" ", "");
            request.CardExpireMonth = monthAndYear.Split('/')[0].Trim();
            request.CardExpireYear = monthAndYear.Split('/')[1].Trim();
            request.Installment = "1";
            request.Cvc = cvc;
            request.CardId = cardId;
            request.UserId = userId;



            request.PurchaserName = response.NameOrTitle.Split(' ')[0];
            request.PurchaserSurname = response.NameOrTitle.Split(' ')[1];
            request.PurchaserEmail = response.EmailAddress;

            var successUrl = hizmetTipi == 1 ? "Home/ThreeDResultSuccess" : "Home/ThreeDResultSuccessPackage";
            request.SuccessUrl = "https://api.bullbeezapi.co/" + successUrl;
            request.FailUrl = "https://api.bullbeezapi.co/" + "Home/ThreeDResultFail";

            DB.InsertLog("", request.OrderId, Newtonsoft.Json.JsonConvert.SerializeObject(response), Newtonsoft.Json.JsonConvert.SerializeObject(request));

            var form = ThreeDPaymentInitRequest.Execute(request, settings);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write(form);
            System.Web.HttpContext.Current.Response.End();
            return View();
        }


        public ActionResult ThreeDResultSuccess()
        {
            var DB = new db();

            ThreeDPaymentInitResponse paymentResponse = new ThreeDPaymentInitResponse();
            paymentResponse.OrderId = Request.Form["orderId"];
            paymentResponse.Result = Request.Form["result"];
            paymentResponse.Amount = Request.Form["amount"];
            paymentResponse.Mode = Request.Form["mode"];



            if (Request.Form["errorCode"] != null)
                paymentResponse.ErrorCode = Request.Form["errorCode"];

            if (Request.Form["errorMessage"] != null)
                paymentResponse.ErrorMessage = Request.Form["errorMessage"];

            if (Request.Form["transactionDate"] != null)
                paymentResponse.TransactionDate = Request.Form["transactionDate"];

            if (Request.Form["hash"] != null)
                paymentResponse.Hash = Request.Form["hash"];


            var log = DB.GetLog(paymentResponse.OrderId);
            var cardDetailModel = JsonConvert.DeserializeObject<CardDetailModel>(log.UserDetail);
            var threeDPaymentInitRequest = JsonConvert.DeserializeObject<ThreeDPaymentInitRequest>(log.RequestDetail);
            var createPaymentId = cardDetailModel.CreatePaymentId;

            var deleteLog = DB.DeleteLog(paymentResponse.OrderId);
            threeDPaymentInitRequest.CardNumber = threeDPaymentInitRequest.CardNumber.Substring(0, 4) + "XXXXXXXX" + threeDPaymentInitRequest.CardNumber.Substring(12, 4);
            DB.InsertLog("", paymentResponse.OrderId, Newtonsoft.Json.JsonConvert.SerializeObject(cardDetailModel), Newtonsoft.Json.JsonConvert.SerializeObject(threeDPaymentInitRequest));



            if (Helper.Validate3DReturn(paymentResponse, settings))
            {

                var request = new ThreeDPaymentCompleteRequest();

                #region Request New
                request.OrderId = Request.Form["orderId"];
                request.Echo = "Echo";
                request.Mode = "P";
                request.Amount = cardDetailModel.Amount.ToString().Replace(",", "");// 100 tL
                request.CardOwnerName = threeDPaymentInitRequest.CardOwnerName;
                request.CardNumber = threeDPaymentInitRequest.CardNumber;
                request.CardExpireMonth = threeDPaymentInitRequest.CardExpireMonth;
                request.CardExpireYear = threeDPaymentInitRequest.CardExpireYear;
                request.Installment = threeDPaymentInitRequest.Installment;
                request.Cvc = threeDPaymentInitRequest.Cvc;
                request.ThreeD = "true";
                request.ThreeDSecureCode = Request.Form["threeDSecureCode"];
                #endregion

                #region Sipariş veren bilgileri
                request.Purchaser = new Purchaser();
                request.Purchaser.BirthDate = "";
                request.Purchaser.GsmPhone = cardDetailModel.GSM;
                request.Purchaser.IdentityNumber = "";
                #endregion

                #region Fatura bilgileri
                request.Purchaser.InvoiceAddress = new PurchaserAddress();
                request.Purchaser.InvoiceAddress.Name = cardDetailModel.NameOrTitle.Split(' ')[0];
                request.Purchaser.InvoiceAddress.SurName = cardDetailModel.NameOrTitle.Split(' ')[1];
                request.Purchaser.InvoiceAddress.Address = cardDetailModel.Address.AddressString;
                request.Purchaser.InvoiceAddress.ZipCode = "";
                request.Purchaser.InvoiceAddress.CityCode = cardDetailModel.Address.CityId.ToString();
                request.Purchaser.InvoiceAddress.IdentityNumber = "";
                request.Purchaser.InvoiceAddress.CountryCode = "TR";
                request.Purchaser.InvoiceAddress.TaxNumber = "";
                request.Purchaser.InvoiceAddress.TaxOffice = "";
                request.Purchaser.InvoiceAddress.CompanyName = "iPara";
                request.Purchaser.InvoiceAddress.PhoneNumber = "";
                #endregion

                #region Kargo Adresi bilgileri
                request.Purchaser.ShippingAddress = new PurchaserAddress();
                request.Purchaser.ShippingAddress.Name = cardDetailModel.NameOrTitle.Split(' ')[0];
                request.Purchaser.ShippingAddress.SurName = cardDetailModel.NameOrTitle.Split(' ')[1];
                request.Purchaser.ShippingAddress.Address = cardDetailModel.Address.AddressString;
                request.Purchaser.ShippingAddress.ZipCode = "";
                request.Purchaser.ShippingAddress.CityCode = cardDetailModel.Address.CityId.ToString();
                request.Purchaser.ShippingAddress.IdentityNumber = "";
                request.Purchaser.ShippingAddress.CountryCode = "TR";
                request.Purchaser.ShippingAddress.PhoneNumber = "";
                #endregion

                #region Ürün bilgileri
                request.Products = new List<Product>();
                Product p = new Product();
                p.Title = cardDetailModel.ServiceName;
                p.Code = cardDetailModel.ServiceId.ToString();
                p.Price = cardDetailModel.Amount.ToString().Replace(",", "");
                p.Quantity = 1;
                request.Products.Add(p);
                #endregion

                var response = ThreeDPaymentCompleteRequest.Execute(request, settings);
                DB.InsertLog("Başarı :" + response);

                SendMail(cardDetailModel.EmailAddress);
                SendMailNotification("Bir kişi için ödeme yapılmıştır. Ödeme yapan kişi adı : " + cardDetailModel.NameOrTitle + ". Kişi id : " + cardDetailModel.UserId);
                DB.ServiceAnswer(createPaymentId.Value, 2);
                return View(response);

            }
            else
            {
                return RedirectToAction("ThreeDResultFail");
            }
        }

        public ActionResult ThreeDResultSuccessPackage()
        {
            var DB = new db();

            ThreeDPaymentInitResponse paymentResponse = new ThreeDPaymentInitResponse();
            paymentResponse.OrderId = Request.Form["orderId"];
            paymentResponse.Result = Request.Form["result"];
            paymentResponse.Amount = Request.Form["amount"];
            paymentResponse.Mode = Request.Form["mode"];



            if (Request.Form["errorCode"] != null)
                paymentResponse.ErrorCode = Request.Form["errorCode"];

            if (Request.Form["errorMessage"] != null)
                paymentResponse.ErrorMessage = Request.Form["errorMessage"];

            if (Request.Form["transactionDate"] != null)
                paymentResponse.TransactionDate = Request.Form["transactionDate"];

            if (Request.Form["hash"] != null)
                paymentResponse.Hash = Request.Form["hash"];


            var log = DB.GetLog(paymentResponse.OrderId);
            var cardDetailModel = JsonConvert.DeserializeObject<CardDetailModel>(log.UserDetail);
            var threeDPaymentInitRequest = JsonConvert.DeserializeObject<ThreeDPaymentInitRequest>(log.RequestDetail);
            var createPaymentId = cardDetailModel.CreatePaymentId;

            var deleteLog = DB.DeleteLog(paymentResponse.OrderId);
            threeDPaymentInitRequest.CardNumber = threeDPaymentInitRequest.CardNumber.Substring(0, 4) + "XXXXXXXX" + threeDPaymentInitRequest.CardNumber.Substring(12, 4);
            DB.InsertLog("", paymentResponse.OrderId, Newtonsoft.Json.JsonConvert.SerializeObject(cardDetailModel), Newtonsoft.Json.JsonConvert.SerializeObject(threeDPaymentInitRequest));



            if (Helper.Validate3DReturn(paymentResponse, settings))
            {

                var request = new ThreeDPaymentCompleteRequest();

                #region Request New
                request.OrderId = Request.Form["orderId"];
                request.Echo = "Echo";
                request.Mode = "P";
                request.Amount = cardDetailModel.Amount.ToString().Replace(",", "");// 100 tL
                request.CardOwnerName = threeDPaymentInitRequest.CardOwnerName;
                request.CardNumber = threeDPaymentInitRequest.CardNumber;
                request.CardExpireMonth = threeDPaymentInitRequest.CardExpireMonth;
                request.CardExpireYear = threeDPaymentInitRequest.CardExpireYear;
                request.Installment = threeDPaymentInitRequest.Installment;
                request.Cvc = threeDPaymentInitRequest.Cvc;
                request.ThreeD = "true";
                request.ThreeDSecureCode = Request.Form["threeDSecureCode"];
                #endregion

                #region Sipariş veren bilgileri
                request.Purchaser = new Purchaser();
                request.Purchaser.BirthDate = "";
                request.Purchaser.GsmPhone = cardDetailModel.GSM;
                request.Purchaser.IdentityNumber = "";
                #endregion

                #region Fatura bilgileri
                request.Purchaser.InvoiceAddress = new PurchaserAddress();
                request.Purchaser.InvoiceAddress.Name = cardDetailModel.NameOrTitle.Split(' ')[0];
                request.Purchaser.InvoiceAddress.SurName = cardDetailModel.NameOrTitle.Split(' ')[1];
                request.Purchaser.InvoiceAddress.Address = cardDetailModel.Address.AddressString;
                request.Purchaser.InvoiceAddress.ZipCode = "";
                request.Purchaser.InvoiceAddress.CityCode = cardDetailModel.Address.CityId.ToString();
                request.Purchaser.InvoiceAddress.IdentityNumber = "";
                request.Purchaser.InvoiceAddress.CountryCode = "TR";
                request.Purchaser.InvoiceAddress.TaxNumber = "";
                request.Purchaser.InvoiceAddress.TaxOffice = "";
                request.Purchaser.InvoiceAddress.CompanyName = "iPara";
                request.Purchaser.InvoiceAddress.PhoneNumber = "";
                #endregion

                #region Kargo Adresi bilgileri
                request.Purchaser.ShippingAddress = new PurchaserAddress();
                request.Purchaser.ShippingAddress.Name = cardDetailModel.NameOrTitle.Split(' ')[0];
                request.Purchaser.ShippingAddress.SurName = cardDetailModel.NameOrTitle.Split(' ')[1];
                request.Purchaser.ShippingAddress.Address = cardDetailModel.Address.AddressString;
                request.Purchaser.ShippingAddress.ZipCode = "";
                request.Purchaser.ShippingAddress.CityCode = cardDetailModel.Address.CityId.ToString();
                request.Purchaser.ShippingAddress.IdentityNumber = "";
                request.Purchaser.ShippingAddress.CountryCode = "TR";
                request.Purchaser.ShippingAddress.PhoneNumber = "";
                #endregion

                #region Ürün bilgileri
                request.Products = new List<Product>();
                Product p = new Product();
                p.Title = cardDetailModel.ServiceName;
                p.Code = cardDetailModel.ServiceId.ToString();
                p.Price = cardDetailModel.Amount.ToString().Replace(",", "");
                p.Quantity = 1;
                request.Products.Add(p);
                #endregion

                var response = ThreeDPaymentCompleteRequest.Execute(request, settings);
                DB.InsertLog("Başarı :" + response);

                SendMail(cardDetailModel.EmailAddress);
                SendMailNotification("Bir kişi için ödeme yapılmıştır. Ödeme yapan kişi adı : " + cardDetailModel.NameOrTitle + ". Kişi id : " + cardDetailModel.UserId);
                DB.InsertPackagePayments(cardDetailModel.UserId, cardDetailModel.ServiceId, 1, cardDetailModel.Amount, 2);//2 ödeme alındı demek
                return View(response);

            }
            else
            {
                return RedirectToAction("ThreeDResultFail");
            }
        }

        public ActionResult ThreeDResultFail()
        {
            var DB = new db();
            ThreeDPaymentInitResponse response = new ThreeDPaymentInitResponse();
            response.OrderId = Request.Form["orderId"];
            response.Result = Request.Form["result"];
            response.Amount = Request.Form["amount"];
            response.Mode = Request.Form["mode"];
            if (Request.Form["errorCode"] != null)
                response.ErrorCode = Request.Form["errorCode"];

            if (Request.Form["errorMessage"] != null)
                response.ErrorMessage = Request.Form["errorMessage"];

            if (Request.Form["transactionDate"] != null)
                response.TransactionDate = Request.Form["transactionDate"];

            if (Request.Form["hash"] != null)
                response.Hash = Request.Form["hash"];
            var a = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            DB.InsertLog("Hata :" + a);
            return View(response);
        }

        [HttpGet]
        public ActionResult ServisPage(int servisId, int userId)
        {
            var DB = new db();
            var response = DB.GetUser(userId, servisId);
            return View(response);
        }


        public void SendMail(string toMail)
        {
            string Body = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/ServiceMail.html"));

            SmtpClient client = new SmtpClient();
            client.Port = 587; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = "mail.bullbeezapi.co"; // Hostunuzun smtp için mail domaini.
            client.EnableSsl = false; // Güvenlik ayarları, host'a ve gönderilen server'a göre değişebilir.
            client.Timeout = 10000; // Milisaniye cinsten timeout
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Mailin yollanma methodu
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@bullbeezapi.co", "Aytug1989.,"); // Burada hangi hesabı kullanarak mail yollayacaksanız onun ayarlarını yapmanız gerekiyor
            MailMessage mm = new MailMessage("info@bullbeezapi.co", toMail, "Ödeme Bilgilendirme", Body); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
            mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
            mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure; // Hata olduğunda uyarı ver 
            client.Send(mm); // Mail yolla
        }

        public void SendMailNotification(string body)
        {
            SmtpClient client = new SmtpClient();
            client.Port = 587; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = "mail.bullbeezapi.co"; // Hostunuzun smtp için mail domaini.
            client.EnableSsl = false; // Güvenlik ayarları, host'a ve gönderilen server'a göre değişebilir.
            client.Timeout = 10000; // Milisaniye cinsten timeout
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Mailin yollanma methodu
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@bullbeezapi.co", "Aytug1989.,"); // Burada hangi hesabı kullanarak mail yollayacaksanız onun ayarlarını yapmanız gerekiyor
            MailMessage mm = new MailMessage("info@bullbeezapi.co", "kocerabdulkadir@gmail.com", "Ödeme Bilgilendirme", body); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
            mm.To.Add("ahmetunat34@hotmail.com");
            mm.To.Add("bullbeezco@gmail.com");
            mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
            mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure; // Hata olduğunda uyarı ver 
            client.Send(mm); // Mail yolla
        }
    }
}