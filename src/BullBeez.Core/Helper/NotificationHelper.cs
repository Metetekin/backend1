using BullBeez.Core.DTO;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Helper
{
    public class NotificationHelper : INotificationHelper
    {
        public async Task<string> SendNotification(NotificationModel requestModel)
        {
            Root root = new Root();
            root.to = requestModel.DeviceFirebaseToken;
            root.content_avaible = true;
            root.notification = new Notification { body=requestModel.Body,title = requestModel.Title};
            root.data = new Data { postId = requestModel.PostId };

            var client = new RestClient("https://fcm.googleapis.com/fcm/send");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "key=AAAAV6XZEow:APA91bGoQJX8U0gFw-EYlzoYcaQ01DtzhNCkYO4f5JXYpcYDpMpzvyHmvfMYH8VYpCX-zsJo460yvs6TPHfTeNfuKiguD7Evjvm953J3gwYHK8nJ37cUba1FWzaYBn9e78-q8ixiJ_eV");
            request.AddHeader("Content-Type", "application/json");
            var body = root.SerializeObject();
         
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response2 = client.Execute(request);

            return "";
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
