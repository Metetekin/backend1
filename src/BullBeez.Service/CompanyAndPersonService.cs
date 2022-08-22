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

using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

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
        private readonly INotificationHelper _notificationHelper;

      
        public CompanyAndPersonService(IUnitOfWork unitOfWork, IMapper mapper, INotificationHelper notificationHelper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._notificationHelper = notificationHelper;
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

        public async Task<ApiResult<UserInfoByUserIdResponse>> GetUserByUsername(UserInfoByUsernameRequest request)
        {
            try
            {
                var user = await _unitOfWork.CompanyAndPersons.GetByUserName(request.UserName);

                var response = new UserInfoByUserIdResponse
                {
                    UserId      = user.Id,
                    ProfileType = user.ProfileType
                };

                return new ApiResult<UserInfoByUserIdResponse>
                {
                    StatusCode  = StatusCode,
                    Message     = ResultMessage,
                    Data        = response
                };
            }
            catch(Exception e)
            {
                return new ApiResult<UserInfoByUserIdResponse>
                {
                    StatusCode  = StatusCode,
                    Message     = ResultMessage,
                    Data        = null
                };
            }
            
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
                    Address = userData.Address.Count == 0 ? "" : userData.Address.FirstOrDefault().AddressString,
                    CountyId = userData.Address.FirstOrDefault().County == null ? 0 : userData.Address.FirstOrDefault().County.Id,
                    CountyName = userData.Address.FirstOrDefault().County == null ? "" : userData.Address.FirstOrDefault().County.Name,
                    CityId = userData.Address.FirstOrDefault().County.City == null ? 0 : userData.Address.FirstOrDefault().County.City.Id,
                    CityName = userData.Address.FirstOrDefault().County.City == null ? "" : userData.Address.FirstOrDefault().County.City.Name,
                    EstablishDate = userData.EstablishDate == null ? "" : userData.EstablishDate.Value.ToString("dd/MM/yyyy"),
                    IsVenture = userData.CompanyType == null ? false : userData.CompanyType.IsVenture,
                    WorkerCount = userData.WorkerCount,
                    MailPermission = userData.MailPermission,
                    BannerImage = string.IsNullOrEmpty(userData.BannerImage) == true ? "https://i.hizliresim.com/mos64c5.jpg" : userData.BannerImage,
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
                    Data = new BaseInsertOrUpdateResponse{ Response = 0}
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
            var userData = await UserInfoByUserId(new BaseRequest { UserId = request.RequestUserId});
            var followData = await _unitOfWork.Follows.SingleOrDefaultAsync(x=> x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.UserId && x.ToUserId == request.RequestUserId && x.FollowType == EnumFollowType.Follows).ConfigureAwait(false);
           
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

            //!< Goruntuleyen, goruntulenenden farkliysa bildirim gonderelim
            if(request.RequestUserId != request.UserId.Value)
            {
                await SendNotification(new SendNotificationModel { userId = request.RequestUserId, toUserId = request.UserId.Value, NotificationType = EnumNotificationType.Show, ProfileType = userData.Data.ProfileType });
            }

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
            var popup = await _unitOfWork.PopUp.SingleOrDefaultAsync(x=> x.StartDate<= DateTime.Now && x.EndDate >= DateTime.Now && x.PageNumber == request.PageNumber).ConfigureAwait(false);

            if (popup == null )
            {
                return new ApiResult<PopUpResponse>
                {
                    StatusCode = StatusCode,
                    Message = ResultMessage,
                    Data = null
                };
            }

            PopUpResponse response = new PopUpResponse();

            //!< Eger servisler popu gosterilecekse ozel durum var, her turlu popup gonderilir.
            if (request.PageNumber == 2)
            {
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

            //!< Aksi halde, okunmamissa popup gonderilir.
            var popupIsReadControl = await _unitOfWork.PopUpRead.SingleOrDefaultAsync(x => x.PopUpId == popup.Id && x.UserId == request.UserId).ConfigureAwait(false);
            
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

        public async Task<ApiResult<PopUpResponse>> InsertPopUp(PopUpInsertRequest request)
        {
            await _unitOfWork.PopUp.AddAsync(new PopUp { Body = request.Body, Subject= request.Subject,
                StartDate = DateTime.ParseExact(request.StartDate,"yyyy-MM-dd",System.Globalization.CultureInfo.InvariantCulture),
                EndDate= DateTime.ParseExact(request.EndDate, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture), PageNumber = request.PageNumber });
            await _unitOfWork.CommitAsync();
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
                Data = new BaseInsertOrUpdateResponse { Response = 1}
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

            var userControl = userControlData.ToList().OrderByDescending(x=> x.Id).FirstOrDefault();

            if (userControl != null)
            {
                var addToken = _unitOfWork.Tokens.AddAsync(new Token
                {
                    DeviceId = request.DeviceId,
                    CompanyAndPerson = userControl.CompanyAndPerson,
                    TokenId = userToken.Token,
                    FirebaseToken = request.FirebaseToken
                });

                await _unitOfWork.RequestHistory.AddAsync(new RequestHistory { UserId = request.UserId.Value, FunctionName = "NewLogin", JsonRequest = request.SerializeObject()+"--" + userControl.SerializeObject() });


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

        public async Task<ApiResult<NewUserResponse>> LoginUserWeb(NewLoginRequest request)
        {
            var user = await _unitOfWork.CompanyAndPersons.GetAllFilter(x => x.EmailAddress == request.EmailAddress && x.Password == request.Password && x.IsAdmin == true).ConfigureAwait(false);

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

          

            return new ApiResult<NewUserResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new NewUserResponse
                {
                    UserId = user.FirstOrDefault().Id
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
            string message = "";
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
                var message2 = await mailHelper.SendMail(new MailRequest { EmailAddress = request.EmailAddress.ToLower(new CultureInfo("en-US", false)), Code = randomCode.ToString() });
                message = message2.ToString();
                var code = randomCode;
            }
            await _unitOfWork.CommitAsync();
            return new ApiResult<SendMailResponse>
            {
                StatusCode = StatusCode,
                Message = message,
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
                await SendNotification(new SendNotificationModel { userId = followUser.CompanyAndPerson.Id, toUserId = request.UserId.Value,ProfileType = user.ProfileType, NotificationType = EnumNotificationType.Information });
            }


            await _unitOfWork.CommitAsync();

            return new ApiResult<UploadImageResponse>
            {
                StatusCode = StatusCode,
                Message = ResultMessage,
                Data = new UploadImageResponse { ProfileImage = user.ProfileImage }
            };
        }

        public async Task<ApiResult<UploadImageResponse>> UploadBannerImage(UploadImageRequest request)
        {
            string cloud_name   = "bullbeez-co";
            string ApiKey       = "897556497718767";
            string ApiSecret    = "pWtVMPB4uxzchmrkAJgj38dV8Gc";

            Account account         = new Account(cloud_name, ApiKey, ApiSecret);
            Cloudinary cloudinary   = new Cloudinary(account);
            cloudinary.Api.Timeout  = int.MaxValue;

            var ImguploadParams = new ImageUploadParams()
            {
                File = new FileDescription(@"data:image/png;base64," + request.Base64Image)
            };


            var ImguploadResult = cloudinary.Upload(ImguploadParams);

            var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);

            user.BannerImage    = ImguploadResult.SecureUrl.AbsoluteUri;
            user.UpdatedDate    = DateTime.Now;

            await _unitOfWork.CompanyAndPersons.UpdateAsync(user);

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
                Data = new BaseInsertOrUpdateResponse { Response = 1}
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

                    var broke_list = allUserList?.Where(x => x.CompanyType == null && x.ProfileType == 2).ToList();
                    foreach(CompanyAndPerson item in broke_list)
                    {
                        var companyTypeData = await _unitOfWork.CompanyType.GetById(1);
                        item.CompanyType = companyTypeData;
                        //await _unitOfWork.CompanyAndPersons.UpdateAsync(item);
                    }
                    //await _unitOfWork.CommitAsync();

                    allUserList = allUserList?.Where(x => companyList.Contains(x.CompanyType.Id)).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(request.EmailAddress))
            {
                allUserList = allUserList?.Where(x => x.EmailAddress.ToLower().Contains(request.EmailAddress.ToLower())).ToList();
            }

            var response = _mapper.Map<IEnumerable<CompanyAndPerson>, List<SearchUserByFilterResponse>>(allUserList).OrderByDescending(x=> x.BullbeezSentence).ToList();

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

            await _unitOfWork.Follows.AddAsync(new Follows { CompanyAndPerson = user, ToUserId = request.ToUserId ,FollowType = request.FollowType, WorkerFollowType = request.WorkerFollowType, Position = request.Position, IsShow = true });

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
            var followData = await _unitOfWork.Follows.SingleOrDefaultAsync(x=> x.RowStatu == EnumRowStatusType.Active && x.CompanyAndPerson.Id == request.UserId.Value && x.ToUserId == request.ToUserId).ConfigureAwait(false);

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
                    Occupation = item.CompanyAndPersonOccupation.Count == 0 ? "" : item.CompanyAndPersonOccupation.Where(x=> x.RowStatu == EnumRowStatusType.Active).FirstOrDefault().Occupation.Name,
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

            followData = followData.Where(x=> x.CompanyAndPerson.Id == request.ToUserId);

            if (request.WorkerFollowType == EnumWorkerFollowType.Approved)
            {
                var follows = followData.FirstOrDefault(x=> x.WorkerFollowType == EnumWorkerFollowType.Waiting);
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
                Data = new BaseInsertOrUpdateResponse { Response = 1}
            };
        }

        public async Task<ApiResult<ExistsUserNameResponse>> IsExistsUserName(IsExistsUserNameRequest request)
        {
            var userControl = await _unitOfWork.CompanyAndPersons.GetAllFilter(x=> x.UserName == request.UserName).ConfigureAwait(false);

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

            interestMapList.Where(x => userInterestList.Contains(x.Id)).ToList().ForEach(c =>  c.IsSelected = 1);
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

                    var userOccopation = await _unitOfWork.ComponyAndPersonOccupationRepository.SingleOrDefaultAsync(x=> x.CompanyAndPerson.Id == request.UserId.Value && x.RowStatu == EnumRowStatusType.Active).ConfigureAwait(false);
                    
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

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> NewPostReport(NewPostReportRequest request)
        {
            try
            {
                PostReport new_user_report = new PostReport
                {
                    PostId      = request.PostId,
                    ReasonId    = request.ReasonId,
                    ReasonText  = request.ReasonText,
                };

                await _unitOfWork.PostReport
                    .AddAsync(new_user_report);

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

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> NewPost(NewPostRequest request)
        {
            try
            {

                ImageUploadResult ImguploadResult = new ImageUploadResult();

                //!< Media var mi?
                if (request.PostMedia != null)
                {
                    string cloud_name = "bullbeez-co";
                    string ApiKey = "897556497718767";
                    string ApiSecret = "pWtVMPB4uxzchmrkAJgj38dV8Gc";

                    Account account = new Account(cloud_name, ApiKey, ApiSecret);
                    Cloudinary cloudinary = new Cloudinary(account);
                    cloudinary.Api.Timeout = int.MaxValue;

                    var ImguploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(@"data:image/png;base64," + request.PostMedia)
                    };


                    ImguploadResult = cloudinary.Upload(ImguploadParams);
                }

                var user = await _unitOfWork.CompanyAndPersons.GetById(request.UserId.Value);
                //var topic_list = request.PostTopicsStr.DeserializeObject<List<string>>();
                
                UserPosts new_user_post = new UserPosts
                {
                    CompanyAndPerson    = user,
                    PostText            = request.PostText,
                    PostMedia           = request.PostMedia != null ? ImguploadResult.SecureUrl.AbsoluteUri : "",
                    PostTopics          = request.PostTopicsStr,
                    CreatedDate         = DateTime.Now,
                    LikeCount           = 0,
                    CommentCount        = 0,
                    UserIdWhoLike       = ",",
                    IsSponsoredPost     = request.IsSponsoredPost,
                    IsUpgradedToBoard   = request.IsUpgradedToBoard,
                    SponsoredTitle      = request.SponsoredTitle,
                };

                await _unitOfWork.UserPosts
                    .AddAsync(new_user_post);

                await _unitOfWork.CommitAsync();

                //postu atan kişiyi takip edenler çekilir
                var getUserFollowList = await _unitOfWork.Follows.GetFollowUserToken(request.UserId.Value);

                foreach (var item in getUserFollowList)
                {
                    _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                    {
                        Title = "İlgini çekebilir!",
                        Body = "Bağlantılarından " + user.NameOrTitle + ", yeni bir gönderi paylaştı!",
                        DeviceFirebaseToken = item,
                        PostId = new_user_post.Id.ToString()
                    });
                }


                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = StatusCode,
                    Message     = ResultMessage,
                    Data        = new BaseInsertOrUpdateResponse { Response = 1 }
                };
            }
            catch(Exception e)
            {
                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                    Data        = new BaseInsertOrUpdateResponse { Response = 0 }
                };
            }
            
        }
        
        public async Task<ApiResult<BaseInsertOrUpdateResponse>> UpgradePostToBoardById(UpgradePostRequest request)
        {
            try
            {
                var post = _unitOfWork.UserPosts.GetDetailedById(request.PostId).Result;

                //!< Bu istek gercekten post sahibinden geldiyse
                if(post.CompanyAndPerson.Id == request.UserId)
                {
                    //!< Bu bir upgrade ise
                    if (request.IsUpgrade)
                    {
                        post.IsUpgradedToBoard = true;
                    }
                    //!< Upgrade degilse
                    else
                    {
                        post.IsUpgradedToBoard = false;
                    }

                    await _unitOfWork.UserPosts.UpdateAsync(post);

                    await _unitOfWork.CommitAsync();
                }

                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = StatusCode,
                    Message     = ResultMessage,
                    Data        = new BaseInsertOrUpdateResponse { Response = 1 }
                };
            }
            catch(Exception e)
            {
                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                    Data        = new BaseInsertOrUpdateResponse { Response = 0 }
                };
            }
        }

        public async Task<ApiResult<List<PostCommentsByPostIdResponse>>> GetMoreCommentsByPostId(GetMoreCommentsRequest request)
        {
            try
            {
                //!< Commentleri cekelim ve tarihe gore siralayalim
                var comments = _unitOfWork.PostComments.GetByOffset(request.PostId, request.StartIdx, request.Count).Result;

                List<PostCommentsByPostIdResponse> dondurulecek_comment_list = new List<PostCommentsByPostIdResponse>();

                int created_date = 0;

                string[] kullanici_idleri;
                bool begenmis_mi;

                foreach (PostComments comment in comments)
                {
                    var user    = _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailById(comment.CompanyAndPerson.Id).Result;

                    created_date = Convert.ToInt32((DateTime.Now - comment.InsertedDate).Value.TotalSeconds);

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = comment.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }

                    dondurulecek_comment_list.Add(new PostCommentsByPostIdResponse
                    {
                        CommentId           = comment.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorLogo          = user.ProfileImage,
                        AuthorUserName      = user.UserName,
                        AuthorProfileType   = user.ProfileType,
                        AuthorUserId        = user.Id,
                        AuthorOccupation    = user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name,
                        CommentText         = comment.Text,
                        CommentCreatedDate  = created_date,
                        CommentLikeCount    = comment.LikeCount,
                        IsLiked             = begenmis_mi
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<PostCommentsByPostIdResponse>>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = ResultMessage,
                    Data = dondurulecek_comment_list,
                };
            }
            catch (Exception e)
            {
                return new ApiResult<List<PostCommentsByPostIdResponse>>
                {
                    StatusCode = ResponseCode.Basarisiz,
                    Message = ResultMessage,
                    IsError = true,
                };
            }
        }

        public async Task<ApiResult<List<PostCommentsByPostIdResponse>>> GetPostCommentsByPostId(GetPostCommentsByPostIdRequest request)
        {
            try
            {
                //!< Commentleri cekelim ve tarihe gore siralayalim
                var comments = _unitOfWork.PostComments.GetAllFilter(x => x.UserPosts.Id == request.PostId).Result.OrderBy(a => a.InsertedDate);

                List<PostCommentsByPostIdResponse> dondurulecek_comment_list = new List<PostCommentsByPostIdResponse>();
                int created_date = 0;

                string[] kullanici_idleri;
                bool begenmis_mi;

                for (int i = 0; i < Int32.Parse(request.Count) && i < comments.Count(); i++)
                {
                    var user    = _unitOfWork.CompanyAndPersons.GetCompanyAndPersonAllDetailById(comments.ElementAt(i).CompanyAndPerson.Id).Result;
                    var comment = comments.ElementAt(i);

                    created_date = Convert.ToInt32((DateTime.Now - comment.InsertedDate).Value.TotalSeconds);

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = comment.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }

                    dondurulecek_comment_list.Add(new PostCommentsByPostIdResponse
                    {
                        CommentId           = comment.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorLogo          = user.ProfileImage,
                        AuthorUserName      = user.UserName,
                        AuthorProfileType   = user.ProfileType,
                        AuthorUserId        = user.Id,
                        AuthorOccupation    = user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name,
                        CommentText         = comment.Text,
                        CommentCreatedDate  = created_date,
                        CommentLikeCount    = comment.LikeCount,
                        IsLiked             = begenmis_mi
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<PostCommentsByPostIdResponse>>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = ResultMessage,
                    Data = dondurulecek_comment_list,
                };
            }
            catch(Exception e)
            {
                return new ApiResult<List<PostCommentsByPostIdResponse>>
                {
                    StatusCode = ResponseCode.Basarisiz,
                    Message = ResultMessage,
                    IsError = true,
                };
            }
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> LikePostById(LikeByIdRequest request)
        {
            try
            {
                //!< Postu getirelim
                var begenilecek_post = _unitOfWork.UserPosts.GetById(request.Id).Result;

                //!< begen
                if(true == request.Like)
                {
                    //!< Eger zaten bu postu begenmissek ve bir hata sonucu tekrar begenme istegi geldiyse direkt return edelim
                    if(begenilecek_post.UserIdWhoLike.Contains(request.UserId.ToString()))
                    {
                        return new ApiResult<BaseInsertOrUpdateResponse>
                        {
                            StatusCode  = ResponseCode.Basarili,
                            Message     = ResultMessage,
                        };
                    }

                    begenilecek_post.UserIdWhoLike += request.UserId.ToString() + ",";
                    begenilecek_post.LikeCount += 1;

                    //!< Post sahibine bildirim gonderelim
                    var user            = _unitOfWork.CompanyAndPersons.GetById(begenilecek_post.CompanyAndPerson.Id).Result;
                    var request_user    = _unitOfWork.CompanyAndPersons.GetById((int)request.UserId).Result;

                    _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                    {
                        Title = "Ekosistemde fark ediliyorsun",
                        Body = request_user.NameOrTitle + ", gönderini beğendi.",
                        DeviceFirebaseToken = user.Tokens.OrderByDescending(x => x.Id).FirstOrDefault().FirebaseToken,
                        PostId = begenilecek_post.Id.ToString()
                    });
                }
                //!< begeniyi geri al
                else
                {
                    string   useridswholike = begenilecek_post.UserIdWhoLike;
                    string   newids         = ",";
                    string[] useridsarray   = useridswholike.Split(",");
                    if(useridsarray.Contains(request.UserId.ToString()))
                    {
                        foreach(string item in useridsarray)
                        {
                            if(item != request.UserId.ToString() && item != "")
                            {
                                newids += item + ",";
                            }
                        }

                        begenilecek_post.UserIdWhoLike = newids;

                        begenilecek_post.LikeCount -= 1;
                    }
                }
                
                begenilecek_post.UpdatedDate =  DateTime.Now;

                await _unitOfWork.UserPosts.UpdateAsync(begenilecek_post);

                await _unitOfWork.CommitAsync();

                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = ResultMessage,
                };
            }
            catch (Exception e)
            {
                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode = ResponseCode.Basarisiz,
                    Message = ResultMessage,
                    IsError = true,
                };
            }
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> DeletePostById(DeleteByIdRequest request)
        {
            try
            {
                var silinecek_post = _unitOfWork.UserPosts.GetById(request.Id).Result;

                silinecek_post.RowStatu = EnumRowStatusType.SoftDeleted;

                await _unitOfWork.UserPosts.UpdateAsync(silinecek_post);

                await _unitOfWork.CommitAsync();

                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = StatusCode,
                    Message     = ResultMessage,
                    Data        = new BaseInsertOrUpdateResponse { Response = 1 }
                };
            }
            catch(Exception e)
            {
                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                };
            }
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> LikeCommentById(LikeByIdRequest request)
        {
            try
            {
                //!< Commenti getirelim
                var begenilecek_comment = _unitOfWork.PostComments.GetById(request.Id).Result;

                if(true == request.Like)
                {
                    //!< Eger zaten bu commenti begenmissek ve bir hata sonucu tekrar begenme istegi geldiyse direkt return edelim
                    if (begenilecek_comment.UserIdWhoLike.Contains(request.UserId.ToString()))
                    {
                        return new ApiResult<BaseInsertOrUpdateResponse>
                        {
                            StatusCode  = ResponseCode.Basarili,
                            Message     = ResultMessage,
                        };
                    }

                    begenilecek_comment.UserIdWhoLike += request.UserId.ToString() + ",";
                    begenilecek_comment.LikeCount += 1;

                    //!< Comment sahibine bildirim gonderelim
                    var user            = _unitOfWork.CompanyAndPersons.GetById(begenilecek_comment.CompanyAndPerson.Id).Result;
                    var request_user    = _unitOfWork.CompanyAndPersons.GetById((int)request.UserId).Result;

                    _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                    {
                        Title = "Ekosistemde fark ediliyorsun",
                        Body = request_user.NameOrTitle + ", bir gönderiye yaptığın yorumu beğendi.",
                        DeviceFirebaseToken = user.Tokens.OrderByDescending(x => x.Id).FirstOrDefault().FirebaseToken,
                        PostId = begenilecek_comment.UserPosts.Id.ToString()
                    });
                }
                else
                {
                    string useridswholike = begenilecek_comment.UserIdWhoLike;
                    string newids = ",";
                    string[] useridsarray = useridswholike.Split(",");
                    if (useridsarray.Contains(request.UserId.ToString()))
                    {
                        foreach (string item in useridsarray)
                        {
                            if (item != request.UserId.ToString() && item != "")
                            {
                                newids += item + ",";
                            }
                        }

                        begenilecek_comment.UserIdWhoLike = newids;

                        begenilecek_comment.LikeCount -= 1;
                    }
                }
                
                begenilecek_comment.UpdatedDate = DateTime.Now;

                await _unitOfWork.PostComments.UpdateAsync(begenilecek_comment);

                await _unitOfWork.CommitAsync();

                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = ResultMessage,
                };
            }
            catch (Exception e)
            {
                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode = ResponseCode.Basarisiz,
                    Message = ResultMessage,
                    IsError = true,
                };
            }
        }

        public async Task<ApiResult<List<UserPostListResponse>>> GetUsersUserPostByUserId(GetUsersUserPostByUserIdRequest request)
        {
            try
            {
                //!< Kullanici adini cekelim
                int userId = _unitOfWork.CompanyAndPersons.GetByUserName(request.ShowingUserName).Result.Id;

                //!< Son x gündeki postlari interestlere gore cekelim
                var userPosts = _unitOfWork.UserPosts.GetLastN(request.StartIndex, request.RequestCount, userId).Result;

                //!< Veriyi elde ettik. Bunu simdi UserPostListResponse listesi haline getirelim.
                List<UserPostListResponse> dondurulecek_post_list = new List<UserPostListResponse>();

                int created_date = 0;

                string[] kullanici_idleri;
                bool     begenmis_mi;

                foreach (UserPosts post in userPosts)
                {
                    //!< Postun yazarini cekelim
                    CompanyAndPerson user   = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(post.CompanyAndPerson.Id);

                    var packageIcon = "";
                    var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);

                    if (userPackages.Count() > 0)
                    {
                        var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                        var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                        packageIcon = package.PackageIcon;
                    }

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }
                    
                    created_date = Convert.ToInt32( (DateTime.Now - post.CreatedDate).Value.TotalSeconds );
                    //!< Listeye postu ekleyelim
                    dondurulecek_post_list.Add(new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = created_date,
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarili,
                    Message     = ResultMessage,
                    Data        = dondurulecek_post_list,
                };
            }
            catch(Exception e)
            {
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                };
            }
        }

        public async Task<ApiResult<UserPostListResponse>> GetUserPostById(GetUserPostByIdRequest request)
        {
            try
            {
                var userPost = _unitOfWork.UserPosts.GetDetailedById(request.PostId).Result;

                //!< Postun yazarini cekelim
                CompanyAndPerson user = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(userPost.CompanyAndPerson.Id);

                var packageIcon = "";
                var userPackages   = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);
                if(userPackages.Count() > 0)
                {
                    var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                    var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                    packageIcon = package.PackageIcon;
                }

                int created_date = Convert.ToInt32((DateTime.Now - userPost.CreatedDate).Value.TotalSeconds);
                bool begenmis_mi = false;
                try
                {
                    string[] kullanici_idleri = userPost.UserIdWhoLike.Split(",");
                    begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                }
                catch (Exception e)
                {
                    begenmis_mi = false;
                }

                var response = new UserPostListResponse
                {
                    PostId              = userPost.Id.ToString(),
                    AuthorName          = user.NameOrTitle,
                    AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                    AuthorUserName      = user.UserName,
                    AuthorUserId        = user.Id,
                    AuthorProfileType   = user.ProfileType,
                    AuthorLogo          = user.ProfileImage,
                    AuthorPackageIcon   = packageIcon,
                    PostText            = userPost.PostText,
                    PostMedia           = userPost.PostMedia,
                    PostTopics          = userPost.PostTopics,
                    PostCreatedDate     = created_date,
                    PostLikeCount       = userPost.LikeCount,
                    PostCommentCount    = userPost.CommentCount,
                    IsLiked             = begenmis_mi,
                    IsUpgradedToBoard   = userPost.IsUpgradedToBoard,
                    SponsoredTitle      = userPost.SponsoredTitle,
                };

                return new ApiResult<UserPostListResponse>
                {
                    StatusCode  = ResponseCode.Basarili,
                    Message     = ResultMessage,
                    Data        = response,
                };
            }
            catch(Exception e)
            {
                return new ApiResult<UserPostListResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                };
            }
        }

        public async Task<ApiResult<List<UserPostListResponse>>> GetKitleFonlamaUserPosts(GetUserPostsRequest request)
        {
            try
            {
                var kitlefonlamaUser        = _unitOfWork.CompanyAndPersons.GetByUserName("eFonla").Result;
                var kitlefonlamaPostlar     = _unitOfWork.UserPosts.GetLastN( Int32.Parse(request.FilterTopicsStr), request.RequestCount, kitlefonlamaUser.Id).Result;
                var dondurulecek_post_list  = new List<UserPostListResponse>();

                int created_date = 0;

                string[] kullanici_idleri;
                bool begenmis_mi;

                foreach (UserPosts post in kitlefonlamaPostlar)
                {
                    //!< Postun yazarini cekelim
                    CompanyAndPerson user = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(post.CompanyAndPerson.Id);

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }

                    created_date = Convert.ToInt32((DateTime.Now - post.CreatedDate).Value.TotalSeconds);
                    //!< Listeye postu ekleyelim
                    dondurulecek_post_list.Add(new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = "",
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = created_date,
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode = ResponseCode.Basarili,
                    Message = ResultMessage,
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

        public async Task<ApiResult<List<UserPostListResponse>>> GetUserPosts(GetUserPostsRequest request)
        {

            //_notificationHelper.SendNotification(new Core.DTO.NotificationModel { Title = "dsgsdgds", Body = "dsdgds", DeviceFirebaseToken = "cEQ3YIxnd0ZJq0u_JK5Z4I:APA91bFylXL2FiWbB6fkbLEqon4vwZ1H67CjDd1-k9F08e_1Ge7yCP8ceKaNeSo0MDaIx04xdbd9Xwo9PMak9H55j0FTwNuC17WfbEAEJyQ7U_ff7IzDEK_Sg8tYUWcwVR1ztsnxS8ve" });

            try
            {
                Dictionary<UserPosts, int> post_dict = new Dictionary<UserPosts, int>();

                var             sponsoredPost       = _unitOfWork.UserPosts.GetSponsored().Result;

                //!< Son x gündeki postlari interestlere gore cekelim
                var             userPosts           = _unitOfWork.UserPosts.GetByLastHours(60000).Result;
                var             RequestTopicList    = request.FilterTopicsStr.DeserializeObject<List<string>>();
                List<UserPosts> userPosts2          = new List<UserPosts>();
                foreach(UserPosts post in userPosts)
                {
                    if(post.PostTopics.DeserializeObject<List<string>>().Intersect(RequestTopicList).Count() > 0 &&
                       RequestTopicList.Count                                                                > 0  )
                    {
                        userPosts2.Add(post);
                    }
                    else if(RequestTopicList.Count == 0)
                    {
                        userPosts2.Add(post);
                    }
                }

                int postScore = 0;

                //!< Siralamayi olusturalim
                foreach(UserPosts post in userPosts2)
                {
                    postScore = 0;

                    //!< Gecen dakika cinsinden zaman, ters oranli olarak post skoruna etki edecektir.
                    postScore += (int) (post.CreatedDate - DateTime.Now).Value.TotalMinutes;

                    //!< Like sayisi dogru orantili olarak post skoruna etki edecektir.
                    postScore += post.LikeCount;

                    //!< Dicte postu pushlayalim
                    post_dict.Add(post, postScore);
                }

                //!< Postlari skorlara gore yeniden siralayalim
                var siralanmis_ordered_dict = post_dict.OrderByDescending(i => i.Value);
                Dictionary<UserPosts, int> siralanmis_dict = new Dictionary<UserPosts, int>();
                for (int i = 0; i < siralanmis_ordered_dict.Count(); i++)
                {
                    siralanmis_dict.Add(siralanmis_ordered_dict.ElementAt(i).Key,
                                        siralanmis_ordered_dict.ElementAt(i).Value);
                }

                //!< Kac adet dondurulecekse bunlari sinirlandiralim
                //!< Eger post sayisi istenenden fazla ise
                Dictionary<UserPosts, int> sinirlandirilmis_dict = new Dictionary<UserPosts, int>();
                if (request.RequestCount <= siralanmis_dict.Count())
                {
                    for(int i = 0; i<request.RequestCount; i++)
                    {
                        sinirlandirilmis_dict.Add(  siralanmis_dict.ElementAt(i).Key    ,
                                                    siralanmis_dict.ElementAt(i).Value  );
                    }
                }
                //!< Degilse tum siralanmis dicti koyabiliriz.
                else
                {
                    sinirlandirilmis_dict = siralanmis_dict;
                }

                //!< Veriyi elde ettik. Bunu simdi UserPostListResponse listesi haline getirelim.
                List<UserPostListResponse> dondurulecek_post_list = new List<UserPostListResponse>();

                //!< Sponsorlu post varsa onu ekleyelim
                if(sponsoredPost.Count() > 0)
                {
                    CompanyAndPerson user_s         = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(sponsoredPost.ElementAt(0).CompanyAndPerson.Id);
                    int              created_date_s = Convert.ToInt32((DateTime.Now - sponsoredPost.ElementAt(0).CreatedDate).Value.TotalSeconds);
                    bool             begenmis_mi_s  = false;

                    var packageIcon = "";
                    var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user_s.Id).ConfigureAwait(false);
                    if (userPackages.Count() > 0)
                    {
                        var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                        var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                        packageIcon = package.PackageIcon;
                    }

                    try
                    {
                        string[] kullanici_idleri_s = sponsoredPost.ElementAt(0).UserIdWhoLike.Split(",");
                        begenmis_mi_s               = kullanici_idleri_s.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi_s = false;
                    }

                    dondurulecek_post_list.Add(new UserPostListResponse
                    {
                        PostId              = sponsoredPost.ElementAt(0).Id.ToString(),
                        AuthorName          = user_s.NameOrTitle,
                        AuthorOccupation    = user_s.ProfileType == 1 ? user_s.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user_s.CompanyType?.Name,
                        AuthorUserName      = user_s.UserName,
                        AuthorUserId        = user_s.Id,
                        AuthorProfileType   = user_s.ProfileType,
                        AuthorLogo          = user_s.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = sponsoredPost.ElementAt(0).PostText,
                        PostMedia           = sponsoredPost.ElementAt(0).PostMedia,
                        PostTopics          = sponsoredPost.ElementAt(0).PostTopics,
                        PostCreatedDate     = created_date_s,
                        PostLikeCount       = sponsoredPost.ElementAt(0).LikeCount,
                        PostCommentCount    = sponsoredPost.ElementAt(0).CommentCount,
                        IsLiked             = begenmis_mi_s,
                        IsUpgradedToBoard   = sponsoredPost.ElementAt(0).IsUpgradedToBoard,
                        SponsoredTitle      = sponsoredPost.ElementAt(0).SponsoredTitle,
                    });
                }

                int created_date = 0;

                string[] kullanici_idleri;
                bool     begenmis_mi;

                foreach (UserPosts post in sinirlandirilmis_dict.Keys.ToArray())
                {
                    //!< Postun yazarini cekelim
                    CompanyAndPerson user   = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(post.CompanyAndPerson.Id);

                    var packageIcon = "";
                    var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);

                    if (userPackages.Count() > 0)
                    {
                        var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                        var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                        packageIcon = package.PackageIcon;
                    }

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }
                    
                    created_date = Convert.ToInt32( (DateTime.Now - post.CreatedDate).Value.TotalSeconds );
                    //!< Listeye postu ekleyelim
                    dondurulecek_post_list.Add(new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = created_date,
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarili,
                    Message     = ResultMessage,
                    Data        = dondurulecek_post_list,
                };
            }
            catch(Exception e)
            {
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                };
            }
        }

        public async Task<ApiResult<List<UserPostListResponse>>> GetMoreUserPosts(GetMoreUserPostsRequest request)
        {
            try
            {
                Dictionary<UserPosts, int> post_dict = new Dictionary<UserPosts, int>();

                //!< Var olan post ID'leri alalim
                var existingPostIds = request.PostIds.DeserializeObject<List<int>>();

                //!< Son x gündeki postlari interestlere gore cekelim
                var userPosts = _unitOfWork.UserPosts.GetByLastHours(600).Result;
                var RequestTopicList = request.FilterTopicsStr.DeserializeObject<List<string>>();
                List<UserPosts> userPosts2 = new List<UserPosts>();
                foreach (UserPosts post in userPosts)
                {
                    if(post.PostTopics.DeserializeObject<List<string>>().Intersect(RequestTopicList).Count() > 0 &&
                       RequestTopicList.Count                                                                > 0  )
                    {
                        userPosts2.Add(post);
                    }
                    else if(RequestTopicList.Count == 0)
                    {
                        userPosts2.Add(post);
                    }
                }

                //!< Var olan postlari siradan cikaralim
                List<UserPosts> userPosts3 = new List<UserPosts>();
                foreach (UserPosts post in userPosts2)
                {
                    if (existingPostIds.Contains(post.Id) == false)
                    {
                        userPosts3.Add(post);
                    }
                }

                int postScore = 0;

                //!< Siralamayi olusturalim
                foreach (UserPosts post in userPosts3)
                {
                    postScore = 0;

                    //!< Gecen dakika cinsinden zaman, ters oranli olarak post skoruna etki edecektir.
                    postScore += (int)(post.CreatedDate - DateTime.Now).Value.TotalMinutes;

                    //!< Like sayisi dogru orantili olarak post skoruna etki edecektir.
                    postScore += post.LikeCount;

                    //!< Dicte postu pushlayalim
                    post_dict.Add(post, postScore);
                }

                //!< Postlari skorlara gore yeniden siralayalim
                var siralanmis_ordered_dict = post_dict.OrderByDescending(i => i.Value);
                Dictionary<UserPosts, int> siralanmis_dict = new Dictionary<UserPosts, int>();
                for (int i = 0; i < siralanmis_ordered_dict.Count(); i++)
                {
                    siralanmis_dict.Add(siralanmis_ordered_dict.ElementAt(i).Key,
                                        siralanmis_ordered_dict.ElementAt(i).Value);
                }

                //!< Kac adet dondurulecekse bunlari sinirlandiralim
                //!< Eger post sayisi istenenden fazla ise
                Dictionary<UserPosts, int> sinirlandirilmis_dict = new Dictionary<UserPosts, int>();
                if (request.RequestCount <= siralanmis_dict.Count())
                {
                    for (int i = 0; i < request.RequestCount; i++)
                    {
                        sinirlandirilmis_dict.Add(siralanmis_dict.ElementAt(i).Key,
                                                    siralanmis_dict.ElementAt(i).Value);
                    }
                }
                //!< Degilse tum siralanmis dicti koyabiliriz.
                else
                {
                    sinirlandirilmis_dict = siralanmis_dict;
                }

                //!< Veriyi elde ettik. Bunu simdi UserPostListResponse listesi haline getirelim.
                List<UserPostListResponse> dondurulecek_post_list = new List<UserPostListResponse>();
                int created_date = 0;

                string[] kullanici_idleri;
                bool begenmis_mi;

                foreach (UserPosts post in sinirlandirilmis_dict.Keys.ToArray())
                {
                    //!< Postun yazarini cekelim
                    CompanyAndPerson user   = await _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(post.CompanyAndPerson.Id);

                    var packageIcon = "";
                    var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);
                    if (userPackages.Count() > 0)
                    {
                        var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                        var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                        packageIcon = package.PackageIcon;
                    }

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }

                    created_date = Convert.ToInt32((DateTime.Now - post.CreatedDate).Value.TotalSeconds);
                    //!< Listeye postu ekleyelim
                    dondurulecek_post_list.Add(new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = created_date,
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarili,
                    Message     = ResultMessage,
                    Data        = dondurulecek_post_list,
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

        public async Task<ApiResult<List<UserPostListResponse>>> GetUserPostsByUserId(BaseRequest request)
        {
            try
            {
                //!< Kullanicinin postlarini cekelim
                var userPosts = _unitOfWork.UserPosts.GetByUserId(request.UserId.Value).Result;

                //!< Postun yazarini cekelim
                var user      = _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById((request.UserId.Value)).Result;

                var packageIcon = "";
                var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);
                if (userPackages.Count() > 0)
                {
                    var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                    var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                    packageIcon = package.PackageIcon;
                }

                List<UserPostListResponse> dondurulecek_post_list = new List<UserPostListResponse>();

                string[] kullanici_idleri;
                bool     begenmis_mi;

                //!< Sayiyla sinirlandiralim
                int dondurulecek_post_sayisi = Math.Min(10, userPosts.Count());
                
                for(int i = dondurulecek_post_sayisi-1; i >= 0; i--)
                {
                    UserPosts post = userPosts.ElementAt(i);

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch(Exception e)
                    {
                        begenmis_mi = false;
                    }

                    //!< Listeye postu ekleyelim
                    dondurulecek_post_list.Add(new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = Convert.ToInt32((DateTime.Now - (DateTime)post.CreatedDate).TotalSeconds),
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    });
                }

                //!< Yaniti dondurelim
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarili,
                    Message     = ResultMessage,
                    Data        = dondurulecek_post_list,
                };
            }
            catch(Exception e)
            {
                return new ApiResult<List<UserPostListResponse>>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                };
            }
        }

        public async Task<ApiResult<UserPostListResponse>> GetSponsoredUserPost(BaseRequest request)
        {
            try
            {
                var post_arr = _unitOfWork.UserPosts.GetSponsored().Result;

                if (post_arr.Count() > 0)
                {
                    var         post            = post_arr.ElementAt(0);
                    var         user            = _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(post.CompanyAndPerson.Id).Result;
                    var         begenmis_mi     = false;
                    string[]    kullanici_idleri;

                    var packageIcon = "";
                    var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);
                    if (userPackages.Count() > 0)
                    {
                        var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                        var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                        packageIcon = package.PackageIcon;
                    }

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }

                    UserPostListResponse response = new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = Convert.ToInt32((DateTime.Now - (DateTime)post.CreatedDate).TotalSeconds),
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    };


                    return new ApiResult<UserPostListResponse>
                    {
                        StatusCode  = ResponseCode.Basarili,
                        Message     = ResultMessage,
                        Data        = response,
                    };
                }
                else
                {
                    return new ApiResult<UserPostListResponse>
                    {
                        StatusCode  = ResponseCode.Basarisiz,
                        Message     = ResultMessage,
                        IsError     = true,
                    };
                }
            }
            catch(Exception e)
            {
                return new ApiResult<UserPostListResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                };
            }
        }

        public async Task<ApiResult<UserPostListResponse>> GetLastPostByUserId(GetLastPostByUserIdRequest request)
        {
            try
            {
                var post_arr = _unitOfWork.UserPosts.GetLastByUserId(request.PostOwner).Result;

                if(post_arr.Count() > 0)
                {
                    var         post            = post_arr.ElementAt(0);
                    var         user            = _unitOfWork.CompanyAndPersons.GetCompanyAndPersonPostDetailById(post.CompanyAndPerson.Id).Result;
                    var         begenmis_mi     = false;
                    string[]    kullanici_idleri;

                    var packageIcon = "";
                    var userPackages = await _unitOfWork.PackagePayment.GetUserPayPackageList(user.Id).ConfigureAwait(false);
                    if (userPackages.Count() > 0)
                    {
                        var userTopPackage = userPackages.OrderByDescending(x => x.PackageId).First();
                        var package = await _unitOfWork.Package.GetById(userTopPackage.PackageId).ConfigureAwait(false);
                        packageIcon = package.PackageIcon;
                    }

                    //!< Postu kullanici begenmis mi?
                    try
                    {
                        kullanici_idleri = post.UserIdWhoLike.Split(",");
                        begenmis_mi = kullanici_idleri.Contains(request.UserId.ToString());
                    }
                    catch (Exception e)
                    {
                        begenmis_mi = false;
                    }

                    UserPostListResponse response = new UserPostListResponse
                    {
                        PostId              = post.Id.ToString(),
                        AuthorName          = user.NameOrTitle,
                        AuthorOccupation    = user.ProfileType == 1 ? user.CompanyAndPersonOccupation.FirstOrDefault()?.Occupation.Name : user.CompanyType?.Name,
                        AuthorUserName      = user.UserName,
                        AuthorUserId        = user.Id,
                        AuthorProfileType   = user.ProfileType,
                        AuthorLogo          = user.ProfileImage,
                        AuthorPackageIcon   = packageIcon,
                        PostText            = post.PostText,
                        PostMedia           = post.PostMedia,
                        PostTopics          = post.PostTopics,
                        PostCreatedDate     = Convert.ToInt32((DateTime.Now - (DateTime)post.CreatedDate).TotalSeconds),
                        PostLikeCount       = post.LikeCount,
                        PostCommentCount    = post.CommentCount,
                        IsLiked             = begenmis_mi,
                        IsUpgradedToBoard   = post.IsUpgradedToBoard,
                        SponsoredTitle      = post.SponsoredTitle,
                    };


                    return new ApiResult<UserPostListResponse>
                    {
                        StatusCode  = ResponseCode.Basarili,
                        Message     = ResultMessage,
                        Data        = response,
                    };
                }
                else
                {
                    return new ApiResult<UserPostListResponse>
                    {
                        StatusCode  = ResponseCode.Basarisiz,
                        Message     = ResultMessage,
                        IsError     = true,
                    };
                }
            }
            catch(Exception ex)
            {
                return new ApiResult<UserPostListResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
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
                obj.RowStatu = (int)item.RowStatu.Value;
                obj.PostText = item.PostText;
                obj.PostMedia = item.PostMedia;
                obj.NameOrTitle = item.CompanyAndPerson.NameOrTitle;
                obj.CountData = count;
                responseList.Add(obj);

            }
            return responseList;
        }

        public async Task<PostDatatableModel> GetUserPostById(GetUsersRequestDTO request)
        {
            var userPost = _unitOfWork.UserPosts.GetDetailedById(request.Id).Result;
            
            PostDatatableModel obj = new PostDatatableModel();
            obj.Id = userPost.Id;
            obj.PostText = userPost.PostText;
            obj.PostMedia = userPost.PostMedia;
            obj.PostTopics = userPost.PostTopics;
            obj.IsSponsoredPost = userPost.IsSponsoredPost;
            obj.NameOrTitle = userPost.CompanyAndPerson.NameOrTitle;

            return obj;
        }

        public async Task<ApiResult<BaseInsertOrUpdateResponse>> SetMailPermission(SetBoolValueRequest request)
        {
            try
            {
                var user = _unitOfWork.CompanyAndPersons.GetById((int)request.UserId).Result;

                user.MailPermission = request.Value;

                await _unitOfWork.CompanyAndPersons.UpdateAsync(user);
                await _unitOfWork.CommitAsync();

                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode = StatusCode,
                    Message = ResultMessage,
                    Data = new BaseInsertOrUpdateResponse { Response = 1 }
                };
            }
            catch(Exception e)
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
                    UserPosts           = post,
                    Text                = request.Text,
                    CompanyAndPerson    = user,
                    LikeCount           = 0,
                    UserIdWhoLike       = ",",
                };

                post.CommentCount += 1;

                await _unitOfWork.PostComments
                    .AddAsync(new_comment);
                await _unitOfWork.UserPosts
                    .UpdateAsync(post);

                await _unitOfWork.CommitAsync();

                //!< Post sahibine bildirim gonderelim
                var post_sahibi_user = _unitOfWork.CompanyAndPersons.GetById(post.CompanyAndPerson.Id).Result;

                _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                {
                    Title = "Ekosistemde fark ediliyorsun",
                    Body = user.NameOrTitle + ", gönderine yorum yaptı.",
                    DeviceFirebaseToken = post_sahibi_user.Tokens.OrderByDescending(x => x.Id).FirstOrDefault().FirebaseToken,
                    PostId = post.Id.ToString()
                });

                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = StatusCode,
                    Message     = ResultMessage,
                    Data        = new BaseInsertOrUpdateResponse { Response = new_comment.Id }
                };
            }
            catch(Exception e)
            {
                return new ApiResult<BaseInsertOrUpdateResponse>
                {
                    StatusCode  = ResponseCode.Basarisiz,
                    Message     = ResultMessage,
                    IsError     = true,
                    Data        = new BaseInsertOrUpdateResponse { Response = 0 }
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
                    await SendNotification(new SendNotificationModel { userId = followUser.CompanyAndPerson.Id, toUserId = request.UserId.Value,ProfileType = userData.ProfileType, NotificationType = EnumNotificationType.Information });
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
                if(!(item == "" || item == null))
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
            var response = await _unitOfWork.CompanyAndPersons.Find(x => x.NameOrTitle.Contains(request.search == null ? "" :request.search)).ConfigureAwait(false);
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
        public List<UserAndPostListResponse> GetUsersByPostCount(GetUsersRequestDTO request)
        {
            var query = _unitOfWork.GetRepository<UserPosts>().Where(predicate: x => x.RowStatu == EnumRowStatusType.Active)
              .Include(x => x.CompanyAndPerson).AsEnumerable()
              .GroupBy(x => x.CompanyAndPerson).OrderByDescending(x => x.Count())
              .Skip(request.skip).Take(request.take > 0 ? request.take : 20); ;


            List<UserAndPostListResponse> response = new List<UserAndPostListResponse>();
            foreach (var user in query)
            {
                UserAndPostListResponse data = new UserAndPostListResponse
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
                message = toUserInfo.NameOrTitle + " seni bağlantılarına ekledi.";
                if (0 < user.Tokens.Count)
                {
                    _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                    {
                        Title = "Girişimci çevren genişliyor!",
                        Body = message,
                        DeviceFirebaseToken = user.Tokens.OrderByDescending(x => x.Id).FirstOrDefault().FirebaseToken
                    });
                }
            }
            else if (sendNotificationModel.NotificationType == EnumNotificationType.Show)
            {
                message = toUserInfo.NameOrTitle + " profilini görüntüledi.";
                if( 0 < user.Tokens.Count )
                {
                    _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                    {
                        Title = "BULLBEEZ",
                        Body = message,
                        DeviceFirebaseToken = user.Tokens.OrderByDescending(x => x.Id).FirstOrDefault().FirebaseToken
                    });
                }
            }
            else if (sendNotificationModel.NotificationType == EnumNotificationType.Worker)
            {
                message = toUserInfo.NameOrTitle + " seni çalışanı olarak ekledi.";
                if (0 < user.Tokens.Count)
                {
                    _notificationHelper.SendNotification(new Core.DTO.NotificationModel
                    {
                        Title = "BULLBEEZ",
                        Body = message,
                        DeviceFirebaseToken = user.Tokens.OrderByDescending(x => x.Id).FirstOrDefault().FirebaseToken
                    });
                }
            }
            
            await _unitOfWork.Notifications.AddAsync(new Notification { CompanyAndPerson = user, ToUserId = toUserInfo.Id, Message = message, IsShow = 0, ProfileImage = toUserInfo.ProfileImage,ProfileType = toUserInfo.ProfileType });

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
