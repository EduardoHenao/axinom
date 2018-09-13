using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManagementSystem.Filters
{
    public class AuthFilter : ActionFilterAttribute
    {
        private readonly string _user;
        private readonly string _password;

        public AuthFilter(string user, string password)
        {
            _user = user;
            _password = password;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string authHeader = context.HttpContext.Request.Headers["Authorization"];
            //if (authHeader == null) _logger.LogWarning("AuthFilter no Authorization in header");
            var parts = authHeader.Split(' ', 2);
            //if(parts.Length != 2 ) _logger.LogWarning("AuthFilter Authorization in header inconsistent");
            var user = parts[0];
            var password = parts[1];
            if(user != _user || password != _password)
            {
                //_logger.LogWarning("AuthFilter Authorization in header incorrect");
                return;
            }
            
            base.OnActionExecuting(context);
        }

        //public override void OnActionExecuted(ActionExecutedContext context)
        //{
        //    _logger.LogWarning("ClassFilter OnActionExecuted");
        //    base.OnActionExecuted(context);
        //}

        //public override void OnResultExecuting(ResultExecutingContext context)
        //{
        //    _logger.LogWarning("ClassFilter OnResultExecuting");
        //    base.OnResultExecuting(context);
        //}

        //public override void OnResultExecuted(ResultExecutedContext context)
        //{
        //    _logger.LogWarning("ClassFilter OnResultExecuted");
        //    base.OnResultExecuted(context);
    }
}
