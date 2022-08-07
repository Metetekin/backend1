using BullBeez.WebAdmin.Models;
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
    public class PackageController : Controller
    {
        // GET: Package
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://localhost:44340/api/WebAdminService/";
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPackageList(DataTablesRequest dataRequest)
        {
            var json = _Serializer.Serialize(new { take = dataRequest.length, skip = dataRequest.start, search = dataRequest.Search.Value });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetPackageList";
            var client = new HttpClient();

            var response2 = await client.GetAsync(url);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<List<PackageDatatableModel>>(result);

            int count = response.Count();
            response = response.OrderBy(x => x.Id).Skip(dataRequest.start).Take(dataRequest.length).ToList();

            DataTableResponse<PackageDatatableModel> datas = new DataTableResponse<PackageDatatableModel>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = count;
            datas.data = response;
            datas.recordsFiltered = count;
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
    }
}