using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace MiniAuth
{
    /// <summary>
    /// Configures the web application.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initialize the class instance with the provided app configuration.
        /// </summary>
        /// <param name="configuration">The initial app configuration, read from appsettings.json and environment.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the current app configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Configures the app services.
        /// </summary>
        /// <param name="services">The current service container.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // configure Auth0-related services
            services
                .AddAuthentication(ConfigureAuthentication)
                .AddCookie()
                .AddOpenIdConnect(Const.Auth0, ConfigureOpenIdConnect);

            services.AddMvc();
        }

        /// <summary>
        /// Configures the app, depending on the hosting environment.
        /// </summary>
        /// <param name="app">The app to configure.</param>
        /// <param name="env">The hosting environment information.</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            app.UseStaticFiles();
            app.UseAuthentication(); // enable the authentication

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Configures authentication scheme.
        /// </summary>
        /// <param name="options">The options to store the configuration.</param>
        private void ConfigureAuthentication(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }

        /// <summary>
        /// Configures the Auth0 OIDC provider.
        /// </summary>
        /// <param name="options">The options to store the configuration.</param>
        private void ConfigureOpenIdConnect(OpenIdConnectOptions options)
        {
            // set the client settings
            options.Authority = $"https://{Configuration["Auth0:Domain"]}";
            options.ClientId = Configuration["Auth0:ClientId"];
            options.ClientSecret = Configuration["Auth0:ClientSecret"];

            // specify the authentication flow
            options.ResponseType = "code";

            // set the scopes
            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");

            options.CallbackPath = new PathString("/signin-auth0");
            options.ClaimsIssuer = "Auth0";
            options.SaveTokens = true; // save obtained tokens for local inspection and debugging

            // specify the claims mapping settings
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "name",
                RoleClaimType = "https://schemas.quickstarts.com/roles"
            };

            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context =>
                {
                    if (context.Properties.Items.ContainsKey("connection"))
                        context.ProtocolMessage.SetParameter("connection", context.Properties.Items["connection"]);

                    return Task.FromResult(0);
                },

                OnRedirectToIdentityProviderForSignOut = (context) =>
                {
                    var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

                    var postLogoutUri = context.Properties.RedirectUri;
                    if (!string.IsNullOrEmpty(postLogoutUri))
                    {
                        if (postLogoutUri.StartsWith("/"))
                        {
                            // transform to absolute
                            var request = context.Request;
                            postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                        }
                        logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
                    }

                    context.Response.Redirect(logoutUri);
                    context.HandleResponse();

                    return Task.CompletedTask;
                }
            };
        }
    }
}
