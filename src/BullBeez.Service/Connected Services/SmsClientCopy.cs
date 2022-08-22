using SmsServiceSOAP;
using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Service.Connected_Services
{
	internal class SmsClientCopy : SMSServiceSoapClient, IDisposable
	{
		public SmsClientCopy(EndpointConfiguration endpointConfiguration) : base(endpointConfiguration)
		{

		}
	}
}
