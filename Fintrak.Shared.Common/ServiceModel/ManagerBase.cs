using Fintrak.Shared.Common.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.Shared.Common.ServiceModel
{
    public abstract class ManagerBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        // Fields to store the logged-in user information
        protected readonly string LoggedInUsername;
        protected readonly string LoggedInUserId;

        // Constructor where the fields are initialized
        protected ManagerBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            // Initialize fields with user information from the HTTP context
            LoggedInUsername = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            LoggedInUserId = _httpContextAccessor.HttpContext?.User?.Identity.Name;
        }

        // You can add other shared fields here as needed
    }

}
