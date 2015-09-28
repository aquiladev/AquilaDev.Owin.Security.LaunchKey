# AquilaDev.Owin.Security.LaunchKey
Middleware that enables an application to support LaunchKey's OAuth 2.0 authentication workflow.

## Using middleware

Before you can begin using middleware, you need a [Rocket](https://docs.launchkey.com/glossary.html#term-rocket). If you have not created a [Rocket](https://docs.launchkey.com/glossary.html#term-rocket) yet, you can use [_LaunchKey Getting Started Guide_](https://docs.launchkey.com/common/getting-started-guide.html) to create one.

Than open a project **LaunchKey.Web** in folder examples. There you need to put your rocket id and client secret. For it you need to open Startup.Auth.cs file and write something like this:
```
app.UseLaunchKeyAuthentication(
	rocketId: "1234567890",
	clientSecret: "e55tv2ch4e1103qxrr64wggj42ie61w0");
```
*Keys you can find in your [dashboard](https://dashboard.launchkey.com). More about it you can find [here](https://docs.launchkey.com/common/getting-started-guide.html#set-up-your-keys).

This project runs on specific uri, because we have a deal with OAuth(if you know what I mean), the uri is _http://local.launchkey.dev_. That is why you should'n forget to change **hosts** system file:
```
127.0.0.1		local.launchkey.dev
```

### Important

Before you start to test it you need to setup Domain(OAuth) for your Roket. If you will not do it you will have error during authorization.

For it you need to open your [Rocket](https://docs.launchkey.com/glossary.html#term-rocket) in [dashboard](https://dashboard.launchkey.com) and set field Domain(OAuth). In my case I put:
```
local.launchkey.dev
```
