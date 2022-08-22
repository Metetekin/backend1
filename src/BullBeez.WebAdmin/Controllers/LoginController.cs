using BullBeez.WebAdmin.Models;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace BullBeez.WebAdmin.Controllers
{
    public class LoginController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://bullbeezapi.co/api/WebAdminService/";

        //private static string baseUrl = "https://localhost:44340/api/WebAdminService/";       
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UserLogin(LoginModel loginModel)
        {
            var json = _Serializer.Serialize(new { EmailAddress = loginModel.EmailAddress, Password = loginModel.Password });


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "LoginUserWeb";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<NewUserResponse>(result);

            Session["UserProfile"] = response;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}