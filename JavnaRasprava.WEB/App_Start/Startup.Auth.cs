using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using System;
using JavnaRasprava.WEB.DomainModels;

namespace JavnaRasprava.WEB
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth( IAppBuilder app )
        {
            // Configure the db context and user manager to use a single instance per request
			app.CreatePerOwinContext( ApplicationDbContext.Create );
			app.CreatePerOwinContext<ApplicationUserManager>( ApplicationUserManager.Create );

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
			app.UseCookieAuthentication( new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                },
				CookieName = Infrastructure.AppConfig.GetStringValue( "CookieName" ),
				CookieDomain = Infrastructure.AppConfig.GetStringValue( "CookieDomain" )
			} );
            
			app.UseExternalSignInCookie( DefaultAuthenticationTypes.ExternalCookie );

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

			// Prod
			var x = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions();
			x.Scope.Add( "email" );
			x.AppId = System.Configuration.ConfigurationManager.AppSettings[ "FB.AppId" ];
			x.AppSecret = System.Configuration.ConfigurationManager.AppSettings[ "FB.Secret" ];
			x.Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
			{
				OnAuthenticated = async context =>
				{
					context.Identity.AddClaim( new System.Security.Claims.Claim( "FacebookAccessToken", context.AccessToken ) );
					foreach ( var claim in context.User )
					{
						var claimType = string.Format( "urn:facebook:{0}", claim.Key );
						string claimValue = claim.Value.ToString();
						if ( !context.Identity.HasClaim( claimType, claimValue ) )
							context.Identity.AddClaim( new System.Security.Claims.Claim( claimType, claimValue, "XmlSchemaString", "Facebook" ) );

					}

				}
			};
			app.UseFacebookAuthentication( x );
			//app.UseFacebookAuthentication(
			//   appId: "524383337661333",
			//   appSecret: "3de5709f70ec81ba49f6724994238dde" );

			// QA
			//app.UseFacebookAuthentication(
			//  appId: "580376448728688",
			//  appSecret: "27d1b0324ae580f09d241d63b9e4f2ba" );

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}