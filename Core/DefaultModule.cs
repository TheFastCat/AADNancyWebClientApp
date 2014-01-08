using Nancy;
using Nancy.Responses;

namespace Core
{
    public class DefaultModule : NancyModule
    {
        public readonly string HELLO_WORLD = "Hello World!";

        private const string TenantIdClaimType = "f721817b-99eb-4505-b220-850208ab5dd7";
        private static readonly string AppPrincipalId = "17c9a991-4954-48e4-9cf9-39b126ba975c";// 'clientID' for NancyDemoWebClient AAD config


        public DefaultModule()
        {
            Get["/hellonancy"] = _ => HELLO_WORLD;

            Get["/"] = _ =>
            {
                string authorizationUrl = string.Format("https://login.windows.net/{0}/oauth2/authorize?api-version=1.0&response_type=code&client_id={1}&resource={2}&redirect_uri={3}",
                                          TenantIdClaimType,
                                          AppPrincipalId,
                                          "https://SalesApplication.onmicrosoft.com/WebAPIDemo", // resource we want to access
                                          "https://localhost:44308/Home/CatchCode"); // reply url from NancyDemoWebClient AAD config

                // TODO - return the call from the API
                RedirectResponse response = new RedirectResponse(authorizationUrl);

                return response;
            };
        }
    }
}