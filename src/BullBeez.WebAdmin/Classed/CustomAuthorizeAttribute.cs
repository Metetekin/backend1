using BullBeez.WebAdmin.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BullBeez.WebAdmin.Classed
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContextBase)
        {
            var profileData = httpContextBase.Session["UserProfile"] as NewUserResponse;
            if (profileData == null)
            {
                return false;
            }
            return true;

        }
    }
}