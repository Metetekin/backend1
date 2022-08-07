using BullBeez.WebAdmin.Models;
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
    public class PostsController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://localhost:44340/api/WebAdminService/";
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Posts
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPostList(DataTablesRequest dataRequest)
        {
            var json = _Serializer.Serialize(new { take = dataRequest.length, skip = dataRequest.start, search = dataRequest.Search.Value });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetUserPosts";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<List<PostDatatableModel>>(result);

            
            response = response.OrderByDescending(x => x.Id).ToList();

            DataTableResponse<PostDatatableModel> datas = new DataTableResponse<PostDatatableModel>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = response.FirstOrDefault().CountData;
            datas.data = response;
            datas.recordsFiltered = response.FirstOrDefault().CountData;
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
    }
}