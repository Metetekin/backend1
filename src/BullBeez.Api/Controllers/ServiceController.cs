using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;

using Microsoft.AspNetCore.Mvc;

using RestSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BullBeez.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceService _serviceService;

        public ServiceController(IServiceService serviceService)
        {
            this._serviceService = serviceService;
        }

        [HttpGet("GetDeneme")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetDeneme()
        {
          
            var response = await CreatePackagePayment(new CreatePackagePaymentRequest { UserId = 17,productId="kelebek1",transactionDate=1640205078000});
            
            return response.SerializeObject();
        }


        
       

        [HttpPost("GetServiceList")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetServiceList()
        {
            var response = await _serviceService.GetServiceList();

            return response.SerializeObject();
        }

        [HttpPost("GetPackageList")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetPackageList()
        {
            var response = await _serviceService.GetPackageList();
            response.Data = response.Data.OrderBy(x => x.Id).ToList();
            return response.SerializeObject();
        }

        [HttpPost("GetUserPackageDetail")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUserPackageDetail([FromHeader] BaseRequest request)
        {
            var response = await _serviceService.GetPackageUserById(request.UserId.Value);

            return response.SerializeObject();

        }

        [HttpPost("GetUserPackageDetail2")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUserPackageDetail2([FromHeader] BaseRequest request)
        {
            List<int> dataList = new List<int>{ 6, 17, 18, 28, 30};
            if (dataList.Contains(request.UserId.Value))
            {
                GetUserPackageDetail data = new GetUserPackageDetail();
                data.PackageId = 2;
                data.PackageName = "Arı Paketi";
                data.ProfileType = 1;
                data.UniqCode = "ari";
                data.Color = 1;
                var response = new ApiResult<GetUserPackageDetail>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = "",
                    Data = data
                };

                return response.SerializeObject();
            }
            else
            {
                GetUserPackageDetail data2 = new GetUserPackageDetail();
               
                var response = new ApiResult<GetUserPackageDetail>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = "",
                    Data = data2
                };

                return response.SerializeObject();
            }
            
        }

        [HttpPost("CreatePayment")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> CreatePayment([FromHeader] ServiceAnswerRequest request)
        {
            var response = await _serviceService.CreatePayment(request);

            return response.SerializeObject();
        }

        [HttpPost("CreatePackagePayment2")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> CreatePackagePayment2([FromHeader] CreatePackagePaymentRequest request)
        {
            var response = await _serviceService.CreatePackagePayment(request);

            return response.SerializeObject();
        }

        [HttpPost("CreatePackagePayment")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> CreatePackagePayment([FromHeader] CreatePackagePaymentRequest request)
        {
            var response = await _serviceService.CreatePackagePayment(request);

            return response.SerializeObject();
        }

        
    }
}