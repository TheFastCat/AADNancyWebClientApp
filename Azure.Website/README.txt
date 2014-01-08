  ///////////////////////////////////////////////
 //// OVERVIEW               version 0.0.03 ////
///////////////////////////////////////////////

	This .csproj is basically a "husk" that kicks off the OWIN pipeline.
	The only thing it does is reference it's hosting mechanism: Microsoft.Owin.Host.SystemWeb (OWIN hosting on IIS) which in turns starts up the OWIN pipeline ; 
	application functionality
	resides within the various SalesApplication .csprojs that this project references.

	   ////////////////
	  //Configuration/
	 ////////////////

	-The OWIN pipeline's Startup class is invoked via explicit declaration in this .csproj's Properties/AssemblyInfo.cs file
	-The required WCF endpoint configurations by SalesApplication.Adapter.Legacy.Wcf are also defined within web.config.
	 These settings are currently required to consume legacy's WCF pipe
	 However when deployed to Azure (The cloud) the website cannot drill into our internal network to access the WCF services since they aren't publicly hosted,
	 even though we can support remote NTLM authentication using their network credentials 

	   /////////////
	  //Deployment/
	 /////////////

	This website is currently deployed to Windows Azure and is publicly accessible via http://salesapp.us/
	The deployed version probably doesn't match the code base though since there isn't a continuous integration/deployment pipeline in place 
	(yet)

	   ////////////////////////
	  //Continuous Deployment/
	 ////////////////////////

	 TODO!