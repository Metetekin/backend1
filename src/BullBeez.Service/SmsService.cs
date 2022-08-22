using BullBeez.Core.DTO.Sms;
using BullBeez.Core.Resource;
using BullBeez.Core.ResponseDTO;
using BullBeez.Core.Services;
using BullBeez.Service.Connected_Services;
using SmsServiceSOAP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Service
{
	public class SmsService : ISmsService
	{
		public async Task<ApiResult<object>> SendSmsAsync(SmsRequestModel smsRequestModel)
		{
			ApiResult<object> response = new ApiResult<object>();
			try
			{
			
				using (var _smsService = new SmsClientCopy(SMSServiceSoapClient.EndpointConfiguration.SMSServiceSoap))
				{
					ArrayOfString recipients = new ArrayOfString();
					recipients.AddRange(smsRequestModel.PhoneNumbers);
					ArrayOfString messages = new ArrayOfString();
					messages.AddRange(smsRequestModel.Content);
					SendSMSRequest sendSMSRequest = new SendSMSRequest(new SendSMSRequestBody
					{
						iysrecipienttype = "BIREYSEL",
						originator = "DEMO",
						user = "bullbeez",
						password = "6632-Hbn",
						onlengthproblem = enmOnLengthProblem.SendAllPackage,
						receipents = recipients,
						messages = messages
   				});

					var res = await ((SmsServiceSOAP.SMSServiceSoap)(_smsService)).SendSMSAsync(sendSMSRequest);

					if (res == null || res.Body == null || res.Body.SendSMSResult == null || res.Body.SendSMSResult.ErrorCode != "00")
					{
						response.IsError = true;
						response.Message = res?.Body?.SendSMSResult?.ID + res?.Body?.SendSMSResult?.ErrorCode;
						response.StatusCode = ResponseCode.Basarisiz;
						return response;
					}

					response.Message = "Mesaj gönderim başarılı.";
					response.IsError = false;
					response.StatusCode = ResponseCode.Basarili;
					return response;

				}

			}
			catch (Exception ex)
			{

				response.IsError = true;
				response.Message = ex.Message;
				response.StatusCode = ResponseCode.Basarisiz;
				return response;
			}
		}
	}
}
