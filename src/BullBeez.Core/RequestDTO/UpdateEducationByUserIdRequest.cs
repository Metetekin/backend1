using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.RequestDTO
{
    public class UpdateEducationByUserIdRequest : BaseRequest
    {
        public string EducationListStr { get; set; }
       
    }

}
