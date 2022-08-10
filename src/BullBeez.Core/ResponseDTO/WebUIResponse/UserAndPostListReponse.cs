using System;
using System.Collections.Generic;
using System.Text;

namespace BullBeez.Core.ResponseDTO.WebUIResponse
{
  public class UserAndPostListReponse
  {
    public UserListResponse User { get; set; }
    public int PostCount { get; set; }
  }
}
