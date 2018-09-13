using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace ControlPanel.Middleware
{
    
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string user;
        private readonly string password;

        public AuthMiddleware(RequestDelegate next, string user, string password)
        {
            this.next = next;
            this.user = user;
            this.password = password;
        }


        public async Task Invoke(HttpContext context)
        {
            context.Response.Headers.Add("Authorization", $"{user} {password}");
            await next.Invoke(context);
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    context.Response.Headers.Add("Authorization", new[] { watch.ElapsedMilliseconds.ToString() });


        //    string authHeader = context.Request.Headers[];
        //    if (authHeader != null && authHeader.StartsWith("Basic "))
        //    {
        //        // Get the encoded username and password
        //        var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
        //        // Decode from Base64 to string
        //        var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
        //        // Split username and password
        //        var username = decodedUsernamePassword.Split(':', 2)[0];
        //        var password = decodedUsernamePassword.Split(':', 2)[1];
        //        // Check if login is correct
        //        if (IsAuthorized(username, password))
        //        {
        //            await next.Invoke(context);
        //            return;
        //        }
        //    }
        //    // Return authentication type (causes browser to show login dialog)
        //    context.Response.Headers["WWW-Authenticate"] = "Basic";
        //    // Add realm if it is not null
        //    if (!string.IsNullOrWhiteSpace(realm))
        //    {
        //        context.Response.Headers["WWW-Authenticate"] += $" realm=\"{realm}\"";
        //    }
        //    // Return unauthorized
        //    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        //}
        //// Make your own implementation of this
        //public bool IsAuthorized(string username, string password)
        //{
        //    // Check that username and password are correct
        //    return username.Equals("User1", StringComparison.InvariantCultureIgnoreCase)
        //           && password.Equals("SecretPassword!");
        //}
    }
}