using BullBeez.Core.Entities;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.RequestDTO.WebUIRequest;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse;
using BullBeez.Core.Services;
using BullBeez.Core.UOW;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BullBeez.Core.DTO.Sms;

namespace BullBeez.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class WebAdminService : Controller
	{
		private readonly ICompanyAndPersonService _companyAndPersonService;
		private readonly ICommonService _commonService;
		private readonly IServiceService _serviceService;
		private readonly ISmsService _smsService;
		private readonly IUnitOfWork _unitOfWork;

		public WebAdminService(ICompanyAndPersonService companyAndPersonService,
				ICommonService commonService,
				IServiceService serviceService,
				IUnitOfWork unitOfWork,
				ISmsService smsService)
		{
			this._companyAndPersonService = companyAndPersonService;
			this._commonService = commonService;
			this._serviceService = serviceService;
			this._unitOfWork = unitOfWork;
			_smsService = smsService;
		}

		[HttpPost("GetUsers")]
		public async Task<string> GetUsers(GetUsersRequestDTO request)
		{
			var response = await _companyAndPersonService.GetUsers(request).ConfigureAwait(false);

			return response.SerializeObject();
		}

		[HttpPost("GetUserById")]
		public async Task<string> GetUserById(GetUserByIdRequest request)
		{
			var response = await _companyAndPersonService.GetUserById(request.userId).ConfigureAwait(false);

			return response.SerializeObject();
		}

		[HttpPost("GetCommonData")]
		public async Task<string> GetCommonData(GetCommonDataRequest request)
		{
			if (request.CommonType == 1)
			{
				var response = await _commonService.OccupationListWeb().ConfigureAwait(false);

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 2)
			{
				var response = await _commonService.InterestsListWeb();

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 3)
			{
				var response = await _commonService.SkillListWeb();

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 4)
			{
				var response = await _commonService.GetCompanyTypeWeb();

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 5)
			{
				var response = await _commonService.GetCompanyLevelWeb();

				return response.Data.SerializeObject();
			}
			else
			{
				var response = await _commonService.GetCompanyLevelWeb();

				return response.Data.SerializeObject();
			}
		}

		[HttpPost("GetCommonDataById")]
		public async Task<string> GetCommonDataById(GetCommonDataByIdRequest request)
		{
			if (request.CommonType == 1)
			{
				var response = await _commonService.OccupationListWeb().ConfigureAwait(false);

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 2)
			{
				var response = await _commonService.InterestsListWeb();

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 3)
			{
				var response = await _commonService.SkillListWeb();

				return response.Data.SerializeObject();
			}
			else if (request.CommonType == 4)
			{
				var response = await _commonService.GetCompanyTypeWeb();

				return response.Data.SerializeObject();
			}
			else
			{
				var response = await _commonService.GetCompanyLevelWeb();

				return response.Data.SerializeObject();
			}
		}

		[HttpPost("GetCommonDataByIdData")]
		public async Task<string> GetCommonDataByIdData(GetCommonDataByIdRequest request)
		{
			var response = await _commonService.GetCommonDataById(request);

			return response.Data.SerializeObject();

		}

		[HttpPost("CommonDataInsert")]
		public async Task<string> CommonDataInsert(CommonDataInsertOrUpdate request)
		{
			var response = await _commonService.CommonDataInsert(request);

			return response.Data.SerializeObject();

		}

		[HttpPost("CommonDataUpdate")]
		public async Task<string> CommonDataUpdate(CommonDataInsertOrUpdate request)
		{
			var response = await _commonService.CommonDataUpdate(request);

			return response.Data.SerializeObject();

		}

		[HttpGet("GetService")]
		public async Task<string> GetService()
		{
			var response = await _serviceService.GetServiceList();

			return response.Data.SerializeObject();

		}

		[HttpPost("GetServiceById")]
		public async Task<string> GetServiceById(GetServiceByIdRequest request)
		{
			var response = await _serviceService.GetServiceById(request.Id);

			return response.Data.SerializeObject();

		}

		[HttpPost("ServiceInsetOrUpdate")]
		public async Task<string> ServiceInsetOrUpdate(ServiceListResponse request)
		{
			if (request.Id > 0)
			{
				var service = new Core.Entities.Service
				{
					ServiceName = request.ServiceName,
					Amount = Convert.ToDecimal(request.Amount)
				,
					Description = request.Description,
					DiscountPercentage = 50,
					ServiceIcon = request.ServiceIcon
				};
				await _unitOfWork.Service.UpdateAsync(service);

				await _unitOfWork.CommitAsync();

				return new BaseInsertOrUpdateResponse { Response = service.Id }.SerializeObject();
			}
			else
			{
				var service = new Core.Entities.Service
				{
					ServiceName = request.ServiceName,
					Amount = Convert.ToDecimal(request.Amount)
				,
					Description = request.Description,
					DiscountPercentage = 50,
					ServiceIcon = request.ServiceIcon
				};
				await _unitOfWork.Service.AddAsync(service);

				await _unitOfWork.CommitAsync();

				return new BaseInsertOrUpdateResponse { Response = service.Id }.SerializeObject();
			}
		}

		[HttpPost("ServiceQuestionInsetOrUpdate")]
		public async Task<string> ServiceQuestionInsetOrUpdate(QuestionModel request)
		{
			var service = await _unitOfWork.Service.GetServiceById(request.ServiceId).ConfigureAwait(false);

			if (request.QuestionId > 0)
			{
				var serviceEntity = service.FirstOrDefault();


				var updateQuestion = serviceEntity.ServiceQuestions.Where(x => x.Question.Id == request.QuestionId).FirstOrDefault().Question;
				updateQuestion.Question = request.Question;
				updateQuestion.QuestionType = request.QuestionType;

				serviceEntity.ServiceQuestions.Where(x => x.Question.Id == request.QuestionId).FirstOrDefault().Question = updateQuestion;

				await _unitOfWork.Service.UpdateAsync(serviceEntity);

				await _unitOfWork.CommitAsync();

				return new BaseInsertOrUpdateResponse { Response = 1 }.SerializeObject();
			}
			else
			{
				var servisQuestion = new ServiceQuestion();
				var question = new Core.Entities.Questions
				{
					Question = request.Question,
					QuestionType = request.QuestionType
				};
				servisQuestion.Service = service.FirstOrDefault();
				servisQuestion.Question = question;
				var serviceEntity = service.FirstOrDefault();

				serviceEntity.ServiceQuestions.Add(servisQuestion);

				await _unitOfWork.Service.UpdateAsync(serviceEntity);

				await _unitOfWork.CommitAsync();

				return new BaseInsertOrUpdateResponse { Response = 1 }.SerializeObject();
			}
		}

		[HttpPost("QuestionOptionInsetOrUpdate")]
		public async Task<string> QuestionOptionInsetOrUpdate(OptionModel request)
		{
			var service = await _unitOfWork.Service.GetServiceById(request.ServiceId2).ConfigureAwait(false);

			if (request.OptionId > 0)
			{
				var serviceEntity = service.FirstOrDefault();
				//userList.Select(c => { c.CountData = count; return c; }).ToList();
				serviceEntity.ServiceQuestions.Where(x => x.Question.Id == request.QuestionId2).FirstOrDefault().Question.QuestionOptions.Where(x => x.Id == request.OptionId).Select(c => { c.Option = request.Option; return c; }).ToList();

				await _unitOfWork.Service.UpdateAsync(serviceEntity);

				await _unitOfWork.CommitAsync();

				return new BaseInsertOrUpdateResponse { Response = 1 }.SerializeObject();
			}
			else
			{
				var option = new Core.Entities.QuestionOptions
				{
					Option = request.Option
				};

				var serviceEntity = service.FirstOrDefault();
				var optionList = serviceEntity.ServiceQuestions.Where(x => x.Question.Id == request.QuestionId2).FirstOrDefault().Question.QuestionOptions;
				optionList.Add(option);

				serviceEntity.ServiceQuestions.Where(x => x.Question.Id == request.QuestionId2).FirstOrDefault().Question.QuestionOptions = optionList;

				await _unitOfWork.Service.UpdateAsync(serviceEntity);

				await _unitOfWork.CommitAsync();
				return new BaseInsertOrUpdateResponse { Response = 1 }.SerializeObject();
			}
		}

		[HttpPost("GetUserPosts")]
		public async Task<string> GetUserPosts(GetUsersRequestDTO request)
		{
			var response = await _companyAndPersonService.GetUserPostsDatatable(request);

			return response.SerializeObject();
		}

		[HttpPost("GetUserPostById")]
		public async Task<string> GetUserPostById(GetUsersRequestDTO request)
		{
			var response = await _companyAndPersonService.GetUserPostById(request);

			return response.SerializeObject();
		}

		[HttpPost("NewPost")]
		public async Task<string> NewPost(NewPostRequest request)
		{
			var response = await _companyAndPersonService.NewPost(request);

			return response.SerializeObject();
		}

		[HttpPost("InsertPopUp")]
		public async Task<string> InsertPopUp(PopUpInsertRequest request)
		{
			var response = await _companyAndPersonService.InsertPopUp(request);

			return response.SerializeObject();
		}

		[HttpPost("DeletePostById")]
		public async Task<string> DeletePostById(DeleteByIdRequest request)
		{
			var response = await _companyAndPersonService.DeletePostById(request);

			return response.SerializeObject();
		}

		[HttpPost("LoginUserWeb")]
		public async Task<string> LoginUserWeb(NewLoginRequest request)
		{
			var response = await _companyAndPersonService.LoginUserWeb(request);

			return response.Data.SerializeObject();
		}


		[HttpGet("GetPackageList")]
		public async Task<string> GetPackageList()
		{
			var response = await _serviceService.GetPackageList();

			return response.Data.SerializeObject();
		}

		[HttpPost("GetPostByFollowingCompanyAndPerson")]
		public async Task<string> GetPostByFollowingCompanyAndPerson(GetUsersRequestDTO request)
		{
			var response = await _companyAndPersonService.GetUsersPostByFollowingCompanyAndPerson(request);
			return response.SerializeObject();
		}

		[HttpPost("GetUsersByPostCount")]
		public string GetUsersByPostCount(GetUsersRequestDTO request)
		{
			var response = _companyAndPersonService.GetUsersByPostCount(request);
			return response.SerializeObject();
		}

		[HttpPost("SendSms")]
		public async Task<string> SendSms(SmsRequestModel smsRequestModel)
		{
			var response = await _smsService.SendSmsAsync(smsRequestModel);
			return response.SerializeObject();
		}

		[Authorize]
		[HttpGet("test/{id}")]
		public string Test(int id)
		{
			var user = this.User;
			return "value";
		}

	}
}
