using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace DataManagementSystem.Filters
{
    public class VerifyAuthFilter : IActionFilter
    {
        private const string _authorizationFieldName = "Authorization";
        private const string _userFieldName = "User";
        private const string _defaultUser = "Axinom";
        private readonly string _user;

        private const string _passwordFieldName = "Password";
        private const string _defaultPassword = "Monixa";
        private readonly string _password;

        private const string AuthorizationParameterName = "authorized";

        public VerifyAuthFilter(IConfiguration configuration)
        {
            var user = configuration[_userFieldName];
            _user = string.IsNullOrEmpty(user) ? _defaultUser : user;

            var password = configuration[_passwordFieldName];
            _password = string.IsNullOrEmpty(password) ? _defaultPassword : password;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool authorized = true; //the desired answer

            string authHeader = context.HttpContext.Request.Headers[_authorizationFieldName];
            if (authHeader == null) authorized = false; // could add log here?

            var parts = authHeader.Split(' ', 2);
            if (parts.Length != 2) authorized = false; //idem

            var user = parts[0];
            var password = parts[1];
            if(user != _user || password != _password) authorized = false; //idem

            context.ActionArguments[AuthorizationParameterName] = authorized;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        { }
    }
}
