using BullBeez.WebAdmin.Classed;
using BullBeez.WebAdmin.PaymentApiCore.Model;
using BullBeez.WebAdmin.RequestDTO;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BullBeez.WebAdmin.Controllers
{
    [CustomAuthorizeAttribute]
    public class NotificationController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://bullbeezapi.co/api/WebAdminService/";

        //private static string baseUrl = "https://localhost:44340/api/WebAdminService/";       
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Posts
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SendNotification(SendNotificationRequest request)
        {
            var DB = new db();
            var response = DB.SendUserNotification(request);
            foreach (var item in response)
            {
                await SendNotificationData(new NotificationModel
                {
                    Title = request.Title,
                    Body = request.Body,
                    DeviceFirebaseToken = item.FirebaseToken
                });
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        public async Task<string> SendNotificationData(NotificationModel requestModel)
        {
            Root root = new Root();
            root.to = requestModel.DeviceFirebaseToken;
            root.content_avaible = true;
            root.notification = new Notification { body = requestModel.Body, title = requestModel.Title };
            root.data = new Data { postId = requestModel.PostId };

            var client = new RestClient("https://fcm.googleapis.com/fcm/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "key=AAAAV6XZEow:APA91bGoQJX8U0gFw-EYlzoYcaQ01DtzhNCkYO4f5JXYpcYDpMpzvyHmvfMYH8VYpCX-zsJo460yvs6TPHfTeNfuKiguD7Evjvm953J3gwYHK8nJ37cUba1FWzaYBn9e78-q8ixiJ_eV");
            request.AddHeader("Content-Type", "application/json");
            var body = Newtonsoft.Json.JsonConvert.SerializeObject(root);


            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response2 = client.Execute(request);

            return "";
        }

        public class NotificationModel
        {
            public int UserId { get; set; }
            public string DeviceFirebaseToken { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string TypeString { get; set; }
            public string PostId { get; set; }
        }

        public class Notification
        {
            public string body { get; set; }
            public string title { get; set; }
        }

        public class Data
        {
            public string postId { get; set; }
        }

        public class Root
        {
            public string to { get; set; }
            public bool content_avaible { get; set; }
            public Notification notification { get; set; }
            public Data data { get; set; }
        }
    }
}