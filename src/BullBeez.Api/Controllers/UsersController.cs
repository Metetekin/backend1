﻿using AutoMapper;

using BullBeez.Api.Classed;
using BullBeez.Api.Helper;
using BullBeez.Core.DTO.CompanyAndPersonDTO;
using BullBeez.Core.Resource;
using BullBeez.Core.DTO.MailDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse.Auth;
using BullBeez.Core.Entities;
using BullBeez.Core.Enums;
using BullBeez.Core.Helper;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
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
            this._externalAuthHelper = externalAuthHelper;
        }

        [HttpGet("GetDeneme")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetDeneme()
        {
            //var response = await IsMailExists(new IsMailExistsRequest {EmailAddress="aytugkonuralp@gmail.com",});
            //var response = await UserShowByUserId(new UserShowByUserIdRequest { UserId=6,RequestUserId= 17});
            //var response = await FollowUserByUserId(new FollowUserByUserIdRequest { UserId = 17,ToUserId=28,FollowType=EnumFollowType.Follows,WorkerFollowType=EnumWorkerFollowType.Waiting });
            //var response = await UserInfoByUserId(new NewLoginRequest { UserId = 6 });
            var response = await GetUserPosts(new GetUserPostsRequest { UserId = 6, RequestCount = 10, FilterTopicsStr = "[\"3D Yazıcı\"]" });
            return response.SerializeObject();
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

        [HttpPost("UploadBannerImage")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> UploadBannerImage([FromHeader] UploadImageRequest request)
        {
            var response = await _companyAndPersonService.UploadBannerImage(request);

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

        [HttpPost("GetUserByUsername")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUserByUsername([FromHeader] UserInfoByUsernameRequest request)
        {
            var response = await _companyAndPersonService.GetUserByUsername(request);

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

        [HttpPost("UpgradePostToBoardById")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> UpgradePostToBoardById([FromHeader] UpgradePostRequest request)
        {
            var response = await _companyAndPersonService.UpgradePostToBoardById(request);

            return response.SerializeObject();
        }

        [HttpPost("NewPostReport")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> NewPostReport([FromHeader] NewPostReportRequest request)
        {
            var response = await _companyAndPersonService.NewPostReport(request);

            return response.SerializeObject();
        }

        [HttpPost("GetUserPostById")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUserPostById([FromHeader] GetUserPostByIdRequest request)
        {
            var response = await _companyAndPersonService.GetUserPostById(request);

            return response.SerializeObject();
        }

        [HttpPost("GetUsersUserPostByUserId")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUsersUserPostByUserId([FromHeader] GetUsersUserPostByUserIdRequest request)
        {
            var response = await _companyAndPersonService.GetUsersUserPostByUserId(request);

            return response.SerializeObject();
        }

        [HttpPost("GetKitleFonlamaUserPosts")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetKitleFonlamaUserPosts([FromHeader] GetUserPostsRequest request)
        {
            var response = await _companyAndPersonService.GetKitleFonlamaUserPosts(request);

            return response.SerializeObject();
        }

        [HttpPost("GetUserPosts")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUserPosts([FromHeader] GetUserPostsRequest request)
        {
            var response = await _companyAndPersonService.GetUserPosts(request);

            return response.SerializeObject();
        }

        [HttpPost("GetLastPostByUserId")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetLastPostByUserId([FromHeader] GetLastPostByUserIdRequest request)
        {
            var response = await _companyAndPersonService.GetLastPostByUserId(request);

            return response.SerializeObject();
        }

        [HttpPost("GetSponsoredUserPost")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetSponsoredUserPost([FromHeader] BaseRequest request)
        {
            var response = await _companyAndPersonService.GetSponsoredUserPost(request);

            return response.SerializeObject();
        }

        [HttpPost("GetMoreUserPosts")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetMoreUserPosts([FromHeader] GetMoreUserPostsRequest request)
        {
            var response = await _companyAndPersonService.GetMoreUserPosts(request);

            return response.SerializeObject();
        }

        [HttpPost("GetMoreCommentsByPostId")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetMoreCommentsByPostId([FromHeader] GetMoreCommentsRequest request)
        {
            var response = await _companyAndPersonService.GetMoreCommentsByPostId(request);

            return response.SerializeObject();
        }

        [HttpPost("GetUserPostsByUserId")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetUserPostsByUserId([FromHeader] BaseRequest request)
        {
            var response = await _companyAndPersonService.GetUserPostsByUserId(request);

            return response.SerializeObject();
        }

        [HttpPost("GetPostCommentsByPostId")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> GetPostCommentsByPostId([FromHeader] GetPostCommentsByPostIdRequest request)
        {
            var response = await _companyAndPersonService.GetPostCommentsByPostId(request);

            return response.SerializeObject();
        }

        [HttpPost("LikePostById")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> LikePostById([FromHeader] LikeByIdRequest request)
        {
            var response = await _companyAndPersonService.LikePostById(request);

            return response.SerializeObject();
        }

        [HttpPost("DeletePostById")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> DeletePostById([FromHeader] DeleteByIdRequest request)
        {
            var response = await _companyAndPersonService.DeletePostById(request);

            return response.SerializeObject();
        }

        [HttpPost("LikeCommentById")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> LikeCommentById([FromHeader] LikeByIdRequest request)
        {
            var response = await _companyAndPersonService.LikeCommentById(request);

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

        [HttpPost("SetMailPermission")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<string> SetMailPermission([FromHeader] SetBoolValueRequest request)
        {
            var response = await _companyAndPersonService.SetMailPermission(request);

            return response.SerializeObject();
        }

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
