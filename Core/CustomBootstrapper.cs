using Core.ADAL;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Diagnostics;
using Nancy.TinyIoc;
using System;

namespace Core
{
    /// <summary>
    /// See NancyFx's documentation http://goo.gl/HeXsp
    /// </summary>
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// This method exists for the purpose of enabling Nancy's Stateless authentication (http://goo.gl/Dtxhve)
        /// </summary>
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            // At request startup we modify the request pipelines to
            // include stateless authentication
            //
            // Configuring stateless authentication is simple. Just use the 
            // NancyContext to get the apiKey. Then, use the authorization code to get 
            // your user's identity from Azure Active Directory via ADAL.
            //
            // If the authorization code required to do this is missing, NancyModules
            // secured via RequiresAuthentication() cannot be invoked...
            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                {
                    // the only way a user will be authenticated is if a request contains an authentication code
                    // attached to it...
                    if (!nancyContext.Request.Query.code.HasValue)
                    {
                        return null;
                    }

                    try
                    {
                        //for now, we will pull the apiKey from the querystring, 
                        //but you can pull it from any part of the NancyContext
                        var authorizationCode = (string)nancyContext.Request.Query.code;

                        //get the user identity however you choose to (for now, using a static class/method)
                        return ActiveDirectoryAuthenticationHelper.GetAuthenticatedUserIDentity(authorizationCode);
                    }
                    // exceptions during ADAL authentication will block user authentication
                    catch (ArgumentNullException)
                    {
                        return null;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                });

            StatelessAuthentication.Enable(pipelines, configuration);
        }

        /// <summary>
        /// Configures a password for the NancyFx diagnostics page useful for debugging.
        /// Nancy's diagnostics can be reached via http://<address-of-your-application>/_Nancy/
        /// Login password is configured below
        /// </summary>
        /// <see cref="https://github.com/NancyFx/Nancy/wiki/Diagnostics"/>
        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"hi" }; }
        }
    }
}
