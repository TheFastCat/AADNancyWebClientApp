using System;
using Nancy;
using Nancy.Responses;
using Nancy.Security;
using Nancy.Owin;
using System.Collections.Generic;
using Nancy.Routing;

namespace Nancy
{
    using Core.ADAL;
    public class NancyModules : NancyModule
    {
        public NancyModules()
        {
            Before += ctx =>
            {
                // pass through 
                if (ctx.Request.Path == "/login" || 
                    ctx.Request.Path ==  "/Home/CatchCode")
                    return null;

                return ctx.CurrentUser == null || String.IsNullOrWhiteSpace(ctx.CurrentUser.UserName)
                    ? new RedirectResponse("/login")
                    // else allow request to continue unabated
                    : null;
            };

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

            Get["/Home/CatchCode"] = _ =>
            {
                if (!Request.Query.code.HasValue)// todo - further validation of incoming code
                    return HttpStatusCode.Unauthorized;

                try
                {
                    // the code returned from AAD after authenticating a user
                    string authorizationCode = Request.Query.code;

                    //IUserIdentity userIdentity = ActiveDirectoryAuthenticationHelper.GetAuthenticatedUserIDentity(authorizationCode);

                    //// assign the context's current user
                    //Context.CurrentUser = userIdentity;

                    return Response.AsRedirect("/Home" + "?code=" + authorizationCode);
                }
                catch (ArgumentNullException)
                {
                    return HttpStatusCode.Forbidden;
                }
                catch (Exception)
                {
                    return HttpStatusCode.Unauthorized;
                }
            };
        }
    }

    public class SecureModule : NancyModule
    {
        public SecureModule()
        {
            Before += ctx =>
            {
                if (ctx.Request.Path == "/login")
                    return null;

                return ctx.CurrentUser == null || String.IsNullOrWhiteSpace(ctx.CurrentUser.UserName)
                    ? new RedirectResponse("/login")
                    // else allow request to continue unabated
                    : null;
            };

            this.RequiresAuthentication();

            Get["/Home"] = _ =>
            {
                return "Hello " + Context.CurrentUser.UserName + "!";
            };

            Get["/Private"] = _ =>
            {
                return "Secret stuff!";
            };
        }
    }
}