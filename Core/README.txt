  ///////////////////////////////////////////////
 //// Reid's Temp Nancy Fix  version 0.0.02 ////
///////////////////////////////////////////////

	As of this writing 11.20.2013 NancyFx has a bug involving finding views embedded as resources in satellite assemblies:
http://goo.gl/2sf0Zd . Reid added a fix (really the guy in that linked discussion discovered it back in August but never fixed it)
and sent a pull request to NancyFx. Until it is merged and the nuget package is updated though the Nancy.dll in this directory 
should be referenced as it contains the fix. The official nuget package for Nancy doesn't and might not for a while.

- Update 11/21/2013 - After a lot of work with people much smarter than me within the NancyFx community there is a follow up fix 
(http://goo.gl/0KaXia) which works even better. It's included in this .csproj within the "SmartGuysNancyFix" directory and referenced throughout 
the .sln until official nuget package replaces it. Might be a while. Additionally I removed the assemblies mentioned above to replace them
with the SmartGuysNancyFix.
