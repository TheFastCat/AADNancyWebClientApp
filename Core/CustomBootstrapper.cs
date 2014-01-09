using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Nancy.Diagnostics;
using Core;
using System.Reflection;
using Nancy.Authentication.Stateless;
using Core.ADAL;
using Nancy.Routing;
using System;

namespace Core
{
    /// <summary>
    /// See NancyFx's documentation http://goo.gl/HeXsp
    /// </summary>
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
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

            base.RequestStartup(requestContainer, pipelines, context);
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
