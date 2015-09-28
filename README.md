# AquilaDev.Owin.Security.LaunchKey
Middleware that enables an application to support LaunchKey's OAuth 2.0 authentication workflow.

## Using middleware

Before you can begin using middleware, you need a [Rocket](https://docs.launchkey.com/glossary.html#term-rocket). If you have not created a [Rocket](https://docs.launchkey.com/glossary.html#term-rocket) yet, you can use [_LaunchKey Getting Started Guide_](https://docs.launchkey.com/common/getting-started-guide.html) to create one.

Open a project **LaunchKey.Web** in the folder **examples**. There, you need to put your rocket id and client secret*. To do it, you need to open Startup.Auth.cs file and write something like this:
```
app.UseLaunchKeyAuthentication(
	rocketId: "1234567890",
	clientSecret: "e55tv2ch4e1103qxrr64wggj42ie61w0");
```
*Keys you can find in your [dashboard](https://dashboard.launchkey.com). You can find more about it [here](https://docs.launchkey.com/common/getting-started-guide.html#set-up-your-keys).

This project runs on specific uri, because we deal with OAuth, the uri is _http://local.launchkey.dev_. That is why you should'n forget to change **hosts** system file:
```
127.0.0.1		local.launchkey.dev
```

### Important

Before you start testing you need to setup Domain(OAuth) for your Rocket. If you do not do this you'll get an error during authorization.

To do that, you need to open your [Rocket](https://docs.launchkey.com/glossary.html#term-rocket) in [dashboard](https://dashboard.launchkey.com) and set field Domain(OAuth). In my case, I put:
```
local.launchkey.dev
```
