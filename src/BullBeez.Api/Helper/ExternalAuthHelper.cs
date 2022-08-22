using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;

using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse.Auth;
using BullBeez.Core.Services;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Api.Helper
{

    public interface IExternalAuthHelper
    {
        Task<ApiResult<NewUserResponse>> VerifyGoogleToken(ExternalAuthGoogle externalAuth);
        Task<ApiResult<NewUserResponse>> VerifyLinkedinToken(ExternalAuthLinkedin externalAuth);
        Task<ApiResult<NewUserResponse>> VerifyAppleToken(ExternalAuthApple externalAuth);
    }
    public class ExternalAuthHelper : IExternalAuthHelper
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _goolgeSettings;
        private readonly ICompanyAndPersonService _companyAndPersonService;
        public ExternalAuthHelper(IConfiguration configuration, ICompanyAndPersonService companyAndPersonService)
        {
            _configuration = configuration;
            _goolgeSettings = _configuration.GetSection("GoogleAuthSettings");
            _companyAndPersonService = companyAndPersonService;
        }
        public async Task<ApiResult<NewUserResponse>> VerifyGoogleToken(ExternalAuthGoogle externalAuth)
        {
            ApiResult<NewUserResponse> response = new ApiResult<NewUserResponse>();
            if (externalAuth == null || string.IsNullOrWhiteSpace(externalAuth.id_token))
            {
                response.Message = "Token bilgisine ulaşılamadı.";
                response.IsError = true;
                response.StatusCode = ResponseCode.Basarisiz;
                response.Data = new NewUserResponse { };
                return response;
            }
            try
            {
                externalAuth.DeviceId = externalAuth.DeviceId ?? "1";
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _goolgeSettings.GetSection("ClientId").Value },
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.id_token, settings);

                if (payload == null)
                {
                    response.Message = "Geçersiz kullanıcı bilgileri.";
                    response.IsError = true;
                    response.StatusCode = ResponseCode.Basarisiz;
                    response.Data = new NewUserResponse { };
                    return response;
                }

                var user = await _companyAndPersonService.SearchUserByFilter(new SearchUserByFilterRequest { EmailAddress = payload.Email });
                //Eğer kullanıcı sistemde yoksa yeni kullanıcı oluştur.
                if (user == null || user.Data == null || !user.Data.Any())
                {
                    var password = Guid.NewGuid().ToString();
                    var newUser = await _companyAndPersonService.NewUser(new NewUserContract
                    {
                        Name = payload.Name,
                        EmailAddress = payload.Email,
                        UserName = payload.Email,
                        Password = password,
                        CompanyLevel = 0,
                        DeviceId = externalAuth.DeviceId

                    });

                    if (newUser.StatusCode == ResponseCode.Basarisiz)
                    {
                        return newUser;
                    }

                    user = await _companyAndPersonService.SearchUserByFilter(new SearchUserByFilterRequest { EmailAddress = payload.Email });
                }

                if (user.Data == null || !user.Data.Any())
                {
                    response.Message = "Kullanıcı oluşturulamadı.";
                    response.IsError = true;
                    response.StatusCode = ResponseCode.Basarisiz;
                    response.Data = new NewUserResponse { };
                    return response;
                }

                //Kullanıcyı oluştur ve login olmasını sağla.
                var loginResult = await _companyAndPersonService.ExternLoginUser(new NewLoginRequest
                {
                    EmailAddress = payload.Email,
                    DeviceId = externalAuth.DeviceId
                });
                return loginResult;



            }
            catch (System.Exception ex)
            {
                response.Message = "Hata meydana geldi.";
                response.IsError = true;
                response.StatusCode = ResponseCode.Basarisiz;
                response.Data = new NewUserResponse { };
                return response;
            }

        }
        public async Task<ApiResult<NewUserResponse>> VerifyLinkedinToken(ExternalAuthLinkedin externalAuth)
        {

            ApiResult<NewUserResponse> response = new ApiResult<NewUserResponse>();
            if (externalAuth == null || string.IsNullOrWhiteSpace(externalAuth.access_token))
            {
                response.Message = "Token bilgisine ulaşılamadı.";
                response.IsError = true;
                response.StatusCode = ResponseCode.Basarisiz;
                response.Data = new NewUserResponse { };
                return response;
            }
            try
            {
                externalAuth.DeviceId = externalAuth.DeviceId ?? "1";
                using (HttpClient httpClient = new HttpClient())
                {
                    string mailUrl = "https://api.linkedin.com/v2/emailAddress?q=members&projection=(elements*(handle~))";
                    string profileUrl = "https://api.linkedin.com/v2/me";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", externalAuth.access_token);
                    var mailResult = await httpClient.GetStringAsync(mailUrl);
                    var mailData = JsonConvert.DeserializeObject<LinkedinProfileEmailResponse>(mailResult, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore,
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    }) ?? new LinkedinProfileEmailResponse();
                    string mail = mailData.elements?.FirstOrDefault()?.Handle?.emailAddress;

                    if (string.IsNullOrWhiteSpace(mail))
                    {
                        response.Message = "Kullanıcı mail bilgisine ulaşılamadı.";
                        response.IsError = true;
                        response.StatusCode = ResponseCode.Basarisiz;
                        response.Data = new NewUserResponse { };
                        return response;
                    }

                    var user = await _companyAndPersonService.SearchUserByFilter(new Core.RequestDTO.SearchUserByFilterRequest { EmailAddress = mail });

                    if (user == null || user.Data == null || !user.Data.Any())
                    {
                        var profileResult = await httpClient.GetStringAsync(profileUrl);
                        var profileData = JsonConvert.DeserializeObject<LinkedinProfileResponseDTO>(profileResult, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }) ?? new LinkedinProfileResponseDTO();
                        string name = profileData.localizedFirstName;
                        string surName = profileData.localizedLastName;

                        name = name ?? surName;
                        surName = surName ?? name;

                        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(surName))
                        {
                            response.Message = "Kullanıcı bilgilerine ulaşılamadı.";
                            response.IsError = true;
                            response.StatusCode = ResponseCode.Basarisiz;
                            response.Data = new NewUserResponse { };
                            return response;
                        }


                        var password = Guid.NewGuid().ToString();
                        var newUser = await _companyAndPersonService.NewUser(new NewUserContract
                        {
                            Name = name + " " + surName,
                            EmailAddress = mail,
                            UserName = mail,
                            Password = password,
                            CompanyLevel = 0,
                            DeviceId = externalAuth.DeviceId

                        });

                        if (newUser.StatusCode == ResponseCode.Basarisiz)
                        {
                            return newUser;
                        }

                        user = await _companyAndPersonService.SearchUserByFilter(new SearchUserByFilterRequest { EmailAddress = mail });
                    }


                    if (user == null || user.Data == null || !user.Data.Any())
                    {
                        response.Message = "Kullanıcı oluşturulamadı.";
                        response.IsError = true;
                        response.StatusCode = ResponseCode.Basarisiz;
                        response.Data = new NewUserResponse { };
                        return response;
                    }
                    //Kullanıcyı oluştur ve login olmasını sağla.
                    var loginResult = await _companyAndPersonService.ExternLoginUser(new NewLoginRequest
                    {
                        EmailAddress = mail,
                        DeviceId = externalAuth.DeviceId
                    });

                    return loginResult;

                }
            }
            catch (System.Exception ex)
            {
                response.Message = "Hata meydana geldi.";
                response.IsError = true;
                response.StatusCode = ResponseCode.Basarisiz;
                response.Data = new NewUserResponse { };
                return response;
            }
        }
        public async Task<ApiResult<NewUserResponse>> VerifyAppleToken(ExternalAuthApple externalAuth)
        {
            ApiResult<NewUserResponse> response = new ApiResult<NewUserResponse>();

            if (externalAuth == null || externalAuth.user == null || string.IsNullOrWhiteSpace(externalAuth.user.email)
              || externalAuth.user.name == null)
            {
                response.Message = "Kullanıcı bilgilerine ulaşılamadı.";
                response.IsError = true;
                response.StatusCode = ResponseCode.Basarisiz;
                response.Data = new NewUserResponse { };
                return response;
            }

            try
            {
                externalAuth.DeviceId = externalAuth.DeviceId ?? "1";
                var user = await _companyAndPersonService.SearchUserByFilter(new SearchUserByFilterRequest { EmailAddress = externalAuth.user.email });
                //Eğer kullanıcı sistemde yoksa yeni kullanıcı oluştur.
                if (user == null || user.Data == null || !user.Data.Any())
                {
                    var password = Guid.NewGuid().ToString();
                    var newUser = await _companyAndPersonService.NewUser(new NewUserContract
                    {
                        Name = externalAuth.user.name.firstName + "" + externalAuth.user.name.lastName,
                        EmailAddress = externalAuth.user.email,
                        UserName = externalAuth.user.email,
                        Password = password,
                        CompanyLevel = 0,
                        DeviceId = externalAuth.DeviceId

                    });

                    if (newUser.StatusCode == ResponseCode.Basarisiz)
                    {
                        return newUser;
                    }

                    user = await _companyAndPersonService.SearchUserByFilter(new SearchUserByFilterRequest { EmailAddress = externalAuth.user.email });
                }

                if (user.Data == null || !user.Data.Any())
                {
                    response.Message = "Kullanıcı oluşturulamadı.";
                    response.IsError = true;
                    response.StatusCode = ResponseCode.Basarisiz;
                    response.Data = new NewUserResponse { };
                    return response;
                }

                //Kullanıcyı oluştur ve login olmasını sağla.
                var loginResult = await _companyAndPersonService.ExternLoginUser(new NewLoginRequest
                {
                    EmailAddress = externalAuth.user.email,
                    DeviceId = externalAuth.DeviceId
                });
                return loginResult;
            }
            catch (Exception)
            {
                response.Message = "Hata meydana geldi.";
                response.IsError = true;
                response.StatusCode = ResponseCode.Basarisiz;
                response.Data = new NewUserResponse { };
                return response;
            }

        }

    }
}
