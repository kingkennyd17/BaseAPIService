using Fintrak.Model.SystemCore.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Host.ServicePortal
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly UserManager<UserSetup> _userManager;

        public TokenValidationMiddleware(RequestDelegate next/*, UserManager<UserSetup> userManager*/)
        {
            _next = next;
            //_userManager = userManager;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<UserSetup> userManager)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                var jwtToken = token.ToString().Split(" ").Last();
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await userManager.FindByNameAsync(userId);
                    if (user != null && user.CurrentSessionId != jwtToken)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        await context.Response.WriteAsync("Concurrent login detected. Session invalidated.");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }

}
