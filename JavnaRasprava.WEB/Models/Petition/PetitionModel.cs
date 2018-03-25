using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models
{
	public class PetitionModel
	{
		public int PetitionID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Petition_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Petition_Description", ResourceType = typeof( GlobalLocalization ) )]
		[UIHint( "tinymce_jquery_min" ), System.Web.Mvc.AllowHtml]
		public string Description { get; set; }

		[Display( Name = "Petition_TargetInsitution", ResourceType = typeof( GlobalLocalization ) )]
		public string TargetInstitution { get; set; } // This becomes read only for users now

		[Display( Name = "Petition_SubmitterName", ResourceType = typeof( GlobalLocalization ) )]
		public string SubmitterName { get; set; }

		public string SubmitterUserID { get; set; }

		[Display( Name = "Global_Verified", ResourceType = typeof( GlobalLocalization ) )]
		public bool Verified { get; set; }

		[Display( Name = "Global_AdminIgnore", ResourceType = typeof( GlobalLocalization ) )]
		public bool AdminIgnore { get; set; }

		[Display( Name = "Petition_Signatures", ResourceType = typeof( GlobalLocalization ) )]
		public int? Signatures { get; set; }

		public bool UserSigned { get; set; }

		public bool SigningEnabled { get; set; }

		public List<PetitionProgressModel> CompletedProgress { get; set; }

		public PetitionProgressModel NextProgress { get; set; }

		public bool HasNextProgress { get { return NextProgress != null; } }

		public List<TargetInstitutionModel> TargetInstitutionList { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Petition_TargetInsitution", ResourceType = typeof( GlobalLocalization ) )]
		public int SelectedTargetInstitution { get; set; }

		[Display( Name = "Petition_YoutubeUrl", ResourceType = typeof( GlobalLocalization ) )]
		public string YoutubeUrl { get; set; }

		[Display( Name = "Petition_YoutubeCode", ResourceType = typeof( GlobalLocalization ) )]
		public string YoutubeCode { get; set; }

		[Display( Name = "Petition_ImageRelativePath", ResourceType = typeof( GlobalLocalization ) )]
		public string ImageRelativePath { get; set; }
		public HttpPostedFileBase Image { get; set; }
	}
}