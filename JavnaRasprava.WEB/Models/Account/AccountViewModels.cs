using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models
{
	public class ExternalLoginConfirmationViewModel
	{
		[Required( ErrorMessageResourceName="Account_DemographyAge_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[Display( Name = "Account_DemographyAge", ResourceType = typeof( GlobalLocalization ) )]
		public Age Age { get; set; }

		[Required( ErrorMessageResourceName="Account_DemographyEducation_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[Display( Name = "Account_DemographyEducation", ResourceType = typeof( GlobalLocalization ) )]
		public Education Education { get; set; }

		[Required( ErrorMessageResourceName= "Account_Email_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[EmailAddress]
		[Display( Name = "Account_Email", ResourceType = typeof( GlobalLocalization ) )]
		public string Email { get; set; }

		[Required( ErrorMessageResourceName="Account_DemographyLocation_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[Display( Name = "Account_DemographyLocation", ResourceType = typeof( GlobalLocalization ) )]
		public int LocationID { get; set; }

		public List<System.Web.Mvc.SelectListItem> Locations { get; set; }

		[Display( Name = "Account_DemographyParty", ResourceType = typeof( GlobalLocalization ) )]
		public int? PartyID { get; set; }

		public List<System.Web.Mvc.SelectListItem> Parties { get; set; }


		public string UserName { get; set; }
	}

	public class ExternalLoginListViewModel
	{
		public string Action { get; set; }
		public string ReturnUrl { get; set; }
	}

	public class ManageUserViewModel
	{
		[Required( ErrorMessageResourceName= "Account_OldPassword_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[DataType( DataType.Password )]
		[Display( Name = "Account_OldPassword", ResourceType = typeof( GlobalLocalization ) )]
		public string OldPassword { get; set; }

		[Required( ErrorMessageResourceName= "Account_NewPassword_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[StringLength( 100, ErrorMessageResourceType = typeof( GlobalLocalization ), ErrorMessageResourceName = "MinLengthError", MinimumLength = 6 )]
		[DataType( DataType.Password )]
		[Display( Name = "Account_NewPassword", ResourceType = typeof( GlobalLocalization ) )]
		public string NewPassword { get; set; }

		[DataType( DataType.Password )]
		[Compare( "NewPassword", ErrorMessageResourceType = typeof( GlobalLocalization ), ErrorMessageResourceName = "Account_ConfirmPassword_Error")]
		[Display( Name = "Account_ConfirmPassword", ResourceType = typeof( GlobalLocalization ) )]
		public string ConfirmPassword { get; set; }
	}

	public class LoginViewModel
	{
		[Required( ErrorMessageResourceName= "Account_Email_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[EmailAddress]
		[Display( Name = "Account_Email", ResourceType = typeof( GlobalLocalization ) )]
		public string Email { get; set; }

		[Required( ErrorMessageResourceName= "Account_Password_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[DataType( DataType.Password )]
		[Display( Name = "Account_Password", ResourceType = typeof( GlobalLocalization ) )]
		public string Password { get; set; }

		[Display( Name = "Account_RememberMe", ResourceType = typeof( GlobalLocalization ) )]
		public bool RememberMe { get; set; }
	}

	public class RegisterViewModel
	{
		[Required( ErrorMessageResourceName= "Account_Email_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[EmailAddress]
		[Display( Name = "Account_Email", ResourceType = typeof( GlobalLocalization ) )]
		public string Email { get; set; }

		[Required( ErrorMessageResourceName= "Account_Password_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[StringLength( 100, ErrorMessageResourceType = typeof( GlobalLocalization ), ErrorMessageResourceName = "MinLengthError", MinimumLength = 6 )]
		[DataType( DataType.Password )]
		[Display( Name = "Account_Password", ResourceType = typeof( GlobalLocalization ) )]
		public string Password { get; set; }

		[DataType( DataType.Password )]
		[Compare( "Password", ErrorMessageResourceType = typeof( GlobalLocalization ), ErrorMessageResourceName = "Account_ConfirmPassword_Error")]
		[Display( Name = "Account_ConfirmPassword", ResourceType = typeof( GlobalLocalization ) )]
		public string ConfirmPassword { get; set; }

		[Required( ErrorMessageResourceName="Account_DemographyAge_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[Display( Name = "Account_DemographyAge", ResourceType = typeof( GlobalLocalization ) )]
		public Age? Age { get; set; }

		[Required( ErrorMessageResourceName="Account_DemographyEducation_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[Display( Name = "Account_DemographyEducation", ResourceType = typeof( GlobalLocalization ) )]
		public Education? Education { get; set; }

		[Required( ErrorMessageResourceName="Account_DemographyEducation_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[Display( Name = "Account_DemographyLocation", ResourceType = typeof( GlobalLocalization ) )]
		public int LocationID { get; set; }

		[Display( Name = "Account_DemographyParty", ResourceType = typeof( GlobalLocalization ) )]
		public int? PartyID { get; set; }

		public List<System.Web.Mvc.SelectListItem> Locations { get; set; }
		public List<System.Web.Mvc.SelectListItem> Parties { get; set; }
	}

	public class ResetPasswordViewModel
	{
		[Required( ErrorMessageResourceName= "Account_Email_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[EmailAddress]
		[Display( Name = "Account_Email", ResourceType = typeof( GlobalLocalization ) )]
		public string Email { get; set; }

		[Required( ErrorMessageResourceName= "Account_Password_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[StringLength( 100, ErrorMessageResourceType = typeof( GlobalLocalization ), ErrorMessageResourceName = "MinLengthError", MinimumLength = 6 )]
		[DataType( DataType.Password )]
		[Display( Name = "Password", ResourceType = typeof( GlobalLocalization ) )]
		public string Password { get; set; }

		[DataType( DataType.Password )]
		[Compare( "Password", ErrorMessageResourceType = typeof( GlobalLocalization ), ErrorMessageResourceName = "Account_ConfirmPassword_Error")]
		[Display( Name = "Account_ConfirmPassword", ResourceType = typeof( GlobalLocalization ) )]
		public string ConfirmPassword { get; set; }

		public string Code { get; set; }
	}

	public class ForgotPasswordViewModel
	{
		[Required( ErrorMessageResourceName= "Account_Email_Required", ErrorMessageResourceType=typeof( GlobalLocalization ) )]
		[EmailAddress]
		[Display( Name = "Account_Email", ResourceType = typeof( GlobalLocalization ) )]
		public string Email { get; set; }
	}
}
