AADNancyWebClientApp
=======================================
NancyFx web client app (OWIN, SystemWeb host) authenticated via Azure Active Directory using Active Directory Authentication Library.

The basis for this project is this blog post http://goo.gl/OAo6XS

as well as lots of NancyFx documentation regarding authentication:

	https://github.com/NancyFx/Nancy/wiki/Authentication-overview
	https://github.com/NancyFx/Nancy/wiki/Stateless-Authentication
	https://github.com/NancyFx/Nancy/wiki/The-Application-Before%2C-After-and-OnError-pipelines
	https://github.com/NancyFx/Nancy/wiki/The-before-and-after-module-hooks

and this blog post about integrating ADAL with Nancy:

	http://dhickey.ie/post/2014/01/04/Protecting-a-Self-Hosted-Nancy-API-with-MicrosoftOwinSecurityActiveDirectory.aspx

	  		   ////// The implementations of ADAL authentication with AAD is directly tied to configurations within AAD. If configurations //////
	!	!     /NOTE/ are changed or removed dependant application authentication will break. Because of this careful inspection and under /NOTE/     !	 !
			 ////// -standing of AAD configurations and how consuming ADAL calls utilize them is important.                              //////



