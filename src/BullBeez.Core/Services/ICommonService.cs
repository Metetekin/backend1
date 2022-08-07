using BullBeez.Core.RequestDTO;
using BullBeez.Core.RequestDTO.WebUIRequest;
using BullBeez.Core.ResponseDTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Services
{
    public interface ICommonService
    {
        Task<ApiResult<List<OccupationResponse>>> OccupationList();
        Task<ApiResult<List<OccupationResponse>>> OccupationListWeb();
        Task<ApiResult<List<EducationResponse>>> EducationList(GetUserEducationListRequest request);
        Task<ApiResult<List<SkillResponse>>> SkillList();
        Task<ApiResult<List<SkillResponse>>> SkillListWeb();
        Task<ApiResult<List<InterestResponse>>> InterestsList();
        Task<ApiResult<List<InterestResponse>>> InterestsListWeb();
        Task<ApiResult<List<GlobalListResponse>>> GetCity();
        Task<ApiResult<List<GlobalListResponse>>> GetCounty(GetCountyByCityIdRequest request);
        Task<ApiResult<List<GlobalListResponse>>> GetCompanyType();
        Task<ApiResult<List<GlobalListResponse>>> GetCompanyTypeWeb();
        Task<ApiResult<List<GlobalListResponse>>> GetCompanyLevel();
        Task<ApiResult<List<GlobalListResponse>>> GetCompanyLevelWeb();
        Task<ApiResult<List<PostListResponse>>> GetPostFilterList(FilterRequest request);
        Task<ApiResult<ConfigResponse>> GetConfig();
        Task<ApiResult<BaseInsertOrUpdateResponse>> InsertFeedBack(InsertFeedBackRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> ShowNotifications(BaseRequest request);
        Task<ApiResult<GlobalListResponse>> GetCommonDataById(GetCommonDataByIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> CommonDataInsert(CommonDataInsertOrUpdate request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> CommonDataUpdate(CommonDataInsertOrUpdate request);
    }
}
