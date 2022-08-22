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
    public class PackagePaymentController : Controller
    {
        // GET: PackagePayment
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPackagePaymentList(DataTablesRequest dataRequest)
        {
            var DB = new db();
            var response = DB.GetPackagePayment();

            int count = response.Count();
            response = response.OrderBy(x => x.Inserteddate).Skip(dataRequest.start).Take(dataRequest.length).ToList();

            DataTableResponse<PackagePayment> datas = new DataTableResponse<PackagePayment>();
            datas.draw = dataRequest.draw;
            datas.recordsTotal = count;
            datas.data = response;
            datas.recordsFiltered = count;
            return Json(datas, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> InsertPackagePayment(int PackageId, int CompanyAndPersonId)
        {
            var DB = new db();
            DB.InsertPackagePayment(PackageId, CompanyAndPersonId);


            return Json(1, JsonRequestBehavior.AllowGet);
        }
    }
}