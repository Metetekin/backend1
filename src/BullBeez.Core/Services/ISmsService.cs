using BullBeez.Core.DTO.Sms;
using BullBeez.Core.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Services
{
	public interface ISmsService
	{
		Task<ApiResult<object>> SendSmsAsync(SmsRequestModel smsRequestModel);
	}
}
