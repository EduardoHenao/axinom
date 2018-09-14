using Microsoft.AspNetCore.Mvc.Filters;

namespace ControlPanel
{
    public class SignAuthFilter : ActionFilterAttribute
    {
        private readonly string _user;
        private readonly string _password;

        public SignAuthFilter(string user, string password)
        {
            _user = user;
            _password = password;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            context.HttpContext.Request.Headers.Add("Authorization", $"{_user} {_password}");
            base.OnActionExecuted(context);
        }
    }
}
