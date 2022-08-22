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
        public string host = "185.210.94.60";
        public int port = 587;
        //public string host = "mail.bullbeezapi.co";
        //public int port = 465;


        public async Task<string> SendMail(MailRequest mailRequest)
        {
            try
            {
                string Body = System.IO.File.ReadAllText("./Validation.html");
                Body = Body.Replace("#CODE#", mailRequest.Code.ToString());

                SmtpClient client = new SmtpClient();
                client.Port = port; // Genelde 587 ve 25 portları kullanılmaktadır.
                client.Host = host; // Hostunuzun smtp için mail domaini.
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
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            

            return "Hata Yok";
        }

        public async Task<string> SendResetPasswordMail(MailRequest mailRequest)
        {
            string Body = System.IO.File.ReadAllText("./ResetPassword.html");
            Body = Body.Replace("#CODE#", mailRequest.Code.ToString());

            SmtpClient client = new SmtpClient();
            client.Port = port; // Genelde 587 ve 25 portları kullanılmaktadır.
            client.Host = host; // Hostunuzun smtp için mail domaini.
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
            client.Port = port; 
            client.Host = host; 
            client.EnableSsl = false; 
            client.Timeout = 10000; 
            client.DeliveryMethod = SmtpDeliveryMethod.Network; 
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("info@bullbeezapi.co", "Aytug1989.,"); 
            MailMessage mm = new MailMessage("info@bullbeezapi.co", "kocerabdulkadir@gmail.com", mailRequest.Subject, mailRequest.Body); 
            mm.To.Add("ahmetunat34@hotmail.com");
            mm.To.Add("bullbeezco@gmail.com");
            mm.To.Add("aytugkonuralp@gmail.com");
            mm.IsBodyHtml = true; 
            mm.BodyEncoding = UTF8Encoding.UTF8; 
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure; 
            client.Send(mm);

            return "";
        }
    }
}
