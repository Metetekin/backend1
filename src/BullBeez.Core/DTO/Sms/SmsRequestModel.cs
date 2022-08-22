using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.DTO.Sms
{
	public class SmsRequestModel
	{
		public string Title { get; set; }
		public string[] Content { get; set; }
		public string[] PhoneNumbers { get; set; }
	}
}
