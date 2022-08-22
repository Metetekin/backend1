using BullBeez.Core.DTO.MailDTO;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;

using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            this._commonService = commonService;
        }

       

        [HttpPost("OccupationList")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> OccupationList()
        {
            var response = await _commonService.OccupationList().ConfigureAwait(false);

            return response.SerializeObject();
        }

        [HttpPost("OccupationListWeb")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> OccupationListWeb()
        {
            var response = await _commonService.OccupationListWeb().ConfigureAwait(false);

            return response.SerializeObject();
        }

        [HttpGet("GetDeneme")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetDeneme()
        {
            OccupationResponse obj = new OccupationResponse();
            obj.Id = 1;
            obj.Description = "safsagfsag";

            return obj.SerializeObject();
        }

        [HttpPost("InterestsList")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> InterestsList()
        {
            var response = await _commonService.InterestsList();

            return response.SerializeObject();
        }

        [HttpPost("InterestsListWeb")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> InterestsListWeb()
        {
            var response = await _commonService.InterestsListWeb();

            return response.SerializeObject();
        }

        [HttpPost("SkillList")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> SkillList()
        {
            var response = await _commonService.SkillList();

            return response.SerializeObject();
        }

        [HttpPost("SkillListWeb")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> SkillListWeb()
        {
            var response = await _commonService.SkillListWeb();

            return response.SerializeObject();
        }

        [HttpPost("GetCity")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCity()
        {
            var response = await _commonService.GetCity();

            return response.SerializeObject();
        }

        [HttpPost("GetCounty")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCounty([FromHeader] GetCountyByCityIdRequest request)
        {
            var response = await _commonService.GetCounty(request);

            return response.SerializeObject();
        }

        [HttpPost("GetCompanyType")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCompanyType()
        {
            var response = await _commonService.GetCompanyType();

            return response.SerializeObject();
        }

        [HttpPost("GetCompanyTypeWeb")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCompanyTypeWeb()
        {
            var response = await _commonService.GetCompanyTypeWeb();

            return response.SerializeObject();
        }

        [HttpPost("GetCompanyLevel")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCompanyLevel()
        {
            var response = await _commonService.GetCompanyLevel();

            return response.SerializeObject();
        }

        [HttpPost("GetCompanyLevelWeb")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCompanyLevelWeb()
        {
            var response = await _commonService.GetCompanyLevelWeb();

            return response.SerializeObject();
        }

        [HttpPost("GetCompanyLevelList")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetCompanyLevelList()
        {
            var response = await _commonService.GetCompanyLevel();

            return response.SerializeObject();
        }

        [HttpPost("GetContent")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetContent([FromHeader] FilterRequest request)
        {
            var response = await _commonService.GetPostFilterList(request);

            return response.SerializeObject();
        }

        [HttpGet("Privacy")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> Privacy()
        {
            var response = await _commonService.GetConfig();
           
            return response.Data.ContractText.SerializeObject();
        }

        [HttpPost("InsertFeedBack")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> InsertFeedBack([FromHeader] InsertFeedBackRequest request)
        {
            var response = await _commonService.InsertFeedBack(request);

            return response.SerializeObject();
        }


        [HttpGet("ShowNotifications")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> ShowNotifications([FromHeader] BaseRequest request)
        {
            var response = await _commonService.ShowNotifications(request);

            return response.SerializeObject();
        }

    }
}
