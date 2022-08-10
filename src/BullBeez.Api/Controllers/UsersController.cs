using AutoMapper;

using BullBeez.Api.Classed;
using BullBeez.Api.Helper;
using BullBeez.Core.DTO.CompanyAndPersonDTO;
using BullBeez.Core.DTO.MailDTO;
using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;

using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse.Auth;
using BullBeez.Core.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BullBeez.Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly ICompanyAndPersonService _companyAndPersonService;
    private readonly IExternalAuthHelper _externalAuthHelper;
    public UsersController(ICompanyAndPersonService companyAndPersonService, IExternalAuthHelper externalAuthHelper)
    {
      this._companyAndPersonService = companyAndPersonService;
      _externalAuthHelper = externalAuthHelper;
    }



    [HttpPost("NewUser")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> NewUser([FromHeader] NewUserContract request)
    {
      var response = await _companyAndPersonService.NewUser(request);

      return response.SerializeObject();
    }

    [HttpPost("TokenControl")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> TokenControl([FromHeader] BaseRequest request)
    {
      var response = await _companyAndPersonService.UserControl(request);

      return response.SerializeObject();
    }

    [HttpPost("NewLogin")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> NewLogin([FromHeader] NewLoginRequest request)
    {
      var response = await _companyAndPersonService.NewLogin(request);

      return response.SerializeObject();
    }

    [HttpPost("LoginUser")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> LoginUser([FromHeader] NewLoginRequest request)
    {
      var response = await _companyAndPersonService.LoginUser(request);

      return response.SerializeObject();
    }

    /// <summary>
    /// Eğer bu methodlar public olacaksa direkt Google developer arayuzunden ayarlar tanımlanırken redirect_url kısmına bu action verilebilir.
    /// Eğer yukarıdaki işlem yapılacaksa Header'dan değil Body'den okunmalı [FromBody]
    /// Eğer public olmayacaksa, googledan dönen response frond-end tarafındaki bir url'e yönlendirilmeli, oradan data header'a eklenip buraya gönderilmeli
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("GoogleLogin")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> GoogleLogin([FromHeader] ExternalAuthGoogle request)
    {
      var result = await _externalAuthHelper.VerifyGoogleToken(request);
      return result.SerializeObject();
    }
    /// <summary>
    /// Eğer bu methodlar public olacaksa direkt Linkedin developer arayuzunden ayarlar tanımlanırken redirect_url kısmına bu action verilebilir.
    /// Eğer yukarıdaki işlem yapılacaksa Header'dan değil Body'den okunmalı [FromBody]
    /// Eğer public olmayacaksa, googledan dönen response frond-end tarafındaki bir url'e yönlendirilmeli, oradan data header'a eklenip buraya gönderilmeli
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("LinkedinLogin")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> LinkedinLogin([FromHeader] ExternalAuthLinkedin request)
    {
      var result = await _externalAuthHelper.VerifyLinkedinToken(request);
      return result.SerializeObject();
    }
    /// <summary>
    /// Eğer bu methodlar public olacaksa direkt Apple developer arayuzunden ayarlar tanımlanırken redirect_url kısmına bu action verilebilir.
    /// Eğer yukarıdaki işlem yapılacaksa Header'dan değil Body'den okunmalı [FromBody]
    /// Eğer public olmayacaksa, googledan dönen response frond-end tarafındaki bir url'e yönlendirilmeli, oradan data header'a eklenip buraya gönderilmeli
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("AppleLogin")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> AppleLogin([FromHeader] ExternalAuthApple request)
    {
      var result = await _externalAuthHelper.VerifyAppleToken(request);
      return result.SerializeObject();
    }

    [HttpPost("SearchUserByFilter")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> SearchUserByFilter([FromHeader] SearchUserByFilterRequest request)
    {
      var response = await _companyAndPersonService.SearchUserByFilter(request);

      return response.SerializeObject();
    }

    [HttpPost("IsMailExists")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> IsMailExists([FromHeader] IsMailExistsRequest request)
    {
      var response = await _companyAndPersonService.IsMailExists(request);

      return response.SerializeObject();
    }

    [HttpPost("UploadImage")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UploadImage([FromHeader] UploadImageRequest request)
    {
      var response = await _companyAndPersonService.UploadImage(request);

      return response.SerializeObject();
    }

    [HttpPost("Logout")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> Logout([FromHeader] BaseRequest request)
    {
      var response = await _companyAndPersonService.Logout(request);

      return response.SerializeObject();
    }



    [HttpPost("UserInfoByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UserInfoByUserId([FromHeader] BaseRequest request)
    {
      var response = await _companyAndPersonService.UserInfoByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("ResetPassword")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> ResetPassword([FromHeader] BaseRequest request)
    {
      var response = await _companyAndPersonService.ResetPassword(request);

      return response.SerializeObject();
    }



    [HttpPost("UserShowByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UserShowByUserId([FromHeader] UserShowByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UserShowByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UploadCv")]
    public async Task<string> UploadCv([FromForm] UploadCvRequest request)
    {
      var response = await _companyAndPersonService.UploadCv(request);

      return response.SerializeObject();

    }

    [HttpPost("DeletePdf")]
    public async Task<string> DeletePdf([FromForm] UploadCvRequest request)
    {
      var response = await _companyAndPersonService.DeletePDF(request);

      return response.SerializeObject();

    }

    #region Post işlemleri
    [HttpPost("NewPost")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> NewPost([FromHeader] NewPostRequest request)
    {
      var response = await _companyAndPersonService.NewPost(request);

      return response.SerializeObject();
    }

    [HttpPost("GetUserPosts")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> GetUserPosts([FromHeader] GetUserPostsRequest request)
    {
      var response = await _companyAndPersonService.GetUserPosts(request);

      return response.SerializeObject();
    }

    [HttpPost("NewComment")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> NewComment([FromHeader] NewCommentRequest request)
    {
      var response = await _companyAndPersonService.NewComment(request);

      return response.SerializeObject();
    }
    #endregion

    #region PopUp işlemi
    [HttpPost("PopUpIsRead")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> PopUpIsRead([FromHeader] PopUpIsReadRequest request)
    {
      var response = await _companyAndPersonService.PopUpIsRead(request);

      return response.SerializeObject();
    }


    [HttpPost("PopUpReading")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> PopUpReading([FromHeader] PopUpReadingRequest request)
    {
      var response = await _companyAndPersonService.PopUpReading(request);

      return response.SerializeObject();
    }
    #endregion

    #region Takip ve çalışan işlemleri

    [HttpPost("FollowUserByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> FollowUserByUserId([FromHeader] FollowUserByUserIdRequest request)
    {
      var response = await _companyAndPersonService.FollowUserByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UnfollowUserByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UnfollowUserByUserId([FromHeader] FollowUserByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UnfollowUserByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("GetFollowUserByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> GetFollowUserByUserId([FromHeader] FollowUserByUserIdRequest request)
    {
      if (request.FollowType == EnumFollowType.Worker)
      {
        var response = await _companyAndPersonService.GetWorkerUserByUserId(request);

        return response.SerializeObject();
      }
      else
      {
        var response = await _companyAndPersonService.GetFollowUserByUserId(request);

        return response.SerializeObject();
      }

    }

    [HttpPost("GetWaitingWorkerList")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> GetWaitingWorkerList([FromHeader] FollowUserByUserIdRequest request)
    {
      var response = await _companyAndPersonService.GetWaitingWorkerList(request);

      return response.SerializeObject();
    }

    [HttpPost("WorkerApprovedAndCancel")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> WorkerApprovedAndCancel([FromHeader] FollowUserByUserIdRequest request)
    {
      var response = await _companyAndPersonService.WorkerApprovedAndCancel(request);

      return response.SerializeObject();
    }


    #endregion
    [HttpPost("IsExistsUserName")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> IsExistsUserName([FromHeader] IsExistsUserNameRequest request)
    {
      var response = await _companyAndPersonService.IsExistsUserName(request);

      return response.SerializeObject();
    }

    [HttpPost("GetInterestsListByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> GetInterestsListByUserId([FromHeader] BaseRequest request)
    {
      var response = await _companyAndPersonService.GetInterestsListByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("GetListNotificationByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> GetListNotificationByUserId([FromHeader] BaseRequest request)
    {
      var response = await _companyAndPersonService.GetListNotificationByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateBullbeezSentenceByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateBullbeezSentenceByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateBullbeezSentenceByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateGSMByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateGSMByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateGSMByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateCompanyDescriptionByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateCompanyDescriptionByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateCompanyDescriptionByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateOccupationByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateOccupationByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateOccupationByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateProfile")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateProfile([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateProfile(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateProjectByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateProjectByUserId([FromHeader] UpdateProjectByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateProjectByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateStatusByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateStatusByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateStatusByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateInterestsByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateInterestsByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateInterestsByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateInformationByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateInformationByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateInformationByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateSkillsByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateSkillsByUserId([FromHeader] UpdateUserInfoByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateSkillsByUserId(request);

      return response.SerializeObject();
    }

    [HttpPost("UpdateEducationByUserId")]
    [Consumes("application/x-www-form-urlencoded")]
    public async Task<string> UpdateEducationByUserId([FromHeader] UpdateEducationByUserIdRequest request)
    {
      var response = await _companyAndPersonService.UpdateEducationByUserId(request);

      return response.SerializeObject();
    }
  }
}
