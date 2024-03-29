﻿using BullBeez.Core.Entities;
using BullBeez.Core.RequestDTO;
using BullBeez.Core.RequestDTO.WebUIRequest;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.ResponseDTO.WebUIResponse;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Services
{
    public interface ICompanyAndPersonService
    {
        #region User işlemleri
        Task<ApiResult<UserControlResponse>> UserControl(object baseRequest);

        Task<ApiResult<UserInfoByUserIdResponse>> UserInfoByUserId(BaseRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> ResetPassword(BaseRequest request);

        Task<ApiResult<UserInfoByUserIdResponse>> UserShowByUserId(UserShowByUserIdRequest request);

        Task<ApiResult<UploadCvResponse>> UploadCv(UploadCvRequest request);
        Task<ApiResult<UploadCvResponse>> DeletePDF(UploadCvRequest request);
        Task<ApiResult<PopUpResponse>> PopUpIsRead(PopUpIsReadRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> PopUpReading(PopUpReadingRequest request);
        Task<ApiResult<NewUserResponse>> NewUser(NewUserContract request);

        Task<ApiResult<NewUserResponse>> NewLogin(NewLoginRequest request);

        Task<ApiResult<NewUserResponse>> LoginUser(NewLoginRequest request);
        Task<ApiResult<SendMailResponse>> IsMailExists(IsMailExistsRequest request);

        Task<ApiResult<UploadImageResponse>> UploadImage(UploadImageRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> Logout(BaseRequest request);

        Task<ApiResult<List<SearchUserByFilterResponse>>> SearchUserByFilter(SearchUserByFilterRequest request);
        #endregion


        #region Takip işlemleri
        Task<ApiResult<FollowUserByUserIdResponse>> FollowUserByUserId(FollowUserByUserIdRequest request);
        Task<ApiResult<List<GetFollowUserByUserIdResponse>>> GetFollowUserByUserId(FollowUserByUserIdRequest request);
        Task<ApiResult<List<GetFollowUserByUserIdResponse>>> GetWorkerUserByUserId(FollowUserByUserIdRequest request);
        Task<ApiResult<List<GetFollowUserByUserIdResponse>>> GetWaitingWorkerList(FollowUserByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> WorkerApprovedAndCancel(FollowUserByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UnfollowUserByUserId(FollowUserByUserIdRequest request);


        #endregion

        Task<ApiResult<ExistsUserNameResponse>> IsExistsUserName(IsExistsUserNameRequest request);
        Task<ApiResult<List<InterestListResponse>>> GetInterestsListByUserId(BaseRequest request);
        Task<ApiResult<List<UserNotificationResponse>>> GetListNotificationByUserId(BaseRequest request);

        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateBullbeezSentenceByUserId(UpdateUserInfoByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateGSMByUserId(UpdateUserInfoByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateCompanyDescriptionByUserId(UpdateUserInfoByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateStatusByUserId(UpdateUserInfoByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateOccupationByUserId(UpdateUserInfoByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateProfile(UpdateUserInfoByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> NewPost(NewPostRequest request);
        Task<ApiResult<List<UserPostListResponse>>> GetUserPosts(GetUserPostsRequest request);
        Task<List<PostDatatableModel>> GetUserPostsDatatable(GetUsersRequestDTO request);

        Task<ApiResult<BaseInsertOrUpdateResponse>> NewComment(NewCommentRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateProjectByUserId(UpdateProjectByUserIdRequest request);
        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateEducationByUserId(UpdateEducationByUserIdRequest request);

        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateInformationByUserId(UpdateUserInfoByUserIdRequest request);

        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateInterestsByUserId(UpdateUserInfoByUserIdRequest request);

        Task<ApiResult<BaseInsertOrUpdateResponse>> UpdateSkillsByUserId(UpdateUserInfoByUserIdRequest request);


        Task<List<UserListResponse>> GetUsers(GetUsersRequestDTO request);
        Task<UserListResponse> GetUserById(int userId);
    }
}
