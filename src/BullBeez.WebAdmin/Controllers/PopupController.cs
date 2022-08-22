using BullBeez.WebAdmin.Classed;
using BullBeez.WebAdmin.RequestDTO;
using BullBeez.WebAdmin.ResponseDTO;

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
    [CustomAuthorizeAttribute]
    public class PopupController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://bullbeezapi.co/api/WebAdminService/";

        //private static string baseUrl = "https://localhost:44340/api/WebAdminService/";                      

        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> InsertPopUp(PopUpInsertRequest request)
        {

            var json = _Serializer.Serialize(request);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "InsertPopUp";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<PostDatatableModel>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}