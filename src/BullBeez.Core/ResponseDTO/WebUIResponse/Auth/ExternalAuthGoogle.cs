using BullBeez.Core.RequestDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BullBeez.Core.ResponseDTO.WebUIResponse.Auth
{
    public class ExternalAuthGoogle
    {

        public string DeviceId { get; set; }
        public string id_token { get; set; }

    }
}
