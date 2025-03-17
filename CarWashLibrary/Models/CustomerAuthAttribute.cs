using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;

namespace Car_Wash_Library.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomerAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = headers?.Split(" ").Last();
            if (token == null)
            {
                context.Result = new JsonResult(

                    new { message = "You are not authortized to use this API" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else
            {
                var validator = context.HttpContext.RequestServices.GetRequiredService<TokenValidator>();
                var user = validator.Validate(token).Result;
                if (user == null)
                {
                    context.Result = new JsonResult(new { message = "You are not authortized to use this API" })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
                else
                {

                    var roles = Roles.Split(",").ToList();
                    var roleExists = roles.Exists(c => c == user.Role.RoleName);
                    if (!roleExists)
                    {
                        context.Result = new JsonResult(new { message = "You are not authortized to use this API" })
                        {
                            StatusCode = StatusCodes.Status401Unauthorized
                        };
                    }

                }
            }
        }
    }
}
