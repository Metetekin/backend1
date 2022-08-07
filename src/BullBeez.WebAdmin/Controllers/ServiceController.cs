using BullBeez.WebAdmin.Models;
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
    public class ServiceController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://localhost:44340/api/WebAdminService/";
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Service
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetServiceList(DataTablesRequest dataRequest)
        {
            
            var url = baseUrl + "GetService";
            var client = new HttpClient();

            var response2 = await client.GetAsync(url);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<List<ServiceListResponse>>(result);

            DataTableResponse<ServiceListResponse> datas = new DataTableResponse<ServiceListResponse>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = response.Count();
            datas.data = response;
            datas.recordsFiltered = response.Count();
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetServiceById(GetServiceByIdRequest request)
        {
            var json = _Serializer.Serialize(new { Id = request.Id});


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetServiceById";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<ServiceListResponse>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ServiceInsetOrUpdate(ServiceListResponse request)
        {
            var json = _Serializer.Serialize(request);


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "ServiceInsetOrUpdate";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<BaseInsertOrUpdateResponse>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> ServiceQuestionInsetOrUpdate(QuestionModel request)
        {
            var json = _Serializer.Serialize(request);


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "ServiceQuestionInsetOrUpdate";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<BaseInsertOrUpdateResponse>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> QuestionOptionInsetOrUpdate(OptionModel request)
        {
            var json = _Serializer.Serialize(request);


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "QuestionOptionInsetOrUpdate";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<BaseInsertOrUpdateResponse>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}