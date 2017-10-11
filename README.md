# Auth0-miniauth

Minimal ASP.NET Core MVC App using Auth0 authentication

## Requirements

The app is built on `ASP.NET Core 2.0` with MVC and `Auth0` authentication.
 Windows 10 + Visual Studio 2017 with latest updates were used, but it should
 run on any OS supported by [.NET Core](https://www.microsoft.com/net/core).

## Auth0 configuration

1. Create a new "Regular Web Applications" [client](https://auth0.com/docs/clients) in you target Auth0 tenant.
2. Get the [application keys](https://auth0.com/docs/quickstart/webapp/aspnet-core/00-intro#get-your-application-keys)
   that will be in the next sections below.

## Running locally

In order to run it locally, you need first to configure the settings for Auth0
 Edit the file `appsettings.Development.json` and fill in the application keys
 obtained above.

_Note: Make sure to not commit this file in source control!_

For the Auth0 client, configure the following values as described
 [here](https://auth0.com/docs/quickstart/webapp/aspnet-core/00-intro#configure-callback-urls):

In "Callback URLs" add `http://localhost:61766/signin-auth0`

In "Allowed Logout URLs" add `http://localhost:61766/`

_Note: Adjust the URLs accordingly if you run it on another local development web server._

## Running on hosting

Configure environment variables for the following keys:

* `Auth0:Domain` 
* `Auth0:ClientId`
* `Auth0:ClientSecret`

For the Auth0 client, configure the following values as described
 [here](https://auth0.com/docs/quickstart/webapp/aspnet-core/00-intro#configure-callback-urls):

In "Callback URLs" add `<YOUR DEPLOYMENT URL>/signin-auth0`

In "Allowed Logout URLs" add `<YOUR DEPLOYMENT URL>`

Where `<YOUR DEPLOYMENT URL>` can be something like
 `https://myauth0app-test.azurewebsites.net/` for deployments on Azure Web Apps.

_Note: Adjust the URLs accordingly if you run it on another local development
 web server._

## Hints

All functionality is concentrated in files `Startup.cs` and
 `Controllers/AccountController.cs` as per
 [Auth0 tutorial](https://auth0.com/docs/quickstart/webapp/aspnet-core/v2/00-intro).

The app on Azure may fail silently if Auth0 environment variables are not
 configured. See the **Running on hosting** section on how to configure it.