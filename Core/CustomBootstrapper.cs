using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Nancy.Diagnostics;
using Core;
using System.Reflection;

namespace Core
{
    /// <summary>
    /// See NancyFx's documentation http://goo.gl/HeXsp
    /// </summary>
    public class CustomBootstrapper : DefaultNancyBootstrapper
    {
        // uncomment to remove default ctor
        public CustomBootstrapper()
        {

        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            // enable for NancyFx diagnostics
            StaticConfiguration.EnableRequestTracing = true;                    
            base.ConfigureApplicationContainer(container);
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
