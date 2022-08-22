using BullBeez.WebAdmin.Classed;
using BullBeez.WebAdmin.Models;
using BullBeez.WebAdmin.RequestDTO;
using BullBeez.WebAdmin.ResponseDTO;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class InterestController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://bullbeezapi.co/api/WebAdminService/";

        //private static string baseUrl = "https://localhost:44340/api/WebAdminService/";       
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Interest
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetInterestList(DataTablesRequest dataRequest)
        {
            var json = _Serializer.Serialize(new { take = dataRequest.length, skip = dataRequest.start, search = dataRequest.Search.Value, CommonType = 2 });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetCommonData";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<List<CommonListResponse>>(result);

            if (dataRequest.Search.Value != null)
            {
                response = response.Where(x => x.Name.ToLower(new CultureInfo("tr-TR", false)).Contains(dataRequest.Search.Value.ToLower(new CultureInfo("tr-TR", false)))).ToList();
            }
            var count = response.Count;
            response = response.OrderBy(x => x.Name).Skip(dataRequest.start).Take(dataRequest.length).ToList();

            DataTableResponse<CommonListResponse> datas = new DataTableResponse<CommonListResponse>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = count;
            datas.data = response;
            datas.recordsFiltered = count;
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetInterestById(GetCommonDataByIdRequest request)
        {
            var json = _Serializer.Serialize(new { Id = request.Id, CommonType = 2 });


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetCommonDataByIdData";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<GlobalListResponse>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetInterestInsetOrUpdate(CommonDataInsertOrUpdate request)
        {
            request.CommonType = 2;
            var json = _Serializer.Serialize(request);


            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var insertOrUpdateUrl = request.Id > 0 ? "CommonDataUpdate" : "CommonDataInsert";
            var url = baseUrl + insertOrUpdateUrl;
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<BaseInsertOrUpdateResponse>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}