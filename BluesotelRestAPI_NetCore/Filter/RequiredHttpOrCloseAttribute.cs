using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluesotelRestAPI_NetCore.Filter
{
    public class RequiredHttpOrCloseAttribute: RequireHttpsAttribute
    {
        // Overridding the default behaviour of redirecting the request from HTTP -> HTTPS
        // Now insted of redirecting it will throw an error of an bad request
        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            filterContext.Result = new StatusCodeResult(400);
        }
    }
}
