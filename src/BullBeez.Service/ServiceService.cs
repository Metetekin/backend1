﻿using AutoMapper;

using BullBeez.Core.DTO.MailDTO;
using BullBeez.Core.Entities;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;
using BullBeez.Core.UOW;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Service
{
    public class ServiceService : IServiceService
    {
        private readonly string StatusCode = ResponseCode.Basarili;
        private readonly string ResultMessage = "";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<ApiResult<List<ServiceListResponse>>> GetServiceList()
        {
            var response = await _unitOfWork.Service.GetAllServiceList();

            List<ServiceListResponse> responseList = new List<ServiceListResponse>();

            foreach (var item in response)
            {
                ServiceListResponse obj = new ServiceListResponse();
                obj.Id = item.Id;
                obj.ServiceName = item.ServiceName;
                obj.Amount = item.Amount;
                obj.OldAmount = item.Amount * 2;
                obj.ServiceIcon = item.ServiceIcon;
                obj.Description = item.Description;
                List<QuestionModel> questionList = new List<QuestionModel>();
                foreach (var itemQuestion in item.ServiceQuestions)
                {
                    QuestionModel objQuestion = new QuestionModel();
                    objQuestion.QuestionId = itemQuestion.Question.Id;
                    objQuestion.Question = itemQuestion.Question.Question;
                    objQuestion.QuestionType = itemQuestion.Question.QuestionType;
                    List<OptionModel> optionList = new List<OptionModel>();
                    foreach (var itemQuestionOptions in itemQuestion.Question.QuestionOptions)
                    {
                        optionList.Add(new OptionModel { OptionId = itemQuestionOptions.Id, Option = itemQuestionOptions .Option});
                    }
                    objQuestion.OptionList = optionList;
                    questionList.Add(objQuestion);
                }
                obj.QuestionList = questionList;

                responseList.Add(obj);
            }

            return new ApiResult<List<ServiceListResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseList
            };
        }

        public async Task<ApiResult<List<PackageResponse>>> GetPackageList()
        {
            var response = await _unitOfWork.Package.GetAll();

            List<PackageResponse> responseList = new List<PackageResponse>();

            foreach (var item in response)
            {
                PackageResponse obj = new PackageResponse();
                obj.Id = item.Id;
                obj.PackageName = item.PackageName;
                obj.Amount = item.Amount;
                obj.OldAmount = item.Amount * 2;
                obj.PackageIcon = item.PackageIcon;
                obj.Description = item.Description;
                
                responseList.Add(obj);
            }

            return new ApiResult<List<PackageResponse>>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseList
            };
        }



        public async Task<ApiResult<ServiceListResponse>> GetServiceById(int id)
        {
            var response = await _unitOfWork.Service.GetServiceById(id);

            ServiceListResponse responseData = new ServiceListResponse();

            foreach (var item in response)
            {
                responseData.Id = item.Id;
                responseData.ServiceName = item.ServiceName;
                responseData.Amount = item.Amount;
                responseData.OldAmount = item.Amount * 2;
                responseData.ServiceIcon = item.ServiceIcon;
                responseData.Description = item.Description;
                List<QuestionModel> questionList = new List<QuestionModel>();
                foreach (var itemQuestion in item.ServiceQuestions)
                {
                    QuestionModel objQuestion = new QuestionModel();
                    objQuestion.QuestionId = itemQuestion.Question.Id;
                    objQuestion.Question = itemQuestion.Question.Question;
                    objQuestion.QuestionType = itemQuestion.Question.QuestionType;
                    List<OptionModel> optionList = new List<OptionModel>();
                    foreach (var itemQuestionOptions in itemQuestion.Question.QuestionOptions)
                    {
                        optionList.Add(new OptionModel { OptionId = itemQuestionOptions.Id, Option = itemQuestionOptions.Option });
                    }
                    objQuestion.OptionList = optionList;
                    questionList.Add(objQuestion);
                }
                responseData.QuestionList = questionList;

            }

            return new ApiResult<ServiceListResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = responseData
            };
        }

        public async Task<ApiResult<CreatePayment>> CreatePayment(ServiceAnswerRequest request)
        {
            var companyAndPerson = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

            request.ServiceAnswers = request.ServiceAnswersString.DeserializeObject<List<ServiceAnswerModel>>();
            var guid = Guid.NewGuid(); 
            foreach (var item in request.ServiceAnswers)
            {
                var service = await _unitOfWork.Service.GetById(request.ServiceId);
                ServiceAnswers obj = new ServiceAnswers();
                obj.CompanyAndPerson = companyAndPerson;
                obj.IsPayment = 1;
                obj.QuestionoptionsId = item.OptionId;
                obj.TextData = item.Text;
                obj.ServiceId = request.ServiceId;
                obj.Amount = service.Amount;
                obj.Guid = guid;
                obj.ContractConfirmation = 1;

                await _unitOfWork.ServiceAnswers.AddAsync(obj);
            }

            MailHelper mailHelper = new MailHelper();
            mailHelper.SendMailGlobal(new MailRequest { EmailAddress = "asfasfas", Subject = "Servis Seçimi",Body="Bir kişi ödeme sayfasına kadar geldi ve kayıt atıldı. Kişinin id = " + companyAndPerson.Id });

            await _unitOfWork.CommitAsync();
            return new ApiResult<CreatePayment>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new CreatePayment
                {
                    CreatePaymentId = guid
                }
            };
        }


        public async Task<ApiResult<CreatePayment>> CreatePackagePayment(CreatePackagePaymentRequest request)
        {
            var companyAndPerson = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

            
            var guid = Guid.NewGuid();

            PackagePayments packagePayments = new PackagePayments();
            packagePayments.CompanyAndPerson = companyAndPerson;
            packagePayments.IsPayment = request.IsPayment;
            packagePayments.PackageId = request.PackageId;
            packagePayments.Amount = request.PaymentAmount;
            packagePayments.Guid = guid;
            packagePayments.ContractConfirmation = 1;


            await _unitOfWork.PackagePayment.AddAsync(packagePayments);
            
            MailHelper mailHelper = new MailHelper();
            mailHelper.SendMailGlobal(new MailRequest { EmailAddress = "asfasfas", Subject = "Paket Seçimi", Body = "Bir kişi ödeme sayfasına kadar geldi ve kayıt atıldı. Kişinin id = " + companyAndPerson.Id });

            await _unitOfWork.CommitAsync();
            return new ApiResult<CreatePayment>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new CreatePayment
                {
                    CreatePaymentId = guid
                }
            };
        }
    }
}