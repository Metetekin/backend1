using BullBeez.Core.RequestDTO;
using BullBeez.Core.ResponseDTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Services
{
    public interface IServiceService
    {
        Task<ApiResult<List<ServiceListResponse>>> GetServiceList();
        Task<ApiResult<List<PackageResponse>>> GetPackageList();
        Task<ApiResult<ServiceListResponse>> GetServiceById(int id);
        Task<ApiResult<CreatePayment>> CreatePayment(ServiceAnswerRequest request);
        Task<ApiResult<CreatePayment>> CreatePackagePayment(CreatePackagePaymentRequest request);
    }
}
