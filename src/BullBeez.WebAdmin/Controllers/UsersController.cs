using BullBeez.WebAdmin.Models;
using BullBeez.WebAdmin.RequestDTO;
using BullBeez.WebAdmin.ResponseDTO;

using Newtonsoft.Json;

using RestSharp;

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
    public class UsersController : Controller
    {
        static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://localhost:44340/api/WebAdminService/";
        private static JavaScriptSerializer _Serializer = new JavaScriptSerializer();
        // GET: Users
        public async Task<ActionResult> Index()
        {
           

            return View();
        }

        //public async Task<ActionResult> GetUsers(DataTablesRequest dataRequest)
        //{
        //    HttpResponseMessage responseClient = await client.GetAsync("https://localhost:44340/api/WebAdminService");
        //    responseClient.EnsureSuccessStatusCode();
        //    string responseBody = await responseClient.Content.ReadAsStringAsync();
            
        //    var response = JsonConvert.DeserializeObject<List<UserListResponse>>(responseBody);

        //    DataTableResponse<UserListResponse> datas = new DataTableResponse<UserListResponse>();
        //    datas.draw = dataRequest.draw;
        //    datas.recordsTotal = response.Count;
        //    datas.data = response;
        //    datas.recordsFiltered = response.Count;
        //    return Json(datas, JsonRequestBehavior.AllowGet);
        //}

        public async Task<ActionResult> GetUsers(DataTablesRequest dataRequest)
        {
            var json = _Serializer.Serialize(new { take = dataRequest.length,skip=dataRequest.start,search = dataRequest.Search.Value});


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl+"GetUsers";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<List<UserListResponse>>(result);

            DataTableResponse<UserListResponse> datas = new DataTableResponse<UserListResponse>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = response.FirstOrDefault().CountData;
            datas.data = response;
            datas.recordsFiltered = response.FirstOrDefault().CountData;
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetUserById(GetUserByIdRequest request)
        {
            var json = _Serializer.Serialize(new { userId = request.userId });


            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = baseUrl + "GetUserById";
            var client = new HttpClient();

            var response2 = await client.PostAsync(url, data);

            string result = response2.Content.ReadAsStringAsync().Result;
            var response = JsonConvert.DeserializeObject<UserListResponse>(result);

           
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        
    }
}