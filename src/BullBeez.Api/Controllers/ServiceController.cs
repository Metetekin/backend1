using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.Services;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
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
            var response = await CreatePackagePayment(new CreatePackagePaymentRequest { UserId = 6, PackageId = 1, PaymentAmount = 100, ContractConfirmation = 1 });

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

            return response.SerializeObject();
        }

        [HttpPost("CreatePayment")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> CreatePayment([FromHeader] ServiceAnswerRequest request)
        {
            var response = await _serviceService.CreatePayment(request);

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