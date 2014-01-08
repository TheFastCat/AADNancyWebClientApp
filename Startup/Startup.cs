using Microsoft.Owin;
using Owin;
using Nancy.Owin;
using Microsoft.Owin.Extensions;
using System;
using Core;
using Microsoft.Owin.Security.ActiveDirectory;

namespace Startup
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
            new WindowsAzureActiveDirectoryBearerAuthenticationOptions
            {
                Audience = "https://SalesApplication.onmicrosoft.com/WebAPIDemo", // aka resource / 'APP_ID_URI'
                Tenant   = "SalesApplication.onmicrosoft.com" // aka domain
            });
            app.UseNancy();
            app.UseStageMarker(PipelineStage.MapHandler);//required to display Nancy assets on IIS
        }
    }
}
