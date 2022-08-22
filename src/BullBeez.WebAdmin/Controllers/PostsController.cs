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
    public class PostsController : Controller
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

        public async Task<ActionResult> GetPostById(int Id)
        {
            var json = _Serializer.Serialize(new {Id= Id });

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetUserPostById";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<PostDatatableModel>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> InsertPost(NewPostRequest request)
        {
           
            var json = _Serializer.Serialize(request);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "NewPost";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<PostDatatableModel>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> DeletePost(DeletePostByIdRequest request)
        {
            var json = _Serializer.Serialize(request);

            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "DeletePostById";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<PostDatatableModel>(result);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}