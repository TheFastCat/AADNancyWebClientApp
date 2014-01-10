using Nancy.Responses;
using Nancy.Security;
using System;

namespace Nancy
{
    using Core.ADAL;

    /// <summary>
    /// Module containing a single login route as a launching point for Azure Active Directory authentication prompting.
    /// </summary>
    /// <remarks>
    /// This is the only unsecured route for the application; secure routes redirect here in order to authenticate unauthorized users
    /// </remarks>
    public class AuthenticationModule : NancyModule
    {
        public AuthenticationModule()
        {
            Get["/login"] = _ =>
            {
                // send a request to Azure AAD via oauth2 using a URL we create containing arguments.
                // AAD will in turn prompt the user (via web dialog) to authenticate themselves...
                // only after providing VALID CREDENTIALS for a user existing within the  AAD.TENANT_ID (aka 'domain'/'directory')
                // AAD will return an authorization code to REPLY_URL. This authorization code can then be used to retrieve 
                // a security token.
                // (see CatchModule.cs for reception of this authorization code and its use to retrieve an authentication token)
                return new RedirectResponse(ActiveDirectoryAuthenticationHelper.GetAuthorizationURL());
            };
        }
    }

    /// <summary>
    /// Routes defined here require an authenticated user in order to access ; the module will redirect
    /// unauthenticated users to the login route.
    /// </summary>
    public class SecureModule : NancyModule
    {
        public SecureModule()
        {
            // this hook will redirect all matched routes in the module to the /login route if 
            // the user hasn't been authenticated yet. Removing this hook will not redirect the users
            // and they will just receive a 402 Unauthorized StatusCode (and a blank browser).
            Before += ctx =>
            {
                // in the case that AAD returns an error we should display it
                if (ctx.Request.Query.error.HasValue)
                {
                    // format the returned error (along with a custom suggestion)
                    string errorDesc = string.Format("{0}\n\n{1}\n\n{2}",
                                                     ctx.Request.Query.error,
                                                     ctx.Request.Query.error_description,
                                                     "Verify you are not currently logged into a separate, unauthorized Active Directory domain account.");//I run into this a lot during development so adding reminder

                    Context.Response            = Response.AsText(errorDesc);
                    Context.Response.StatusCode = HttpStatusCode.Forbidden;

                    return Context.Response;
                }

                return ctx.CurrentUser == null ||
                       String.IsNullOrWhiteSpace(ctx.CurrentUser.UserName)
                    ? new RedirectResponse("/login")
                    // else allow request to continue unabated
                    : null;
            };

            this.RequiresHttps();          // http://goo.gl/lQdY8a
            this.RequiresAuthentication(); // http://goo.gl/Dtxhve

            Get["/"] = _ =>
            {
                return "Hello " + Context.CurrentUser.UserName + "!";
            };

            // route useful for demonstrating secured routes redirect unauthorized users to /login => (ane then back to) => "/"
            Get["/Private"] = _ =>
            {
                return "Secret stuff!";
            };
        }
    }
}