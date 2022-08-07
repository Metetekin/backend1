using BullBeez.Core.DTO.MailDTO;

using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Helper
{
    public class MailHelper
    {
        //public async Task<string> SendMail(MailRequest mailRequest)
        //{
        //    string Body = System.IO.File.ReadAllText("./Validation.html");
        //    Body = Body.Replace("#CODE#", mailRequest.Code.ToString());


        //    string SMTPServer = "mail.bullbeez.co";
        //    string MailTo = mailRequest.EmailAddress;
        //    int port = 587;
        //    string username = "validation@bullbeez.co";
        //    string password = "Aytug1989.,";
        //    string mailFrom = "validation@bullbeez.co";


        //    MailMessage mail = new MailMessage();
        //    SmtpClient SmtpServer = new SmtpClient(SMTPServer);


        //    mail.From = new MailAddress(mailFrom);
        //    mail.To.Add(MailTo);
        //    mail.Subject = "Doğrulama Kodu";
        //    mail.Body = Body;
        //    mail.IsBodyHtml = true;


        //    SmtpServer.UseDefaultCredentials = false;
        //    SmtpServer.Port = port;
        //    SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
        //    SmtpServer.EnableSsl = false;


        //    try
        //    {
        //        SmtpServer.Send(mail);
        //    }
        //    catch (Exception e)
        //    {
        //        var uyari = e;
        //    }

        //    return "";
        //}

        public async Task<string> SendMail(MailRequest mailRequest)
        {
            string Body = System.IO.File.ReadAllText("./Validation.html");
            Body = Body.Replace("#CODE#", mailRequest.Code.ToString());

            SmtpClient client = new SmtpClient();
            client.Port = 587; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = "mail.bullbeezapi.co"; // Hostunuzun smtp için mail domaini.
            client.EnableSsl = false; // Güvenlik ayarları, host'a ve gönderilen server'a göre değişebilir.
            client.Timeout = 10000; // Milisaniye cinsten timeout
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Mailin yollanma methodu
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("validation@bullbeezapi.co", "Aytug1989.,"); // Burada hangi hesabı kullanarak mail yollayacaksanız onun ayarlarını yapmanız gerekiyor
            MailMessage mm = new MailMessage("validation@bullbeezapi.co", mailRequest.EmailAddress, "Doğrulama Kodu", Body); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
            mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
            mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure; // Hata olduğunda uyarı ver 
            client.Send(mm);

            return "";
        }

        public async Task<string> SendResetPasswordMail(MailRequest mailRequest)
        {
            string Body = System.IO.File.ReadAllText("./ResetPassword.html");
            Body = Body.Replace("#CODE#", mailRequest.Code.ToString());

            SmtpClient client = new SmtpClient();
            client.Port = 587; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = "mail.bullbeezapi.co"; // Hostunuzun smtp için mail domaini.
            client.EnableSsl = false; // Güvenlik ayarları, host'a ve gönderilen server'a göre değişebilir.
            client.Timeout = 10000; // Milisaniye cinsten timeout
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Mailin yollanma methodu
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@bullbeezapi.co", "Aytug1989.,"); // Burada hangi hesabı kullanarak mail yollayacaksanız onun ayarlarını yapmanız gerekiyor
            MailMessage mm = new MailMessage("info@bullbeezapi.co", mailRequest.EmailAddress, "Parola Yenileme", Body); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
            mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
            mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure; // Hata olduğunda uyarı ver 
            client.Send(mm);

            return "";
        }

        public async Task<string> SendMailGlobal(MailRequest mailRequest)
        {
            
            SmtpClient client = new SmtpClient();
            client.Port = 587; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = "mail.bullbeezapi.co"; // Hostunuzun smtp için mail domaini.
            client.EnableSsl = false; // Güvenlik ayarları, host'a ve gönderilen server'a göre değişebilir.
            client.Timeout = 10000; // Milisaniye cinsten timeout
            client.DeliveryMethod = SmtpDeliveryMethod.Network; // Mailin yollanma methodu
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@bullbeezapi.co", "Aytug1989.,"); // Burada hangi hesabı kullanarak mail yollayacaksanız onun ayarlarını yapmanız gerekiyor
            MailMessage mm = new MailMessage("info@bullbeezapi.co", "kocerabdulkadir@gmail.com", mailRequest.Subject, mailRequest.Body); // Hangi mail adresinden nereye, konu ve içerik mail ayarlarını yapabilirsiniz
            mm.To.Add("ahmetunat34@hotmail.com");
            mm.To.Add("bullbeezco@gmail.com");
            mm.IsBodyHtml = true; // True: Html olarak Gönderme, False: Text olarak Gönderme
            mm.BodyEncoding = UTF8Encoding.UTF8; // UTF8 encoding ayarı
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure; // Hata olduğunda uyarı ver 
            client.Send(mm);

            return "";
        }
    }
}
