using AutoMapper;

using BullBeez.Core.DTO.MailDTO;
using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.RequestDTO.WebUIRequest;
using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse;
using BullBeez.Core.Services;
using BullBeez.Core.UOW;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Service
{
  public class CompanyAndPersonService : ICompanyAndPersonService
  {
    private readonly string StatusCode = ResponseCode.Basarili;
    private readonly string ResultMessage = "";
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;


    public CompanyAndPersonService(IUnitOfWork unitOfWork, IMapper mapper)
    {
      this._unitOfWork = unitOfWork;
      this._mapper = mapper;
    }





    public async Task<ApiResult<UserControlResponse>> UserControl(object baseRequest)
    {
      var jsonData = baseRequest.SerializeObject();
      var requestData = jsonData.DeserializeObject<BaseRequest>();

      var controlEntity = await _unitOfWork.Tokens.GetAllFilter(x => x.DeviceId == requestData.DeviceId && x.CompanyAndPerson.Id == requestData.UserId && x.TokenId == requestData.TokenId).ConfigureAwait(false);
      var response = controlEntity.OrderByDescending(x => x.Id).FirstOrDefault();

      return new ApiResult<UserControlResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new UserControlResponse
        {
          UserId = response == null ? 0 : requestData.UserId.Value
        }
      };
    }

    public async Task<ApiResult<UserInfoByUserIdResponse>> UserInfoByUserId(BaseRequest request)
    {
      try
      {
        var userData = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailById(request.UserId.Value);

        var userControlResponse = new UserInfoByUserIdResponse
        {
          UserId = userData.Id,
          NameOrTitle = userData.NameOrTitle,
          UserName = userData.UserName,
          EmailAddress = userData.EmailAddress,
          GSM = userData.GSM,
          BullbeezSentence = userData.BullbeezSentence,
          Status = userData.Status,
          Biography = userData.Biography,
          ProfileType = userData.ProfileType,
          CompanyTypeId = userData.CompanyType == null ? 0 : userData.CompanyType?.Id,
          CompanyTypeName = userData.CompanyType == null ? "" : userData.CompanyType?.Name,
          CompanyLevelId = userData.CompanyLevel == null ? 0 : userData.CompanyLevel?.Id,
          CompanyLevelName = userData.CompanyLevel == null ? "" : userData.CompanyLevel?.Name,
          CompanyDescription = userData.CompanyDescription,
          Occupation = userData.CompanyAndPersonOccupation.Count == 0 ? "" : userData.CompanyAndPersonOccupation.FirstOrDefault().Occupation.Name,
          OccupationId = userData.CompanyAndPersonOccupation.Count == 0 ? 0 : userData.CompanyAndPersonOccupation.FirstOrDefault().Occupation.Id,
          IsShowSkill = userData.CompanyAndPersonOccupation.Count == 0 ? 0 : userData.CompanyAndPersonOccupation.FirstOrDefault().Occupation.IsShowSkill,
          ProfileImage = string.IsNullOrEmpty(userData.ProfileImage) == true ? "https://i.hizliresim.com/7dstzi.jpg" : userData.ProfileImage,
          FollowCount = userData.Follows.Where(x => x.FollowType == EnumFollowType.Follows).Count(),
          Address = userData.Address.FirstOrDefault().AddressString,
          CountyId = userData.Address.FirstOrDefault().County.Id,
          CountyName = userData.Address.FirstOrDefault().County.Name,
          CityId = userData.Address.FirstOrDefault().County.City.Id,
          CityName = userData.Address.FirstOrDefault().County.City.Name,
          EstablishDate = userData.EstablishDate == null ? "" : userData.EstablishDate.Value.ToString("dd/MM/yyyy"),
          IsVenture = userData.CompanyType == null ? false : userData.CompanyType.IsVenture,
          WorkerCount = userData.WorkerCount,
          //FileName = userData.Files.Count == 0 ? "" : userData.Files.FirstOrDefault()?.FileName,
          //Base64String = userData.Files.Count == 0 ? null : Convert.ToBase64String(userData.Files.FirstOrDefault().ByteFile)

        };

        var file = await _unitOfWork.FileRepository.Find(x => x.CompanyAndPerson.Id == request.UserId.Value && x.RowStatu == EnumRowStatusType.Active).ConfigureAwait(false);

        if (file.Count() != 0)
        {
          userControlResponse.FileName = file.FirstOrDefault().FileName;
          userControlResponse.Base64String = Convert.ToBase64String(file.FirstOrDefault().ByteFile);
        }

        //var userFollowList = await _unitOfWork.Follows.Find(x => x.CompanyAndPerson.Id == request.UserId.Value).ConfigureAwait(false);
        //userControlResponse.FollowCount = userFollowList.Count();



        var workerList = await GetWorkerUserByUserId(new FollowUserByUserIdRequest { UserId = request.UserId.Value });
        userControlResponse.WorkerList = workerList.Data;
        ////interest bilgisi alınır
        var interestList = await _unitOfWork.Interests.GetAll().ConfigureAwait(false);

        var interestMapList = _mapper.Map<IEnumerable<Interest>, List<InterestListResponse>>(interestList);


        var userInterestList = userData.CompanyAndPersonInterests.Select(s => s.InterestId).ToArray();

        interestMapList.Where(x => userInterestList.Contains(x.Id)).ToList().ForEach(c => c.IsSelected = 1);

        userControlResponse.InterestList = interestMapList;


        ///Education bilgisi alınır
        userControlResponse.EducationList = _mapper.Map<IEnumerable<Education>, List<EducationResponse>>(userData.Education);


        ///skill bilgisi alınır
        var skillsList = await _unitOfWork.Skills.GetAll().ConfigureAwait(false);

        var skillsMapList = _mapper.Map<IEnumerable<Skill>, List<SkillResponse>>(skillsList);


        var userSkillList = userData.CompanyAndPersonSkills.Select(s => s.SkillId).ToArray();

        skillsMapList.Where(x => userSkillList.Contains(x.Id)).ToList().ForEach(c => c.IsSelected = 1);

        userControlResponse.SkillList = skillsMapList;


        //Proje Listesi ve ilgi alanları çekilir
        var projectList = userData.CompanyAndPersonProject.Select(a => a.Project).ToList();

        foreach (var projectItem in projectList)
        {
          var project = _mapper.Map<Project, ProjectResponse>(projectItem);
          var projectInterestList = projectItem.ProjectInterest.Select(s => s.InterestId).ToArray();

          var projectInterestMapList = _mapper.Map<IEnumerable<Interest>, List<InterestListResponse>>(interestList);

          projectInterestMapList.Where(x => projectInterestList.Contains(x.Id)).ToList().ForEach(c => c.IsSelected = 1);

          project.InterestList = projectInterestMapList;

          userControlResponse.ProjectList.Add(project);
        }

        return new ApiResult<UserInfoByUserIdResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = userControlResponse
        };
      }
      catch (Exception ex)
      {
        return new ApiResult<UserInfoByUserIdResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = null
        };
      }

    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> ResetPassword(BaseRequest request)
    {
      var userData = await _unitOfWork.CompanyAndPersons.SingleOrDefaultAsync(x => x.RowStatu == EnumRowStatusType.Active && x.EmailAddress == request.EmailAddress);

      if (userData == null)
      {
        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = "Böyle bir email bulunamadı.",
          IsError = true,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }

      Random generator = new Random();
      int newPassword = Convert.ToInt32(generator.Next(0, 999999).ToString("D6"));

      userData.Password = newPassword.ToString();
      userData.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(userData);

      await _unitOfWork.CommitAsync();

      MailHelper mailHelper = new MailHelper();
      mailHelper.SendResetPasswordMail(new MailRequest { EmailAddress = request.EmailAddress, Code = newPassword.ToString() });

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }


    public async Task<ApiResult<UserInfoByUserIdResponse>> UserShowByUserId(UserShowByUserIdRequest request)
    {
      var userData = await UserInfoByUserId(new BaseRequest { UserId = request.RequestUserId });
      var followData = await _unitOfWork.Follows.SingleOrDefaultAsync(x => x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.UserId && x.ToUserId == request.RequestUserId && x.FollowType == EnumFollowType.Follows).ConfigureAwait(false);

      userData.Data.IsFollow = followData == null ? 0 : 1;
      userData.Data.IsWorker = 0;
      var workerData = await _unitOfWork.Follows.SingleOrDefaultAsync(x => x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.UserId && x.ToUserId == request.RequestUserId && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType != EnumWorkerFollowType.Cancel).ConfigureAwait(false);

      if (workerData == null)
      {
        workerData = await _unitOfWork.Follows.SingleOrDefaultAsync(x => x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.RequestUserId && x.ToUserId == request.UserId && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType != EnumWorkerFollowType.Cancel).ConfigureAwait(false);
        if (workerData != null)
        {
          userData.Data.IsWorker = workerData == null ? 0 : (int)workerData.WorkerFollowType;
        }
      }
      else
      {
        userData.Data.IsWorker = workerData == null ? 0 : (int)workerData.WorkerFollowType;
      }

      await SendNotification(new SendNotificationModel { userId = request.RequestUserId, toUserId = request.UserId.Value, NotificationType = EnumNotificationType.Show, ProfileType = userData.Data.ProfileType });

      await _unitOfWork.CommitAsync();

      return userData;
    }

    public async Task<ApiResult<UploadCvResponse>> UploadCv(UploadCvRequest request)
    {
      var userData = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailAndFileById(request.UserId.Value);

      if (userData.Files.Count != 0)
      {
        foreach (var item in userData.Files)
        {
          item.RowStatu = EnumRowStatusType.SoftDeleted;
          await _unitOfWork.FileRepository.UpdateAsync(item);
        }

      }

      byte[] fileBytes;
      using (var memoryStream = new MemoryStream())
      {
        await request.File.CopyToAsync(memoryStream);
        fileBytes = memoryStream.ToArray();
      }

      var filename = request.File.FileName;
      var contentType = request.File.ContentType;


      var insertFile = new FileData();
      insertFile.CompanyAndPerson = userData;
      insertFile.FileName = request.File.FileName;
      insertFile.FileType = request.File.ContentType;
      insertFile.Size = 32532;
      insertFile.ByteFile = fileBytes;

      await _unitOfWork.FileRepository.AddAsync(insertFile);

      await _unitOfWork.CommitAsync();

      return new ApiResult<UploadCvResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new UploadCvResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<UploadCvResponse>> DeletePDF(UploadCvRequest request)
    {
      var userData = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailAndFileById(request.UserId.Value);

      if (userData.Files.Count != 0)
      {
        foreach (var item in userData.Files)
        {
          item.RowStatu = EnumRowStatusType.SoftDeleted;
          await _unitOfWork.FileRepository.UpdateAsync(item);
        }

      }

      await _unitOfWork.CommitAsync();

      return new ApiResult<UploadCvResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new UploadCvResponse { Response = 1 }
      };
    }

    #region PopUp işlemleri
    public async Task<ApiResult<PopUpResponse>> PopUpIsRead(PopUpIsReadRequest request)
    {
      var popup = await _unitOfWork.PopUp.SingleOrDefaultAsync(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.PageNumber == request.PageNumber).ConfigureAwait(false);

      if (popup == null)
      {
        return new ApiResult<PopUpResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = null
        };
      }

      var popupIsReadControl = await _unitOfWork.PopUpRead.SingleOrDefaultAsync(x => x.PopUpId == popup.Id && x.UserId == request.UserId).ConfigureAwait(false);
      PopUpResponse response = new PopUpResponse();
      if (popupIsReadControl == null)
      {
        //okumamııştır popup gönderilir
        response.Id = popup.Id;
        response.Body = popup.Body;
        response.Subject = popup.Subject;

        return new ApiResult<PopUpResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = response
        };
      }

      return new ApiResult<PopUpResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = null
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> PopUpReading(PopUpReadingRequest request)
    {
      await _unitOfWork.PopUpRead.AddAsync(new PopUpIsRead
      {
        PopUpId = request.PopUpId,
        UserId = request.UserId.Value
      });

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }
    #endregion

    #region User işlemleri
    public async Task<ApiResult<NewUserResponse>> NewUser(NewUserContract request)
    {
      try
      {
        var userToken = await TokenControl(request.EmailAddress);

        //var newUserContractCreate = _mapper.Map<NewUserContract, CompanyAndPerson>(request);
        //MailHelper mailHelper = new MailHelper();
        //mailHelper.SendMail(new MailRequest { EmailAddress = "aytugkonuralp@gmail.com", Code = "sdafasfas" });

        var companyTypeData = await _unitOfWork.CompanyType.GetById(request.CompanyType);

        var companyLevelData = request.CompanyLevel.Value == 0 ? null : await _unitOfWork.CompanyLevel.GetById(request.CompanyLevel.Value);


        var companyAndPerson = new CompanyAndPerson
        {
          NameOrTitle = request.Name,
          UserName = request.UserName,
          EmailAddress = request.EmailAddress,
          Password = request.Password,
          GSM = request.GSM,
          Status = request.Status,
          Biography = request.Biography,
          ProfileImage = request.ProfileImage,
          ProfileType = request.ProfileType,
          CompanyType = request.ProfileType == 1 ? null : companyTypeData,
          BirthDay = request.BirthDay,
          Gender = request.Gender
          //CompanyLevel = request.ProfileType == 1 ? null : companyLevelData
        };

        await _unitOfWork.CompanyAndPersons
            .AddAsync(companyAndPerson);

        await _unitOfWork.Tokens
            .AddAsync(new Token
            {
              DeviceId = request.DeviceId,
              CompanyAndPerson = companyAndPerson,
              TokenId = userToken.Token,
              BeginData = DateTime.Now,
              FirebaseToken = request.FirebaseToken
            });

        var countyData = await _unitOfWork.County.GetById(request.CountyId);

        await _unitOfWork.Address
            .AddAsync(new Address
            {
              AddressString = request.Address,
              CompanyAndPersons = companyAndPerson,
              County = countyData
            });


        if (request.OccupationId != 0)
        {
          var occupation = await _unitOfWork.Occupations.GetById(request.OccupationId);

          var componyAndPersonOccupation = new CompanyAndPersonOccupation(companyAndPerson, occupation);

          await _unitOfWork.ComponyAndPersonOccupationRepository.AddAsync(componyAndPersonOccupation);

        }

        if (!string.IsNullOrEmpty(request.Interests))
        {
          var interestList = request.Interests.Replace("#", "").Split(",").Select(Int32.Parse).ToArray();

          foreach (var item in interestList)
          {
            await _unitOfWork.CompanyAndPersonInterest
            .AddAsync(new CompanyAndPersonInterest
            {
              ComponyAndPerson = companyAndPerson,
              InterestId = item
            });
          }
        }

        await _unitOfWork.RequestHistory.AddAsync(new RequestHistory { UserId = companyAndPerson.Id, JsonRequest = request.SerializeObject() });

        await _unitOfWork.CommitAsync();

        return new ApiResult<NewUserResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new NewUserResponse
          {
            UserId = companyAndPerson.Id,
            Token = userToken.Token.ToString(),
            RefreshToken = userToken.RefreshToken
          }
        };
      }
      catch (Exception ex)
      {

        return new ApiResult<NewUserResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = ResultMessage,
          IsError = true,
          Data = new NewUserResponse
          {
            UserId = 0
          }
        };
      }

    }

    public async Task<ApiResult<NewUserResponse>> NewLogin(NewLoginRequest request)
    {
      var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

      var userToken = await TokenControl(user.EmailAddress);

      var userControlData = await _unitOfWork.MailApprovedCodes.GetAllFilter(x => x.CompanyAndPerson.Id == request.UserId && x.Code == request.ValidationCode).ConfigureAwait(false);

      var userControl = userControlData.ToList().OrderByDescending(x => x.Id).FirstOrDefault();

      if (userControl != null)
      {
        var addToken = _unitOfWork.Tokens.AddAsync(new Token
        {
          DeviceId = request.DeviceId,
          CompanyAndPerson = userControl.CompanyAndPerson,
          TokenId = userToken.Token,
          FirebaseToken = request.FirebaseToken
        });

        await _unitOfWork.RequestHistory.AddAsync(new RequestHistory { UserId = request.UserId.Value, FunctionName = "NewLogin", JsonRequest = request.SerializeObject() + "--" + userControl.SerializeObject() });


        await _unitOfWork.CommitAsync();
      }



      return new ApiResult<NewUserResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new NewUserResponse
        {
          UserId = userControl.CompanyAndPerson.Id,
          Token = userToken.Token.ToString(),
          RefreshToken = userToken.RefreshToken.ToString()
        }
      };
    }

    public async Task<ApiResult<NewUserResponse>> LoginUser(NewLoginRequest request)
    {
      var user = await _unitOfWork.CompanyAndPersons.GetAllFilter(x => x.EmailAddress == request.EmailAddress && x.Password == request.Password).ConfigureAwait(false);

      if (user.Count() == 0)
      {
        return new ApiResult<NewUserResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = "Kullanıcı Bulunamadı.",
          IsError = true,
          Data = new NewUserResponse
          {

          }
        };
      }

      var userToken = await TokenControl(request.EmailAddress);

      var addToken = _unitOfWork.Tokens.AddAsync(new Token
      {
        DeviceId = request.DeviceId,
        CompanyAndPerson = user.FirstOrDefault(),
        TokenId = userToken.Token,
        BeginData = DateTime.Now,
        FirebaseToken = request.FirebaseToken
      });

      await _unitOfWork.CommitAsync();

      return new ApiResult<NewUserResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new NewUserResponse
        {
          UserId = user.FirstOrDefault().Id,
          Token = userToken.Token.ToString(),
          ProfileType = user.FirstOrDefault().ProfileType,
          RefreshToken = userToken.RefreshToken.ToString()
        }
      };
    }

    public async Task<ApiResult<NewUserResponse>> ExternLoginUser(NewLoginRequest request)
    {
      var user = await _unitOfWork.CompanyAndPersons.GetAllFilter(x => x.EmailAddress == request.EmailAddress).ConfigureAwait(false);

      if (user.Count() == 0)
      {
        return new ApiResult<NewUserResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = "Kullanıcı Bulunamadı.",
          IsError = true,
          Data = new NewUserResponse
          {

          }
        };
      }

      var userToken = await TokenControl(request.EmailAddress);

      var addToken = _unitOfWork.Tokens.AddAsync(new Token
      {
        DeviceId = request.DeviceId,
        CompanyAndPerson = user.FirstOrDefault(),
        TokenId = userToken.Token,
        BeginData = DateTime.Now,
        FirebaseToken = request.FirebaseToken
      });

      await _unitOfWork.CommitAsync();

      return new ApiResult<NewUserResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new NewUserResponse
        {
          UserId = user.FirstOrDefault().Id,
          Token = userToken.Token.ToString(),
          ProfileType = user.FirstOrDefault().ProfileType,
          RefreshToken = userToken.RefreshToken.ToString()
        }
      };
    }

    public async Task<ApiResult<SendMailResponse>> IsMailExists(IsMailExistsRequest request)
    {
      CompanyAndPerson createUser = new CompanyAndPerson();
      var userToken = Guid.NewGuid();

      var userControl = await _unitOfWork.CompanyAndPersons.SingleOrDefaultAsync(x => x.EmailAddress == request.EmailAddress);

      if (userControl != null)
      {
        return new ApiResult<SendMailResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = "Mail adresi sitemde mevcut.",
          IsError = false,
          Data = new SendMailResponse { UserId = userControl.Id }
        };
      }
      //Bu kısımıda kullanıcı olsada olmasada kod gidiyor login veya create için geçerli
      Random generator = new Random();
      int randomCode = Convert.ToInt32(generator.Next(0, 9999).ToString("D4"));

      if (randomCode.ToString().Length != 4)
      {
        randomCode = Convert.ToInt32(generator.Next(0, 9999).ToString("D4"));
      }

      var resultJson = new SendMailResponse();
      resultJson.IsExistsUser = 0;
      resultJson.EmailAddress = request.EmailAddress;
      resultJson.ValidationCode = randomCode.ToString();

      if (userControl != null)
      {
        resultJson.UserId = userControl.Id;
        resultJson.IsExistsUser = 1;
        resultJson.ProfileType = userControl.ProfileType;
      }
      else
      {
        createUser = await _unitOfWork.CompanyAndPersons.GetById(1);
      }

      if ((!(request.Status == "No") && resultJson.IsExistsUser == 1) || resultJson.IsExistsUser == 0)
      {
        //Mail gönderme protokolü yapılacak ve tabloya kayıt atılacak
        await _unitOfWork.MailApprovedCodes.AddAsync(new MailApprovedCode
        {
          CompanyAndPerson = userControl == null ? createUser : userControl,
          Code = randomCode,
          EmailAddress = request.EmailAddress
        });

        MailHelper mailHelper = new MailHelper();
        mailHelper.SendMail(new MailRequest { EmailAddress = request.EmailAddress.ToLower(new CultureInfo("en-US", false)), Code = randomCode.ToString() });
        var code = randomCode;
      }
      await _unitOfWork.CommitAsync();
      return new ApiResult<SendMailResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = resultJson
      };
    }

    public async Task<ApiResult<UploadImageResponse>> UploadImage(UploadImageRequest request)
    {
      string cloud_name = "bullbeez-co";
      string ApiKey = "897556497718767";
      string ApiSecret = "pWtVMPB4uxzchmrkAJgj38dV8Gc";

      Account account = new Account(cloud_name, ApiKey, ApiSecret);
      Cloudinary cloudinary = new Cloudinary(account);
      cloudinary.Api.Timeout = int.MaxValue;

      var ImguploadParams = new ImageUploadParams()
      {
        File = new FileDescription(@"data:image/png;base64," + request.Base64Image)
      };


      var ImguploadResult = cloudinary.Upload(ImguploadParams);

      var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

      user.ProfileImage = ImguploadResult.SecureUrl.AbsoluteUri;
      user.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(user);

      var userFollowList = await _unitOfWork.Follows.GetAllFilterAndUserFollow(x => x.ToUserId == request.UserId.Value).ConfigureAwait(false);

      foreach (var followUser in userFollowList)
      {
        await SendNotification(new SendNotificationModel { userId = followUser.CompanyAndPerson.Id, toUserId = request.UserId.Value, ProfileType = user.ProfileType, NotificationType = EnumNotificationType.Information });
      }


      await _unitOfWork.CommitAsync();

      return new ApiResult<UploadImageResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new UploadImageResponse { ProfileImage = user.ProfileImage }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> Logout(BaseRequest request)
    {
      var tokenList = await _unitOfWork.Tokens.GetAllFilter(x => x.CompanyAndPerson.Id == request.UserId);


      foreach (var tokenItem in tokenList)
      {
        tokenItem.RowStatu = EnumRowStatusType.SoftDeleted;
        await _unitOfWork.Tokens.UpdateAsync(tokenItem);
      }

      await _unitOfWork.CommitAsync();
      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> TokenControl(BaseRequest request)
    {
      var tokenList = await _unitOfWork.Tokens.GetAllFilter(x => x.CompanyAndPerson.Id == request.UserId && x.DeviceId == request.DeviceId && x.TokenId == request.TokenId);

      if (tokenList == null)
      {
        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = "Token geçersiz. Başka bir cihazdan giriş yapılmış olabilir.",
          IsError = true,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }
      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<List<SearchUserByFilterResponse>>> SearchUserByFilter(SearchUserByFilterRequest request)
    {
      var allUserList = await _unitOfWork.CompanyAndPersons.SearchUserByFilter();


      if (request.OccupationId > 0)
      {
        allUserList = allUserList?.Where(x => x.CompanyAndPersonOccupation.Any(y => y.Occupation.Id == request.OccupationId)).ToList();
      }
      if (!string.IsNullOrEmpty(request.NameOrTitle))
      {
        allUserList = allUserList?.Where(x => x.NameOrTitle.ToLower().Contains(request.NameOrTitle.ToLower())).ToList();
      }
      if (request.ProfileType > 0)
      {
        allUserList = allUserList?.Where(x => x.ProfileType == request.ProfileType).ToList();
      }
      if (request.CityId > 0 && allUserList != null)
      {
        allUserList = allUserList?.Where(x => x.Address.Where(s => s.County.City.Id == request.CityId).Count() > 0).ToList();
      }
      if (!string.IsNullOrEmpty(request.Interests))
      {
        if (!request.Interests.Contains("Full"))
        {
          var interestArray = request.Interests.Replace("#", "").Split(',').Select(Int32.Parse).ToList();
          allUserList = allUserList?.Where(x => x.CompanyAndPersonInterests.Any(y => interestArray.Contains(y.InterestId))).ToList();
        }
      }
      if (!string.IsNullOrEmpty(request.CompanyType))
      {
        if (!request.CompanyType.Contains("Full"))
        {
          var companyList = request.CompanyType.Replace("#", "").Split(',').Select(Int32.Parse).ToList();
          allUserList = allUserList?.Where(x => companyList.Contains(x.CompanyType.Id)).ToList();
        }
      }

      if (!string.IsNullOrWhiteSpace(request.EmailAddress))
      {
        allUserList = allUserList?.Where(x => x.EmailAddress.ToLower().Contains(request.EmailAddress.ToLower())).ToList();
      }


      var response = _mapper.Map<IEnumerable<CompanyAndPerson>, List<SearchUserByFilterResponse>>(allUserList).OrderByDescending(x => x.BullbeezSentence).ToList();

      return new ApiResult<List<SearchUserByFilterResponse>>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = response
      };
    }

    #endregion

    #region Takipci işlemleri

    public async Task<ApiResult<FollowUserByUserIdResponse>> FollowUserByUserId(FollowUserByUserIdRequest request)
    {
      var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

      await _unitOfWork.Follows.AddAsync(new Follows { CompanyAndPerson = user, ToUserId = request.ToUserId, FollowType = request.FollowType, WorkerFollowType = request.WorkerFollowType, Position = request.Position, IsShow = true });

      if (request.FollowType == EnumFollowType.Follows)
      {
        await SendNotification(new SendNotificationModel
        {
          userId = request.ToUserId,
          toUserId = request.UserId.Value,
          ProfileType = user.ProfileType,
          NotificationType = request.FollowType == EnumFollowType.Follows ? EnumNotificationType.Follow : EnumNotificationType.Worker
        });
      }


      await _unitOfWork.CommitAsync();

      return new ApiResult<FollowUserByUserIdResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = null
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UnfollowUserByUserId(FollowUserByUserIdRequest request)
    {
      var followData = await _unitOfWork.Follows.SingleOrDefaultAsync(x => x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.UserId.Value && x.ToUserId == request.ToUserId).ConfigureAwait(false);

      var followData2 = await _unitOfWork.Follows.SingleOrDefaultAsync(x => x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.ToUserId && x.ToUserId == request.UserId).ConfigureAwait(false);

      if (followData == null && followData2 == null)
      {
        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }

      if (followData != null)
      {
        followData.UpdateRowStatuFollows(EnumRowStatusType.SoftDeleted);

        await _unitOfWork.Follows.UpdateAsync(followData);
      }
      if (followData2 != null)
      {
        followData2.UpdateRowStatuFollows(EnumRowStatusType.SoftDeleted);

        await _unitOfWork.Follows.UpdateAsync(followData2);
      }

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }



    public async Task<ApiResult<List<GetFollowUserByUserIdResponse>>> GetFollowUserByUserId(FollowUserByUserIdRequest request)
    {

      //var followData = await _unitOfWork.Follows.GetAllFilter(x => x.CompanyAndPerson.Id == request.UserId.Value && x.FollowType == EnumFollowType.Follows).ConfigureAwait(false); 

      var user = await _unitOfWork.Follows.GetAllFilterAndUserFollow(x => x.CompanyAndPerson.Id == request.UserId.Value && x.FollowType == EnumFollowType.Follows).ConfigureAwait(false);

      var toUserIdList = user.Select(s => s.ToUserId).ToArray();

      var followsList = await _unitOfWork.CompanyAndPersons.GetAllFilter(x => toUserIdList.Contains(x.Id));

      List<GetFollowUserByUserIdResponse> responseData = new List<GetFollowUserByUserIdResponse>();


      //bu kısım neden yapıldı 
      foreach (var item in followsList)
      {
        responseData.Add(new GetFollowUserByUserIdResponse
        {
          UserId = item.Id,
          Name = item.NameOrTitle,
          BullbeezSentence = item.BullbeezSentence,
          ProfileImage = string.IsNullOrEmpty(item.ProfileImage) == true ? "https://i.hizliresim.com/7dstzi.jpg" : item.ProfileImage,
          Occupation = item.CompanyAndPersonOccupation.Count == 0 ? "" : item.CompanyAndPersonOccupation.Where(x => x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Name,
          OccupationId = item.CompanyAndPersonOccupation.Count == 0 ? 0 : item.CompanyAndPersonOccupation.Where(x => x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Id,
          ProfileType = item.ProfileType
        });
      }

      return new ApiResult<List<GetFollowUserByUserIdResponse>>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = responseData
      };
    }

    public async Task<ApiResult<List<GetFollowUserByUserIdResponse>>> GetWorkerUserByUserId(FollowUserByUserIdRequest request)
    {
      //Firmanın çalışan olarak ekledikleri
      var addCompanyWorkerList = await _unitOfWork.Follows.GetAllFilterAndUserWorker(x => x.CompanyAndPerson.Id == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved).ConfigureAwait(false);

      //kişilerin firmaları ekledikleri
      var addUserWorkerList = await _unitOfWork.Follows.GetAllFilterAndUserWorker(x => x.ToUserId == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved).ConfigureAwait(false);

      var toUserIdList = addCompanyWorkerList.Select(s => s.ToUserId).ToList();

      var toUserIdList2 = addUserWorkerList.Select(s => s.CompanyAndPerson.Id).ToList();

      var followsList = await _unitOfWork.CompanyAndPersons.GetAllFilterCompanyTypeAndCompanyLevel(toUserIdList, toUserIdList2);

      List<GetFollowUserByUserIdResponse> responseData = new List<GetFollowUserByUserIdResponse>();


      //bu kısım neden yapıldı anlamadım
      foreach (var item in followsList)
      {
        var position = addCompanyWorkerList.Where(s => s.ToUserId == item.Id).Count() == 0 ? addUserWorkerList.Where(s => s.CompanyAndPerson.Id == item.Id).FirstOrDefault().Position : addCompanyWorkerList.Where(s => s.ToUserId == item.Id).FirstOrDefault().Position;
        var isShow = addCompanyWorkerList.Where(s => s.ToUserId == item.Id).Count() == 0 ? addUserWorkerList.Where(s => s.CompanyAndPerson.Id == item.Id).FirstOrDefault().IsShow : addCompanyWorkerList.Where(s => s.ToUserId == item.Id).FirstOrDefault().IsShow;

        responseData.Add(new GetFollowUserByUserIdResponse
        {
          UserId = item.Id,
          Name = item.NameOrTitle,
          BullbeezSentence = item.BullbeezSentence,
          ProfileImage = string.IsNullOrEmpty(item.ProfileImage) == true ? "https://i.hizliresim.com/7dstzi.jpg" : item.ProfileImage,
          Occupation = item.CompanyAndPersonOccupation.Count == 0 ? "" : item.CompanyAndPersonOccupation.Where(x => x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Name,
          OccupationId = item.CompanyAndPersonOccupation.Count == 0 ? 0 : item.CompanyAndPersonOccupation.Where(x => x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Id,
          ProfileType = item.ProfileType,
          Position = position,
          CompanyTypeId = item.CompanyType == null ? 0 : item.CompanyType?.Id,
          CompanyTypeName = item.CompanyType == null ? "" : item.CompanyType?.Name,
          CompanyLevelId = item.CompanyLevel == null ? 0 : item.CompanyLevel?.Id,
          CompanyLevelName = item.CompanyLevel == null ? "" : item.CompanyLevel?.Name,
          IsVenture = item.CompanyType == null ? false : item.CompanyType.IsVenture,
          IsShow = isShow
        });
      }

      return new ApiResult<List<GetFollowUserByUserIdResponse>>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = responseData
      };
    }

    public async Task<ApiResult<List<GetFollowUserByUserIdResponse>>> GetWaitingWorkerList(FollowUserByUserIdRequest request)
    {
      //Hesap sahibinin idsi gelecek ve todouserıd olarak arayacaz çünkü kim eklemiş
      var user = await _unitOfWork.Follows.GetAllFilterAndUserWorkerWaiting(x => x.ToUserId == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Waiting).ConfigureAwait(false);

      user = user.Where(x => x.WorkerFollowType == EnumWorkerFollowType.Waiting);
      var toUserIdList = user.Select(s => s.CompanyAndPerson.Id).ToArray();

      var followsList = await _unitOfWork.CompanyAndPersons.GetAllFilter(x => toUserIdList.Contains(x.Id));

      List<GetFollowUserByUserIdResponse> responseData = new List<GetFollowUserByUserIdResponse>();


      //bu kısım neden yapıldı anlamadım
      foreach (var item in followsList)
      {
        responseData.Add(new GetFollowUserByUserIdResponse
        {
          UserId = item.Id,
          Name = item.NameOrTitle,
          BullbeezSentence = item.BullbeezSentence,
          ProfileImage = string.IsNullOrEmpty(item.ProfileImage) == true ? "https://i.hizliresim.com/7dstzi.jpg" : item.ProfileImage,
          Occupation = item.CompanyAndPersonOccupation.Count == 0 ? "" : item.CompanyAndPersonOccupation.Where(x => x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Name,
          OccupationId = item.CompanyAndPersonOccupation.Count == 0 ? 0 : item.CompanyAndPersonOccupation.Where(x => x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Id,
          Message = item.ProfileType == 1 ? "şirketinizde çalıştığını beyan etti." : "sizi çalışanı olarak ekledi.",
          ProfileType = item.ProfileType,
          Position = user.Where(s => s.CompanyAndPerson.Id == item.Id).FirstOrDefault().Position
        });
      }

      return new ApiResult<List<GetFollowUserByUserIdResponse>>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = responseData
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> WorkerApprovedAndCancel(FollowUserByUserIdRequest request)
    {
      //Hesap sahibinin idsi gelecek ve todouserıd olarak arayacaz çünkü kim eklemiş
      var followData = await _unitOfWork.Follows.GetAllFilterAndUserWorkerWaiting(x => x.ToUserId == request.UserId.Value && x.CompanyAndPerson.Id == request.ToUserId && x.WorkerFollowType == EnumWorkerFollowType.Waiting).ConfigureAwait(false);

      followData = followData.Where(x => x.CompanyAndPerson.Id == request.ToUserId);

      if (request.WorkerFollowType == EnumWorkerFollowType.Approved)
      {
        var follows = followData.FirstOrDefault(x => x.WorkerFollowType == EnumWorkerFollowType.Waiting);
        follows.WorkerFollowType = request.WorkerFollowType;

        await _unitOfWork.Follows.UpdateAsync(follows);
      }
      else
      {
        var follows = followData.FirstOrDefault(x => x.WorkerFollowType == EnumWorkerFollowType.Waiting);
        follows.WorkerFollowType = request.WorkerFollowType;

        await _unitOfWork.Follows.UpdateAsync(follows);
      }


      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<ExistsUserNameResponse>> IsExistsUserName(IsExistsUserNameRequest request)
    {
      var userControl = await _unitOfWork.CompanyAndPersons.GetAllFilter(x => x.UserName == request.UserName).ConfigureAwait(false);

      return new ApiResult<ExistsUserNameResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new ExistsUserNameResponse
        {
          IsExists = userControl.ToList().Count == 0 ? 0 : 1
        }
      };
    }




    #endregion

    #region İlgi alanları işlemi
    public async Task<ApiResult<List<InterestListResponse>>> GetInterestsListByUserId(BaseRequest request)
    {
      var interestList = await _unitOfWork.Interests.GetAll().ConfigureAwait(false);

      var interestMapList = _mapper.Map<IEnumerable<Interest>, List<InterestListResponse>>(interestList);

      var userControl = await _unitOfWork.CompanyAndPersons.GetInterestsListByUserId(request).ConfigureAwait(false);

      var userInterestList = userControl.CompanyAndPersonInterests.Select(s => s.InterestId).ToArray();

      interestMapList.Where(x => userInterestList.Contains(x.Id)).ToList().ForEach(c => c.IsSelected = 1);
      //interestMapList.Where(x=> userInterestList.Contains(x.Id)).ToList().ForEach(c => { c.IsSelected = 1; c.HashId = "#" + c.Id.ToString() + "#"; });

      return new ApiResult<List<InterestListResponse>>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = interestMapList
      };
    }
    #endregion

    #region Bilidirim işlemleri
    public async Task<ApiResult<List<UserNotificationResponse>>> GetListNotificationByUserId(BaseRequest request)
    {
      var notifications = await _unitOfWork.Notifications.GetListNotificationByUserId(request).ConfigureAwait(false);

      var interestMapList = _mapper.Map<IEnumerable<Notification>, List<UserNotificationResponse>>(notifications);

      //Liste alındıktan sonra update edilir datalar
      notifications.ToList().ForEach(c => c.IsShow = 1);

      foreach (var item in notifications)
      {
        await _unitOfWork.Notifications.UpdateAsync(item);
      }


      await _unitOfWork.CommitAsync();

      return new ApiResult<List<UserNotificationResponse>>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = interestMapList
      };
    }
    #endregion

    #region Kullanıcı bilgi güncellemeleri

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateBullbeezSentenceByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value).ConfigureAwait(false);

      userInfo.BullbeezSentence = request.BullbeezSentence;
      userInfo.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(userInfo);

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateGSMByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value).ConfigureAwait(false);

      userInfo.GSM = request.GSM;
      userInfo.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(userInfo);

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateCompanyDescriptionByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value).ConfigureAwait(false);

      userInfo.CompanyDescription = request.CompanyDescription;
      userInfo.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(userInfo);

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateOccupationByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userOccopation = await _unitOfWork.ComponyAndPersonOccupationRepository.SingleOrDefaultAsync(x => x.CompanyAndPerson.Id == request.UserId.Value && x.RowStatu == EnumRowStatusType.Active).ConfigureAwait(false);
      userOccopation.OccupationId = request.OccupationId;
      await _unitOfWork.ComponyAndPersonOccupationRepository.UpdateAsync(userOccopation);

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateProfile(UpdateUserInfoByUserIdRequest request)
    {
      try
      {
        var userNameControl = await _unitOfWork.CompanyAndPersons.Find(x => x.UserName == request.UserName && x.RowStatu == EnumRowStatusType.Active && x.Id != request.UserId);

        //uyarı ver
        if (userNameControl.Count() > 0)
        {
          return new ApiResult<BaseInsertOrUpdateResponse>
          {
            StatusCode = StatusCode,
            Message = "Aynı kullanıcı adını başkası kullanıyor.",
            IsError = true,
            Data = new BaseInsertOrUpdateResponse { Response = 0 }
          };
        }

        var userInfo = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailById(request.UserId.Value);

        //uyarı ver
        if (userInfo == null)
        {
          return new ApiResult<BaseInsertOrUpdateResponse>
          {
            StatusCode = StatusCode,
            Message = "Kullanıcı bulunamadı.",
            IsError = true,
            Data = new BaseInsertOrUpdateResponse { Response = 0 }
          };
        }

        var companyLevelData = request.CompanyLevelId == 0 ? null : await _unitOfWork.CompanyLevel.GetById(request.CompanyLevelId);
        var companyTypeData = request.CompanyTypeId == 0 ? null : await _unitOfWork.CompanyType.GetById(request.CompanyTypeId);

        userInfo.NameOrTitle = request.Name;
        userInfo.UserName = request.UserName;
        userInfo.EmailAddress = request.EmailAddress;
        userInfo.GSM = request.GSM;
        userInfo.WorkerCount = request.WorkerCount;
        if (string.IsNullOrEmpty(request.EstablishDate) != true)
        {
          userInfo.EstablishDate = DateTime.ParseExact(request.EstablishDate, "dd/MM/yyyy", null);
        }
        if (string.IsNullOrEmpty(request.NewPassword) != true)
        {
          userInfo.Password = request.NewPassword;
        }
        userInfo.CompanyType = companyTypeData;
        userInfo.CompanyLevel = companyLevelData;
        userInfo.UpdatedDate = DateTime.Now;
        await _unitOfWork.CompanyAndPersons.UpdateAsync(userInfo);

        if (userInfo.ProfileType != 2)
        {

          var userOccopation = await _unitOfWork.ComponyAndPersonOccupationRepository.SingleOrDefaultAsync(x => x.CompanyAndPerson.Id == request.UserId.Value && x.RowStatu == EnumRowStatusType.Active).ConfigureAwait(false);

          userOccopation.OccupationId = request.OccupationId;
          await _unitOfWork.ComponyAndPersonOccupationRepository.UpdateAsync(userOccopation);
        }
        var countyData = await _unitOfWork.County.GetById(request.CountyId);
        var cityData = await _unitOfWork.City.GetById(request.CityId);
        countyData.City = cityData;

        var adress = userInfo.Address.FirstOrDefault();
        adress.County = countyData;
        adress.AddressString = request.Address;
        adress.CompanyAndPersons = userInfo;

        await _unitOfWork.Address
            .UpdateAsync(adress);


        if (!string.IsNullOrEmpty(request.IsShowCompanies))
        {
          var isShowWorkerCompanyId = request.IsShowCompanies.Replace("#", "").Split(",").Select(Int32.Parse).ToArray();
          //Firmanın çalışan olarak ekledikleri
          var addCompanyWorkerList = await _unitOfWork.Follows.GetAllFilterAndUserWorker(x => x.CompanyAndPerson.Id == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved).ConfigureAwait(false);

          foreach (var item in addCompanyWorkerList)
          {
            if (isShowWorkerCompanyId.Contains(item.ToUserId))
            {
              item.IsShow = true;
            }
            else
            {
              item.IsShow = false;
            }
            await _unitOfWork.Follows.UpdateAsync(item);
          }

          //kişilerin firmaları ekledikleri
          var addUserWorkerList = await _unitOfWork.Follows.GetAllFilterAndUserWorker(x => x.ToUserId == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved).ConfigureAwait(false);

          foreach (var item in addUserWorkerList)
          {
            if (isShowWorkerCompanyId.Contains(item.CompanyAndPerson.Id))
            {
              item.IsShow = true;
            }
            else
            {
              item.IsShow = false;
            }
            await _unitOfWork.Follows.UpdateAsync(item);
          }
        }
        else
        {
          //Firmanın çalışan olarak ekledikleri
          var addCompanyWorkerList = await _unitOfWork.Follows.GetAllFilterAndUserWorker(x => x.CompanyAndPerson.Id == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved).ConfigureAwait(false);

          foreach (var item in addCompanyWorkerList)
          {
            item.IsShow = false;
            await _unitOfWork.Follows.UpdateAsync(item);
          }

          //kişilerin firmaları ekledikleri
          var addUserWorkerList = await _unitOfWork.Follows.GetAllFilterAndUserWorker(x => x.ToUserId == request.UserId.Value && x.FollowType == EnumFollowType.Worker && x.WorkerFollowType == EnumWorkerFollowType.Approved).ConfigureAwait(false);

          foreach (var item in addUserWorkerList)
          {
            item.IsShow = false;
            await _unitOfWork.Follows.UpdateAsync(item);
          }
        }

        await _unitOfWork.RequestHistory.AddAsync(new RequestHistory { UserId = request.UserId.Value, FunctionName = "UpdateProfile", JsonRequest = request.SerializeObject() });

        await _unitOfWork.CommitAsync();

        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 1 }
        };
      }
      catch (Exception ex)
      {
        await _unitOfWork.RequestHistory.AddAsync(new RequestHistory { UserId = request.UserId.Value, FunctionName = "UpdateProfileEx", JsonRequest = request.SerializeObject() });

        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }

    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> NewPost(NewPostRequest request)
    {
      try
      {
        var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);
        //var topic_list = request.PostTopicsStr.DeserializeObject<List<string>>();

        UserPosts new_user_post = new UserPosts
        {
          CompanyAndPerson = user,
          PostText = request.PostText,
          PostMedia = request.PostMedia,
          PostTopics = request.PostTopicsStr,
          CreatedDate = DateTime.Now,
          LikeCount = 0,
          CommentCount = 0,
        };

        await _unitOfWork.UserPosts
            .AddAsync(new_user_post);

        await _unitOfWork.CommitAsync();

        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 1 }
        };
      }
      catch (Exception e)
      {
        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = ResultMessage,
          IsError = true,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }

    }
    public async Task<ApiResult<List<UserPostListResponse>>> GetUserPosts(GetUserPostsRequest request)
    {
      try
      {
        Dictionary<UserPosts, int> post_dict = new Dictionary<UserPosts, int>();

        //!< Postlari cekelim
        var userPosts = _unitOfWork.UserPosts.GetAll();

        int postScore = 0;

        //!< Siralamayi olusturalim
        foreach (UserPosts post in userPosts.Result)
        {
          postScore = 0;

          //!< Gecen dakika cinsinden zaman, ters oranli olarak post skoruna etki edecektir.
          postScore += (post.CreatedDate - DateTime.Now).Value.Minutes;

          //!< Like sayisi dogru orantili olarak post skoruna etki edecektir.
          postScore += post.LikeCount;

          //!< Dicte postu pushlayalim
          post_dict.Add(post, postScore);
        }

        //!< Postlari skorlara gore yeniden siralayalim
        var siralanmis_dict = post_dict.OrderByDescending(i => i.Value);

        //!< Kac adet dondurulecekse bunlari sinirlandiralim
        //!< Eger post sayisi istenenden fazla ise
        Dictionary<UserPosts, int> sinirlandirilmis_dict = new Dictionary<UserPosts, int>();
        if (request.RequestCount <= siralanmis_dict.Count())
        {
          sinirlandirilmis_dict = (Dictionary<UserPosts, int>)siralanmis_dict.Take(request.RequestCount);
        }
        //!< Degilse tum siralanmis dicti koyabiliriz.
        else
        {
          sinirlandirilmis_dict = (Dictionary<UserPosts, int>)siralanmis_dict;
        }

        //!< Veriyi elde ettik. Bunu simdi UserPostListResponse listesi haline getirelim.
        List<UserPostListResponse> dondurulecek_post_list = new List<UserPostListResponse>();

        foreach (UserPosts post in sinirlandirilmis_dict.Keys.ToArray())
        {
          //!< Postun yazarini cekelim
          CompanyAndPerson user = await _unitOfWork.CompanyAndPersons.GetById(post.CompanyAndPerson.Id);

          //!< Listeye postu ekleyelim
          dondurulecek_post_list.Add(new UserPostListResponse
          {
            AuthorName = user.NameOrTitle,
            AuthorOccupation = user.CompanyAndPersonOccupation.FirstOrDefault().Occupation.Name,
            AuthorUserName = user.UserName,
            PostText = post.PostText,
            PostMedia = post.PostMedia,
            PostTopics = post.PostTopics,
            PostCreatedDate = (DateTime)post.CreatedDate,
            PostLikeCount = post.LikeCount,
            PostCommentCount = post.CommentCount,
          });
        }

        //!< Yaniti dondurelim
        return new ApiResult<List<UserPostListResponse>>
        {
          StatusCode = ResponseCode.Basarili,
          Message = ResultMessage,
          IsError = true,
          Data = dondurulecek_post_list,
        };
      }
      catch (Exception e)
      {
        return new ApiResult<List<UserPostListResponse>>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = ResultMessage,
          IsError = true,
        };
      }
    }

    public async Task<List<PostDatatableModel>> GetUserPostsDatatable(GetUsersRequestDTO request)
    {
      var response = await _unitOfWork.UserPosts.GetPostsAndCompanyAndPersonData(request.search).ConfigureAwait(false);
      var count = response.Count();
      response = response.Skip(request.skip).Take(request.take);
      List<PostDatatableModel> responseList = new List<PostDatatableModel>();
      foreach (var item in response)
      {
        PostDatatableModel obj = new PostDatatableModel();
        obj.Id = item.Id;
        obj.PostText = item.PostText;
        obj.PostMedia = item.PostMedia;
        obj.NameOrTitle = item.CompanyAndPerson.NameOrTitle;
        obj.CountData = count;
        responseList.Add(obj);

      }
      return responseList;
    }

    /// <summary>
    /// Takip edilen kullanıcıların Postlarının gösterimi için kullanılır.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<PostDatatableModel>> GetUsersPostByFollowingCompanyAndPerson(GetUsersRequestDTO request)
    {

      //Parametreden gelen kullanıcının kimleri takip ettiğini çekiyoruz.
      var getFollowedUser = await GetFollowUserByUserId(new FollowUserByUserIdRequest { UserId = request.CompanyAndUserIds.FirstOrDefault() });
      //Takip edilen kullanıcıların ID'leri
      var toUserId = getFollowedUser.Data.Select(x => x.UserId).Distinct().ToArray();
      //Takip edilen kullanıcıların postları.
      var response = await _unitOfWork.UserPosts.GetPostByFollowingCompanyAndPerson(toUserId).ConfigureAwait(false);
      var count = response.Count();
      response = response.Skip(request.skip).Take(request.take);
      List<PostDatatableModel> responseList = new List<PostDatatableModel>();
      foreach (var item in response)
      {
        PostDatatableModel obj = new PostDatatableModel();
        obj.Id = item.Id;
        obj.PostText = item.PostText;
        obj.PostMedia = item.PostMedia;
        obj.NameOrTitle = item.CompanyAndPerson.NameOrTitle;
        obj.CountData = count;
        obj.CompanyAndPersonId = item.CompanyAndPerson.Id;
        responseList.Add(obj);

      }
      return responseList;
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> NewComment(NewCommentRequest request)
    {
      try
      {
        var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);
        var post = await _unitOfWork.UserPosts.GetById(request.PostId);

        PostComments new_comment = new PostComments
        {
          UserPosts = post,
          Text = request.Text,
          CompanyAndPerson = user,
          LikeCount = 0,
        };

        await _unitOfWork.PostComments
            .AddAsync(new_comment);

        await _unitOfWork.CommitAsync();

        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 1 }
        };
      }
      catch (Exception e)
      {
        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = ResponseCode.Basarisiz,
          Message = ResultMessage,
          IsError = true,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }
    }


    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateProjectByUserId(UpdateProjectByUserIdRequest request)
    {
      try
      {
        var userData = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailById(request.UserId.Value);


        //Projeler silinir
        foreach (var item in userData.CompanyAndPersonProject)
        {
          item.RowStatu = EnumRowStatusType.SoftDeleted;
          await _unitOfWork.CompanyAndPersonProject.UpdateAsync(item);
        }

        var interestLit = await _unitOfWork.Interests.GetAll();

        if (string.IsNullOrEmpty(request.ProjectListStr) == false)
        {
          List<ProjectResponse> newProjectList = new List<ProjectResponse>();
          try
          {
            newProjectList = request.ProjectListStr.DeserializeObject<List<ProjectResponse>>();
          }
          catch (Exception ex)
          {

            throw;
          }
          foreach (var newProjectItem in newProjectList)
          {
            var interestList = new List<ProjectInterest>();


            var project = new Project
            {
              ProjectName = newProjectItem.ProjectName,
              BeginDate = DateTime.ParseExact(newProjectItem.BeginDate, "dd/MM/yyyy", null),
              EndDate = DateTime.ParseExact(newProjectItem.EndDate, "dd/MM/yyyy", null),
              MonthCount = newProjectItem.MonthCount,
              ProjectDescription = newProjectItem.ProjectDescription,
              ProjectRoleName = newProjectItem.ProjectRoleName

            };

            foreach (var interestItem in newProjectItem.InterestList.Where(x => x.IsSelected == 1))
            {
              var data = interestLit.Where(x => x.Id == interestItem.Id).FirstOrDefault();
              interestList.Add(new ProjectInterest { InterestId = interestItem.Id, Interest = data, Project = project });
            }

            project.ProjectInterest = interestList;

            var companyAndPersonProject = new CompanyAndPersonProject { ComponyAndPersonId = request.UserId.Value, Project = project };
            await _unitOfWork.CompanyAndPersonProject.AddAsync(companyAndPersonProject);
          }
        }




        var userFollowList = await _unitOfWork.Follows.GetAllFilterAndUserFollow(x => x.ToUserId == request.UserId.Value).ConfigureAwait(false);

        foreach (var followUser in userFollowList)
        {
          await SendNotification(new SendNotificationModel { userId = followUser.CompanyAndPerson.Id, toUserId = request.UserId.Value, ProfileType = userData.ProfileType, NotificationType = EnumNotificationType.Information });
        }

        await _unitOfWork.CommitAsync();
        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 1 }
        };
      }
      catch (Exception ex)
      {

        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 0 }
        };
      }



    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateStatusByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value).ConfigureAwait(false);

      userInfo.Status = request.Status;
      userInfo.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(userInfo);

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateEducationByUserId(UpdateEducationByUserIdRequest request)
    {
      var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);
      var userEducationList = await _unitOfWork.Educations.GetAllFilter(x => x.CompanyAndPerson.Id == request.UserId.Value).ConfigureAwait(false);

      foreach (var item in userEducationList)
      {
        _unitOfWork.Educations.Remove(item);
      }

      if (string.IsNullOrEmpty(request.EducationListStr) == false)
      {
        var newEducationList = request.EducationListStr.DeserializeObject<List<EducationResponse>>();

        foreach (var item in newEducationList)
        {
          await _unitOfWork.Educations.AddAsync(new Education
          {
            CompanyAndPerson = user,
            BeginDate = DateTime.ParseExact(item.BeginDate, "dd/MM/yyyy", null),
            EndDate = DateTime.ParseExact(item.EndDate, "dd/MM/yyyy", null),
            SchoolName = item.SchoolName
          });
        }
      }


      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateInterestsByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAndInterestById(request.UserId.Value).ConfigureAwait(false);

      if (request.Interests == null)
      {
        foreach (var item in userInfo.CompanyAndPersonInterests)
        {
          _unitOfWork.CompanyAndPersonInterest.Remove(item);
        }
        await _unitOfWork.CommitAsync();

        return new ApiResult<BaseInsertOrUpdateResponse>
        {
          StatusCode = StatusCode,
          Message = ResultMessage,
          Data = new BaseInsertOrUpdateResponse { Response = 1 }
        };
      }
      var interestArray = request.Interests.Replace("#", "").Split(",");

      foreach (var item in userInfo.CompanyAndPersonInterests)
      {
        _unitOfWork.CompanyAndPersonInterest.Remove(item);
      }

      foreach (var item in interestArray)
      {
        if (!(item == "" || item == null))
        {
          await _unitOfWork.CompanyAndPersonInterest.AddAsync(new CompanyAndPersonInterest { ComponyAndPersonId = request.UserId.Value, InterestId = Convert.ToInt32(item) });

        }
      }

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateSkillsByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAndSkillById(request.UserId.Value).ConfigureAwait(false);

      var interestArray = request.Skills.Replace("#", "").Split(",");

      foreach (var item in userInfo.CompanyAndPersonSkills)
      {
        _unitOfWork.CompanyAndPersonSkill.Remove(item);
      }

      foreach (var item in interestArray.Where(x => x != ""))
      {
        await _unitOfWork.CompanyAndPersonSkill.AddAsync(new CompanyAndPersonSkills { ComponyAndPersonId = request.UserId.Value, SkillId = Convert.ToInt32(item) });
      }

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }

    public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateInformationByUserId(UpdateUserInfoByUserIdRequest request)
    {
      var userInfo = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value).ConfigureAwait(false);

      userInfo.Biography = request.Information;
      userInfo.UpdatedDate = DateTime.Now;
      await _unitOfWork.CompanyAndPersons.UpdateAsync(userInfo);

      await _unitOfWork.CommitAsync();

      return new ApiResult<BaseInsertOrUpdateResponse>
      {
        StatusCode = StatusCode,
        Message = ResultMessage,
        Data = new BaseInsertOrUpdateResponse { Response = 1 }
      };
    }
    #endregion

    #region WebAdmin
    public async Task<List<UserListResponse>> GetUsers(GetUsersRequestDTO request)
    {
      var response = await _unitOfWork.CompanyAndPersons.Find(x => x.NameOrTitle.Contains(request.search == null ? "" : request.search)).ConfigureAwait(false);
      var count = response.Count();
      response = response.Skip(request.skip).Take(request.take);
      var userList = _mapper.Map<IEnumerable<CompanyAndPerson>, List<UserListResponse>>(response);
      userList.Select(c => { c.CountData = count; return c; }).ToList();
      return userList;
    }


    public async Task<UserListResponse> GetUserById(int userId)
    {
      var response = await _unitOfWork.CompanyAndPersons.SingleOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);
      var userResponse = _mapper.Map<CompanyAndPerson, UserListResponse>(response);
      return userResponse;
    }

    /// <summary>
    /// En çok postu olan kullanıcıları sıralar
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public List<UserAndPostListReponse> GetUsersByPostCount(GetUsersRequestDTO request)
    {
      var query = _unitOfWork.GetRepository<UserPosts>().Where(predicate: x => x.RowStatu == EnumRowStatusType.Active)
        .Include(x => x.CompanyAndPerson).AsEnumerable()
        .GroupBy(x => x.CompanyAndPerson).OrderByDescending(x => x.Count())
        .Skip(request.skip).Take(request.take > 0 ? request.take : 20); ;
            
      
      List<UserAndPostListReponse> response = new List<UserAndPostListReponse>();
      foreach (var user in query)
      {
        UserAndPostListReponse data = new UserAndPostListReponse
        {
          User = _mapper.Map<CompanyAndPerson, UserListResponse>(user.Key),
          PostCount = user.Count()
        };

        response.Add(data);
      }

      return response;

    }

    #endregion

    #region Fonksiyonlar
    public async Task<string> SendNotification(SendNotificationModel sendNotificationModel)
    {
      var user = await _unitOfWork.CompanyAndPersons.GetById(sendNotificationModel.userId);

      var toUserInfo = await _unitOfWork.CompanyAndPersons.GetById(sendNotificationModel.toUserId);

      var message = "";

      if (sendNotificationModel.NotificationType == EnumNotificationType.Information)
      {
        message = toUserInfo.NameOrTitle + " bilgilerini güncelledi.";
      }
      else if (sendNotificationModel.NotificationType == EnumNotificationType.Follow)
      {
        message = toUserInfo.NameOrTitle + " seni takip edilenlerine ekledi.";
      }
      else if (sendNotificationModel.NotificationType == EnumNotificationType.Show)
      {
        message = toUserInfo.NameOrTitle + " profilini görüntüledi.";
      }
      else if (sendNotificationModel.NotificationType == EnumNotificationType.Worker)
      {
        message = toUserInfo.NameOrTitle + " seni çalışanı olarak ekledi.";
      }

      await _unitOfWork.Notifications.AddAsync(new Notification { CompanyAndPerson = user, ToUserId = toUserInfo.Id, Message = message, IsShow = 0, ProfileImage = toUserInfo.ProfileImage, ProfileType = toUserInfo.ProfileType });

      return "";
    }

    public async Task<TokenModel> TokenControl(string emailAddress)
    {
      var claimsdata = new[] { new Claim(ClaimTypes.Name, emailAddress + emailAddress) };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ahbasshfbsahjfbshajbfhjasbfashjbfsajhfvashjfashfbsahfbsahfksdjf"));
      var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
      var token = new JwtSecurityToken(
           issuer: "bullbeez.com",
           audience: "bullbeez.com",
           expires: DateTime.Now.AddMinutes(90),
           claims: claimsdata,
           signingCredentials: signInCred
          );
      var tokenInstance = new TokenModel();
      tokenInstance.Token = new JwtSecurityTokenHandler().WriteToken(token);
      //Refresh Token üretiyoruz.
      tokenInstance.RefreshToken = CreateRefreshToken();

      return tokenInstance;
    }

    //Refresh Token üretecek metot.
    public string CreateRefreshToken()
    {
      byte[] number = new byte[32];
      using (RandomNumberGenerator random = RandomNumberGenerator.Create())
      {
        random.GetBytes(number);
        return Convert.ToBase64String(number);
      }
    }
    #endregion

  }
}
