using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models
{
	public class RepresentativeEditModel
	{
		public int RepresentativeID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_FirstName", ResourceType = typeof( GlobalLocalization ) )]
		public string FirstName { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_LastName", ResourceType = typeof( GlobalLocalization ) )]
		public string LastName { get; set; }

		[UIHint( "tinymce_jquery_min" ), AllowHtml]
		[Required( ErrorMessageResourceName = "Global_Resume", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		public string Resume { get; set; }

		//[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		//[Display( Name = "GlobalImageRelativePath", ResourceType = typeof( GlobalLocalization ) )]
		public string ImageRelativePath { get; set; }

		public HttpPostedFileBase Image { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Email", ResourceType = typeof( GlobalLocalization ) )]
		[EmailAddress]
		public string Email { get; set; }

		public int ParliamentHouseID { get; set; }
		public string ParliamentHouseName { get; set; }

		public string ParliamentName { get; set; }

		[Display( Name = "Representative_NumberOfVotes", ResourceType = typeof( GlobalLocalization ) )]
		public int NumberOfVotes { get; set; }

		[Display( Name = "Representative_ElectoralUnit", ResourceType = typeof( GlobalLocalization ) )]
		public string EletorialUnit { get; set; }

		[Display( Name = "Representative_Function", ResourceType = typeof( GlobalLocalization ) )]
		public string Function { get; set; }

		public List<RepresentativeExternalLinkModel> ExternalLinks { get; set; }

		public List<Representative.RepresentativeAssignmentModel> Assignments { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Party", ResourceType = typeof( GlobalLocalization ) )]
		public int SelectedPartyID { get; set; }
		public string PartyName { get; set; }
		public List<PartyModel> Parties { get; set; }

		public List<Representative.RepresentativeQuestionModel> UserRepresentativeQuestions { get; set; }
	}
}