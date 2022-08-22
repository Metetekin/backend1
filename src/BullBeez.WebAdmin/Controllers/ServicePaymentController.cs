using BullBeez.WebAdmin.Classed;
using BullBeez.WebAdmin.Models;
using BullBeez.WebAdmin.PaymentApiCore.Model;
using BullBeez.WebAdmin.ResponseDTO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BullBeez.WebAdmin.Controllers
{
    [CustomAuthorizeAttribute]
    public class ServicePaymentController : Controller
    {
        // GET: ServicePayment
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetServicePaymentList(DataTablesRequest dataRequest)
        {
            var DB = new db();
            var response = DB.GetServicePayment();

            List<ServicePayment> list = new List<ServicePayment>();

            foreach (var item in response)
            {
                if (list.Where(x => x.Guid == item.Guid).Count() == 0)
                {
                    list.Add(item);
                }
            }

            int count = list.Count();
            response = list.OrderBy(x => x.Inserteddate).Skip(dataRequest.start).Take(dataRequest.length).ToList();

            DataTableResponse<ServicePayment> datas = new DataTableResponse<ServicePayment>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = count;
            datas.data = response;
            datas.recordsFiltered = count;
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetServicePaymentById(Guid Id)
        {
            var DB = new db();
            var response = DB.GetServicePaymentById(Id);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}