UrlShortener Helper ReadMe

The Url Shortener WebMatrix helper does what the names implies, make a long url, short. By now I am sure we have all used some sort of Url Shortener. The basic premise is you have a long url that you want to not only shrink but get some stats on. Here is a example using Bit.ly: before: http://DeveloperSmackdown.com, after: http://bit.ly/acRAbT. Currently we are supporting the simple bit.ly scenario but there is a lot in the works.

---->>
Where can I get it? 
	Well on CodePlex of course: http://urlShortener.CodePlex.com 

---->>
How do I use it?

At this point it’s nothing magical. Lets start with getting setup. There are two assemblies at this point, which need to be copied into your bin folder.

	Microsoft.Web.Helpers.UrlShortener.dll. This is the “Helper”. This assembly exposes the API’s and settings you interact with. 

	Microsoft.Web.Helpers.UrlShortener.Bitly.dll. This assembly is what I like to call the provider. There will be one of these for each URLShortener provider we choose to implement. 

With your assemblies copied into the bin folder lets look at where things start, _appStart.cshtml. AppStart is the place where we will configure the helper for usage throughout the site. Right now there are only two settings you have to configure, UserName and ApiKey. Of course down the road as we add more providers this might change based on the provider you’re using.

	@{ 
		UrlShortener.Settings.UserName = "YOUR BITLY USER NAME"; 
		UrlShortener.Settings.ApiKey = "YOUR BITLY API KEY"; 
	}

Simple enough. Now lets actually use it. In my default.cshtml I call the helper like so:
	@UrlShortener.Shorten(@"http://DeveloperSmackdown.com")
 
If I was in a code block it would look like the following:
	@{ var dsShortLink = @UrlShortener.Shorten(@"http://DeveloperSmackdown.com"); }
 
The result of each of those is the following string returned: http://bit.ly/acRAbT. Of course if you take the same code and run it under your UserName and ApiKey it will yield a different result.

Of course check out the site for more documentaion: http://urlShortener.CodePlex.com

---->>
Change Log

v.0.9.1
	Added overload for the Shorten Method. Now you can override the 'settings' on an individual call
	Shorten the Assembly Name
	Added a NuGet Package
	Updated namespaces overall

v.0.9.0
	Inital Creation