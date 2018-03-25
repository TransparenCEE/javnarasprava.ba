using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure.Helpers;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Controllers
{
	[Authorize]
	public class AccountController : BaseController
	{
		private ApplicationUserManager _userManager;

		public AccountController()
		{
		}

		public AccountController( ApplicationUserManager userManager )
		{
			UserManager = userManager;
		}

		public ApplicationUserManager UserManager
		{
			get
			{
				return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}
			private set
			{
				_userManager = value;
			}
		}

		//
		// GET: /Account/Login
		[AllowAnonymous]
		public ActionResult Login( string returnUrl )
		{
			// Just to add something to session.
			SessionManager.Current.ToString();
			ViewBag.ReturnUrl = returnUrl;
			return View();
		}

		//
		// POST: /Account/Login
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Login( LoginViewModel model, string returnUrl )
		{
			if ( ModelState.IsValid )
			{
				var user = UserManager.Find( model.Email, model.Password );
				if ( user != null )
				{
					if ( !UserManager.IsEmailConfirmed( user.Id ) )
					{
						SendEmailConfirmationToken( user.Id, GlobalLocalization.Account_ConfirmEmailRepeated );

						ViewBag.errorMessage = GlobalLocalization.Account_PleaseConfirmEmailMessage;
						return View( "Error" );
					}

					if ( user.IsDisabled )
					{
						ViewBag.errorMessage = GlobalLocalization.Account_DisabledMessage;
						return View( "Error" );
					}

					SignIn( user, model.RememberMe );
					return RedirectToLocal( returnUrl );
				}
				else
				{
					ModelState.AddModelError( "", GlobalLocalization.InvalidUsernameOrPasswordError );
				}
			}

			// If we got this far, something failed, redisplay form
			return View( model );
		}

		//
		// GET: /Account/Register
		[AllowAnonymous]
		public ActionResult Register()
		{
			RegisterViewModel model = new RegisterViewModel();
			UserService service = new UserService();
			model.Locations = DemographyHelper.ConvertLocationsToDropdownList( service.GetAllLocations() );
			model.Parties = DemographyHelper.ConvertPartiesToDropdownList( service.GetAllParties() );

			return View( model );
		}

		//
		// POST: /Account/Register
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult Register( RegisterViewModel model )
		{
			if ( ModelState.IsValid )
			{
				var user = new ApplicationUser()
				{
					Age = model.Age,
					Email = model.Email,
					Education = model.Education,
					LocationID = model.LocationID,
					PartyID = model.PartyID == 0 ? null : model.PartyID,
					UserName = model.Email
				};

				ViewBag.Message = GlobalLocalization.Account_ConfirmEmail_EmailSent_Message;


				IdentityResult result = UserManager.Create( user, model.Password );
				if ( result.Succeeded )
				{
					//await SignInAsync( user, isPersistent: false );


					SendEmailConfirmationToken( user.Id, GlobalLocalization.Account_ConfirmEmail_EmailSubject );


					return View( "Info" );
					//return RedirectToAction( "Index", "Home" );
				}
				else
				{
					UserService service = new UserService();
					model.Locations = DemographyHelper.ConvertLocationsToDropdownList( service.GetAllLocations() );
					model.Parties = DemographyHelper.ConvertPartiesToDropdownList( service.GetAllParties() );

					AddErrors( result );
				}
			}

			// If we got this far, something failed, redisplay form
			return View( model );
		}

		private string SendEmailConfirmationToken( string userID, string subject )
		{
			string body = GlobalLocalization.Account_ConfirmEmail_EmailBody;

			string code = UserManager.GenerateEmailConfirmationToken( userID );
			var callbackUrl = Url.Action( "ConfirmEmail", "Account", new { userId = userID, code = code }, protocol: Request.Url.Scheme );
			UserManager.SendEmail( userID, subject, String.Format( body, callbackUrl ) );


			return callbackUrl;
		}

		//
		// GET: /Account/ConfirmEmail
		[AllowAnonymous]
		public async Task<ActionResult> ConfirmEmail( string userId, string code )
		{
			if ( userId == null || code == null )
			{
				return View( "Error" );
			}

			IdentityResult result = await UserManager.ConfirmEmailAsync( userId, code );
			if ( result.Succeeded )
			{
				return View( "ConfirmEmail" );
			}
			else
			{
				AddErrors( result );
				return View();
			}
		}

		//
		// GET: /Account/ForgotPassword
		[AllowAnonymous]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		//
		// POST: /Account/ForgotPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ForgotPassword( ForgotPasswordViewModel model )
		{
			if ( ModelState.IsValid )
			{
				var user = UserManager.FindByName( model.Email );
				if ( user == null || !( UserManager.IsEmailConfirmed( user.Id ) ) )
				{
					ModelState.AddModelError( "", GlobalLocalization.UserDoesNotExistOrNotConfirmedError );
					return View();
				}


				// Send an email with this link
				string code = UserManager.GeneratePasswordResetToken( user.Id );
				var callbackUrl = Url.Action( "ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme );
				UserManager.SendEmail( user.Id, GlobalLocalization.Account_ChangePassword_EmailSubject, string.Format( GlobalLocalization.Account_ChangePassword_EmailBody, callbackUrl ) );
				return RedirectToAction( "ForgotPasswordConfirmation", "Account" );
			}

			// If we got this far, something failed, redisplay form
			return View( model );
		}

		//
		// GET: /Account/ForgotPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ForgotPasswordConfirmation()
		{
			return View();
		}

		//
		// GET: /Account/ResetPassword
		[AllowAnonymous]
		public ActionResult ResetPassword( string code )
		{
			if ( code == null )
			{
				return View( "Error" );
			}
			return View();
		}

		//
		// POST: /Account/ResetPassword
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ResetPassword( ResetPasswordViewModel model )
		{
			if ( ModelState.IsValid )
			{
				var user = UserManager.FindByName( model.Email );
				if ( user == null )
				{
					ModelState.AddModelError( "", GlobalLocalization.UserNotFoundError );
					return View();
				}
				IdentityResult result = UserManager.ResetPassword( user.Id, model.Code, model.Password );
				if ( result.Succeeded )
				{
					return RedirectToAction( "ResetPasswordConfirmation", "Account" );
				}
				else
				{
					AddErrors( result );
					return View();
				}
			}

			// If we got this far, something failed, redisplay form
			return View( model );
		}

		//
		// GET: /Account/ResetPasswordConfirmation
		[AllowAnonymous]
		public ActionResult ResetPasswordConfirmation()
		{
			return View();
		}

		//
		// POST: /Account/Disassociate
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Disassociate( string loginProvider, string providerKey )
		{
			ManageMessageId? message = null;
			IdentityResult result = UserManager.RemoveLogin( User.Identity.GetUserId(), new UserLoginInfo( loginProvider, providerKey ) );
			if ( result.Succeeded )
			{
				var user = UserManager.FindById( User.Identity.GetUserId() );
				SignIn( user, isPersistent: false );
				message = ManageMessageId.RemoveLoginSuccess;
			}
			else
			{
				message = ManageMessageId.Error;
			}
			return RedirectToAction( "Manage", new { Message = message } );
		}

		//
		// GET: /Account/Manage
		public ActionResult Manage( ManageMessageId? message )
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? GlobalLocalization.ChangePasswordSuccess
				: message == ManageMessageId.SetPasswordSuccess ? GlobalLocalization.SetPasswordSuccess
				: message == ManageMessageId.RemoveLoginSuccess ? GlobalLocalization.RemoveLoginSuccess
				: message == ManageMessageId.Error ? GlobalLocalization.ErrorOcurred
				: "";
			ViewBag.HasLocalPassword = HasPassword();
			ViewBag.ReturnUrl = Url.Action( "Manage" );
			return View();
		}

		//
		// POST: /Account/Manage
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Manage( ManageUserViewModel model )
		{
			bool hasPassword = HasPassword();
			ViewBag.HasLocalPassword = hasPassword;
			ViewBag.ReturnUrl = Url.Action( "Manage" );
			if ( hasPassword )
			{
				if ( ModelState.IsValid )
				{
					IdentityResult result = UserManager.ChangePassword( User.Identity.GetUserId(), model.OldPassword, model.NewPassword );
					if ( result.Succeeded )
					{
						var user = UserManager.FindById( User.Identity.GetUserId() );
						SignIn( user, isPersistent: false );
						return RedirectToAction( "Manage", new { Message = ManageMessageId.ChangePasswordSuccess } );
					}
					else
					{
						AddErrors( result );
					}
				}
			}
			else
			{
				// User does not have a password so remove any validation errors caused by a missing OldPassword field
				ModelState state = ModelState["OldPassword"];
				if ( state != null )
				{
					state.Errors.Clear();
				}

				if ( ModelState.IsValid )
				{
					IdentityResult result = UserManager.AddPassword( User.Identity.GetUserId(), model.NewPassword );
					if ( result.Succeeded )
					{
						return RedirectToAction( "Manage", new { Message = ManageMessageId.SetPasswordSuccess } );
					}
					else
					{
						AddErrors( result );
					}
				}
			}

			// If we got this far, something failed, redisplay form
			return View( model );
		}

		//
		// POST: /Account/ExternalLogin
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLogin( string provider, string returnUrl )
		{
			// Request a redirect to the external login provider
			return new ChallengeResult( provider, Url.Action( "ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl } ) );
		}

		//
		// GET: /Account/ExternalLoginCallback
		[AllowAnonymous]
		public ActionResult ExternalLoginCallback( string returnUrl )
		{
			var loginInfo = AuthenticationManager.GetExternalLoginInfo();
			if ( loginInfo == null )
			{
				return RedirectToAction( "Login" );
			}

			// Sign in the user with this external login provider if the user already has a login
			var user = UserManager.Find( loginInfo.Login );
			if ( user != null )
			{
				if ( user.IsDisabled )
				{
					ViewBag.errorMessage = GlobalLocalization.Account_DisabledMessage;
					return View( "Error" );
				}

				SignIn( user, isPersistent: false );
				return RedirectToLocal( returnUrl );
			}
			else
			{
				// If the user does not have an account, then prompt the user to create an account
				ViewBag.ReturnUrl = returnUrl;
				ViewBag.LoginProvider = loginInfo.Login.LoginProvider;

				UserService service = new UserService();

				ExternalLoginConfirmationViewModel model = new ExternalLoginConfirmationViewModel()
				{
					Email = loginInfo.Email,
					Locations = DemographyHelper.ConvertLocationsToDropdownList( service.GetAllLocations() ),
					Parties = DemographyHelper.ConvertPartiesToDropdownList( service.GetAllParties() )
				};
				var nameClaim = loginInfo.ExternalIdentity.Claims.First( c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name" );
				model.UserName = nameClaim.Value;


				return View( "ExternalLoginConfirmation", model );
			}
		}

		//
		// POST: /Account/LinkLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin( string provider )
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new ChallengeResult( provider, Url.Action( "LinkLoginCallback", "Account" ), User.Identity.GetUserId() );
		}

		//
		// GET: /Account/LinkLoginCallback
		public ActionResult LinkLoginCallback()
		{
			var loginInfo = AuthenticationManager.GetExternalLoginInfo( XsrfKey, User.Identity.GetUserId() );
			if ( loginInfo == null )
			{
				return RedirectToAction( "Manage", new { Message = ManageMessageId.Error } );
			}
			IdentityResult result = UserManager.AddLogin( User.Identity.GetUserId(), loginInfo.Login );
			if ( result.Succeeded )
			{
				return RedirectToAction( "Manage" );
			}
			return RedirectToAction( "Manage", new { Message = ManageMessageId.Error } );
		}

		//
		// POST: /Account/ExternalLoginConfirmation
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public ActionResult ExternalLoginConfirmation( ExternalLoginConfirmationViewModel model, string returnUrl )
		{
			if ( User.Identity.IsAuthenticated )
			{
				return RedirectToAction( "Manage" );
			}

			if ( ModelState.IsValid )
			{
				// Get the information about the user from the external login provider
				var info = AuthenticationManager.GetExternalLoginInfo();
				if ( info == null )
				{
					return View( "ExternalLoginFailure" );
				}
				var user = new ApplicationUser()
				{
					Age = model.Age,
					Email = model.Email,
					Education = model.Education,
					LocationID = model.LocationID,
					PartyID = model.PartyID == 0 ? null : model.PartyID,
					UserName = model.UserName
				};
				IdentityResult result = UserManager.Create( user );
				if ( result.Succeeded )
				{
					result = UserManager.AddLogin( user.Id, info.Login );
					if ( result.Succeeded )
					{
						SignIn( user, isPersistent: false );

						// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
						// Send an email with this link
						// string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
						// var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
						// SendEmail(user.Email, callbackUrl, "Confirm your account", "Please confirm your account by clicking this link");

						return RedirectToLocal( returnUrl );
					}
				}
				AddErrors( result );
			}

			UserService service = new UserService();
			model.Locations = DemographyHelper.ConvertLocationsToDropdownList( service.GetAllLocations() );
			model.Parties = DemographyHelper.ConvertPartiesToDropdownList( service.GetAllParties() );

			ViewBag.ReturnUrl = returnUrl;
			return View( model );
		}

		//
		// POST: /Account/LogOff
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LogOff()
		{
			AuthenticationManager.SignOut( DefaultAuthenticationTypes.ApplicationCookie );
			return RedirectToAction( "Index", "Home" );
		}

		//
		// GET: /Account/ExternalLoginFailure
		[AllowAnonymous]
		public ActionResult ExternalLoginFailure()
		{
			return View();
		}

		[ChildActionOnly]
		public ActionResult RemoveAccountList()
		{
			var linkedAccounts = UserManager.GetLogins( User.Identity.GetUserId() );
			ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
			return (ActionResult)PartialView( "_RemoveAccountPartial", linkedAccounts );
		}

		protected override void Dispose( bool disposing )
		{
			if ( disposing && UserManager != null )
			{
				UserManager.Dispose();
				UserManager = null;
			}
			base.Dispose( disposing );
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private IAuthenticationManager AuthenticationManager
		{
			get
			{
				return HttpContext.GetOwinContext().Authentication;
			}
		}

		private void SignIn( ApplicationUser user, bool isPersistent )
		{
			AuthenticationManager.SignOut( DefaultAuthenticationTypes.ExternalCookie );
			AuthenticationManager.SignIn( new AuthenticationProperties() { IsPersistent = isPersistent }, user.GenerateUserIdentity( UserManager ) );
		}

		private void AddErrors( IdentityResult result )
		{
			foreach ( var error in result.Errors )
			{
				ModelState.AddModelError( "", error );
			}
		}

		private bool HasPassword()
		{
			var user = UserManager.FindById( User.Identity.GetUserId() );
			if ( user != null )
			{
				return user.PasswordHash != null;
			}
			return false;
		}

		private void SendEmail( string email, string callbackUrl, string subject, string message )
		{
			// For information on sending mail, please visit http://go.microsoft.com/fwlink/?LinkID=320771
		}

		public enum ManageMessageId
		{
			ChangePasswordSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			Error
		}

		private ActionResult RedirectToLocal( string returnUrl )
		{
			if ( Url.IsLocalUrl( returnUrl ) )
			{
				return Redirect( returnUrl );
			}
			else
			{
				return RedirectToAction( "Index", "Home" );
			}
		}

		private class ChallengeResult : HttpUnauthorizedResult
		{
			public ChallengeResult( string provider, string redirectUri )
				: this( provider, redirectUri, null )
			{
			}

			public ChallengeResult( string provider, string redirectUri, string userId )
			{
				LoginProvider = provider;
				RedirectUri = redirectUri;
				UserId = userId;
			}

			public string LoginProvider { get; set; }
			public string RedirectUri { get; set; }
			public string UserId { get; set; }

			public override void ExecuteResult( ControllerContext context )
			{
				var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
				if ( UserId != null )
				{
					properties.Dictionary[XsrfKey] = UserId;
				}
				context.HttpContext.GetOwinContext().Authentication.Challenge( properties, LoginProvider );
			}
		}
		#endregion
	}
}