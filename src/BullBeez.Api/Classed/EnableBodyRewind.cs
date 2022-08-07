using Microsoft.AspNetCore.Mvc.Filters;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BullBeez.Api.Classed
{
    public class EnableBodyRewind : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var bodyStr = "";
            var req = context.HttpContext.Request;

            // Allows using several time the stream in ASP.Net Core

            var stream = new StreamReader(req.Body);
            //var body = stream.ReadToEnd();
            // Rewind, so the core is not lost when it looks the body for the request

            // Do whatever work with bodyStr here

        }
    }
}
