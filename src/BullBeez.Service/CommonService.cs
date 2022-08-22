using AutoMapper;

using BullBeez.Core.DTO.MailDTO;
using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.RequestDTO.WebUIRequest;
using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;
using BullBeez.Core.UOW;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Service
{
    public class CommonService : ICommonService
    {
        private readonly string StatusCode = ResponseCode.Basarili;
        private readonly string ResultMessage = "";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public CommonService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<ApiResult<List<OccupationResponse>>> OccupationList()
        {
            var response = await _unitOfWork.Occupations.GetAll().ConfigureAwait(false);
            List<OccupationResponse> responseData = new List<OccupationResponse>();

            foreach (var item in response.OrderBy(x=> x.Name))
            {
                responseData.Add(new OccupationResponse { Id = item.Id, Description = item.Name, IsSelected = item.IsShowSkill, HashId = "#" + item.Id.ToString() + "#" });
            }
            return new ApiResult<List<OccupationResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<OccupationResponse>>> OccupationListWeb()
        {
            var response = await _unitOfWork.Occupations.GetAllAsync().ConfigureAwait(false);
            List<OccupationResponse> responseData = new List<OccupationResponse>();

            foreach (var item in response.OrderBy(x => x.Name))
            {
                responseData.Add(new OccupationResponse { Id = item.Id, Name = item.Name, IsSelected = item.IsShowSkill, RowStatu = (int)item.RowStatu });
            }
            return new ApiResult<List<OccupationResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<EducationResponse>>> EducationList(GetUserEducationListRequest request)
        {
            var response = await _unitOfWork.Educations.GetAllFilter(x=> x.CompanyAndPerson.Id == request.UserId);
            List<EducationResponse> responseData = new List<EducationResponse>();

            foreach (var item in response)
            {
                responseData.Add(new EducationResponse { Id = item.Id, UserId = item.CompanyAndPerson.Id, BeginDate = item.BeginDate.ToString("dd/MM/yyyy"), EndDate =  item.EndDate?.ToString("dd/MM/yyyy"), SchoolName = item.SchoolName });
            }
            return new ApiResult<List<EducationResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<SkillResponse>>> SkillList()
        {
            var response = await _unitOfWork.Skills.GetAll();
            List<SkillResponse> responseData = new List<SkillResponse>();


            foreach (var item in response.OrderBy(x => x.Name))
            {
                responseData.Add(new SkillResponse { Id = item.Id, Name = item.Name, IsSelected = 0, HashId = "#" + item.Id.ToString() + "#" });
            }
            return new ApiResult<List<SkillResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<SkillResponse>>> SkillListWeb()
        {
            var response = await _unitOfWork.Skills.GetAll();
            List<SkillResponse> responseData = new List<SkillResponse>();


            foreach (var item in response.OrderBy(x => x.Name))
            {
                responseData.Add(new SkillResponse { Id = item.Id, Name = item.Name, IsSelected = 0, RowStatu = (int)item.RowStatu });
            }
            return new ApiResult<List<SkillResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<GlobalListResponse>>> GetCity()
        {
            var response = await _unitOfWork.City.GetAll();
            List<GlobalListResponse> responseData = new List<GlobalListResponse>();


            foreach (var item in response)
            {
                responseData.Add(new GlobalListResponse { Id = item.Id, Name = item.Name });
            }
            return new ApiResult<List<GlobalListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<GlobalListResponse>>> GetCompanyType()
        {
            var response = await _unitOfWork.CompanyType.GetAll();
            List<GlobalListResponse> responseData = new List<GlobalListResponse>();


            foreach (var item in response)
            {
                responseData.Add(new GlobalListResponse { Id = item.Id, Name = item.Name , IsVenture = item.IsVenture});
            }
            return new ApiResult<List<GlobalListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<GlobalListResponse>>> GetCompanyTypeWeb()
        {
            var response = await _unitOfWork.CompanyType.GetAllAsync();
            List<GlobalListResponse> responseData = new List<GlobalListResponse>();


            foreach (var item in response)
            {
                responseData.Add(new GlobalListResponse { Id = item.Id, Name = item.Name, IsVenture = item.IsVenture, RowStatu = (int)item.RowStatu });
            }
            return new ApiResult<List<GlobalListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<GlobalListResponse>>> GetCompanyLevel()
        {
            var response = await _unitOfWork.CompanyLevel.GetAll();
            List<GlobalListResponse> responseData = new List<GlobalListResponse>();


            foreach (var item in response)
            {
                responseData.Add(new GlobalListResponse { Id = item.Id, Name = item.Name });
            }
            return new ApiResult<List<GlobalListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<GlobalListResponse>>> GetCompanyLevelWeb()
        {
            var response = await _unitOfWork.CompanyLevel.GetAllAsync();
            List<GlobalListResponse> responseData = new List<GlobalListResponse>();


            foreach (var item in response)
            {
                responseData.Add(new GlobalListResponse { Id = item.Id, Name = item.Name, RowStatu = (int)item.RowStatu });
            }
            return new ApiResult<List<GlobalListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<GlobalListResponse>>> GetCounty(GetCountyByCityIdRequest request)
        {
            var response = await _unitOfWork.County.GetAllFilter(x=> x.City.Id == request.CityId);
            List<GlobalListResponse> responseData = new List<GlobalListResponse>();


            foreach (var item in response)
            {
                responseData.Add(new GlobalListResponse { Id = item.Id, Name = item.Name });
            }
            return new ApiResult<List<GlobalListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<InterestResponse>>> InterestsList()
        {
            var response = await _unitOfWork.Interests.GetAll();
            List<InterestResponse> responseData = new List<InterestResponse>();


            foreach (var item in response.OrderBy(x => x.Name))
            {
                responseData.Add(new InterestResponse { Id = item.Id, Title = item.Title, Name = item.Name, IsSelected = 0, HashId = "#" + item.Id.ToString() + "#" });
            }
            return new ApiResult<List<InterestResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<InterestResponse>>> InterestsListWeb()
        {
            var response = await _unitOfWork.Interests.GetAllAsync();
            List<InterestResponse> responseData = new List<InterestResponse>();


            foreach (var item in response.OrderBy(x => x.Name))
            {
                responseData.Add(new InterestResponse { Id = item.Id, Name = item.Name, IsSelected = 0, RowStatu = (int)item.RowStatu });
            }
            return new ApiResult<List<InterestResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<List<PostListResponse>>> GetPostFilterList(FilterRequest request)
        {
            var response = await _unitOfWork.Posts.GetPostFilterList(request);

            var responseData = _mapper.Map<IEnumerable<Posts>, List<PostListResponse>>(response);
            
            return new ApiResult<List<PostListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<ConfigResponse>> GetConfig()
        {
            var response = await _unitOfWork.BullBeezConfig.GetByIdAsync(1).ConfigureAwait(false);

            ConfigResponse configResponse = response.JsonData.DeserializeObject<ConfigResponse>();

            return new ApiResult<ConfigResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = configResponse
            };
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> InsertFeedBack(InsertFeedBackRequest request)
        {
            var companyAndPerson = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

            await _unitOfWork.Feedback.AddAsync(new Feedback { CompanyAndPerson = companyAndPerson, Description = request.Description});

            await _unitOfWork.CommitAsync();

            return new ApiResult<BaseInsertOrUpdateResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new BaseInsertOrUpdateResponse { Response = 1}
            };
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> ShowNotifications(BaseRequest request)
        {
            var notifications = await _unitOfWork.Notifications.GetListNotificationByUserId(request).ConfigureAwait(false);

            notifications.ToList().ForEach(c => c.IsShow = 1);

            foreach (var item in notifications)
            {
                await _unitOfWork.Notifications.UpdateAsync(item);
            }

            await _unitOfWork.CommitAsync();

            return new ApiResult<BaseInsertOrUpdateResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new BaseInsertOrUpdateResponse { Response = 1 }
            };
        }

        public async Task<ApiResult<GlobalListResponse>> GetCommonDataById(GetCommonDataByIdRequest request)
        {
            GlobalListResponse globalListResponse = new GlobalListResponse();
            if (request.CommonType == 1)
            {
                var response = await _unitOfWork.Occupations.SingleOrDefaultAsync(x=> x.Id == request.Id).ConfigureAwait(false);
                globalListResponse.Id = response.Id;
                globalListResponse.Name = response.Name;
               
            }
            else if (request.CommonType == 2)
            {
                var response = await _unitOfWork.Interests.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                globalListResponse.Id = response.Id;
                globalListResponse.Name = response.Name;
            }
            else if (request.CommonType == 3)
            {
                var response = await _unitOfWork.Skills.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                globalListResponse.Id = response.Id;
                globalListResponse.Name = response.Name;
            }
            else if (request.CommonType == 4)
            {
                var response = await _unitOfWork.CompanyType.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                globalListResponse.Id = response.Id;
                globalListResponse.Name = response.Name;
            }
            else if (request.CommonType == 5)
            {
                var response = await _unitOfWork.CompanyLevel.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                globalListResponse.Id = response.Id;
                globalListResponse.Name = response.Name;
            }

            return new ApiResult<GlobalListResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = globalListResponse
            };
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> CommonDataInsert(CommonDataInsertOrUpdate request)
        {
            if (request.CommonType == 1)
            {
                await _unitOfWork.Occupations.AddAsync(new Occupation { Name = request.Name, RowStatu = EnumRowStatusType.Active });

            }
            else if (request.CommonType == 2)
            {
                await _unitOfWork.Interests.AddAsync(new Interest { Name = request.Name, RowStatu = EnumRowStatusType.Active });
                
            }
            else if (request.CommonType == 3)
            {
                await _unitOfWork.Skills.AddAsync(new Skill { Name = request.Name, RowStatu = EnumRowStatusType.Active });
                
            }
            else if (request.CommonType == 4)
            {
                await _unitOfWork.CompanyType.AddAsync(new CompanyType { Name = request.Name, RowStatu = EnumRowStatusType.Active });
                
            }
            else if (request.CommonType == 5)
            {
                await _unitOfWork.CompanyLevel.AddAsync(new CompanyLevel { Name = request.Name, RowStatu = EnumRowStatusType.Active });
                
            }
            await _unitOfWork.CommitAsync();
            return new ApiResult<BaseInsertOrUpdateResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new BaseInsertOrUpdateResponse { Response = 1 }
            };
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> CommonDataUpdate(CommonDataInsertOrUpdate request)
        {
            if (request.CommonType == 1)
            {
                var data = await _unitOfWork.Occupations.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                data.Name = request.Name;
                data.RowStatu = request.RowStatu == 1 ? EnumRowStatusType.Active : request.RowStatu == -1 ? EnumRowStatusType.Passive : data.RowStatu;
                await _unitOfWork.Occupations.UpdateAsync(data);

            }
            else if (request.CommonType == 2)
            {
                var data = await _unitOfWork.Interests.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                data.Name = request.Name;
                data.RowStatu = request.RowStatu == 1 ? EnumRowStatusType.Active : request.RowStatu == -1 ? EnumRowStatusType.Passive : data.RowStatu;
                await _unitOfWork.Interests.UpdateAsync(data);

            }
            else if (request.CommonType == 3)
            {
                var data = await _unitOfWork.Skills.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                data.Name = request.Name;
                data.RowStatu = request.RowStatu == 1 ? EnumRowStatusType.Active : request.RowStatu == -1 ? EnumRowStatusType.Passive : data.RowStatu;
                await _unitOfWork.Skills.UpdateAsync(data);

            }
            else if (request.CommonType == 4)
            {
                var data = await _unitOfWork.CompanyType.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                data.Name = request.Name;
                data.RowStatu = request.RowStatu == 1 ? EnumRowStatusType.Active : request.RowStatu == -1 ? EnumRowStatusType.Passive : data.RowStatu;
                await _unitOfWork.CompanyType.UpdateAsync(data);

            }
            else if (request.CommonType == 5)
            {
                var data = await _unitOfWork.CompanyLevel.SingleOrDefaultAsync(x => x.Id == request.Id).ConfigureAwait(false);
                data.Name = request.Name;
                data.RowStatu = request.RowStatu == 1 ? EnumRowStatusType.Active : request.RowStatu == -1 ? EnumRowStatusType.Passive : data.RowStatu;
                await _unitOfWork.CompanyLevel.UpdateAsync(data);

            }
            await _unitOfWork.CommitAsync();
            return new ApiResult<BaseInsertOrUpdateResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new BaseInsertOrUpdateResponse { Response = 1 }
            };
        }

    }
}
