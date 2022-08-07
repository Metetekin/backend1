using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BullBeez.Api.Classed
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var startTime = DateTime.UtcNow;

            var watch = Stopwatch.StartNew();
            await _next.Invoke(context);
            watch.Stop();

            var logTemplate = @"Client IP: {clientIP}
                                Request path: {requestPath}
                                Request content type: {requestContentType}
                                Request content length: {requestContentLength}
                                Start time: {startTime}
                                Duration: {duration}";


            var sdf = context.Connection.RemoteIpAddress.ToString();
            var asgsag = context.Request.Path;
            var asgfsa = context.Request.ContentType;
            var sagsa = context.Request.ContentLength;
            var sadgas = startTime;
            var sagsaga = watch.ElapsedMilliseconds;
        }
    }
}
