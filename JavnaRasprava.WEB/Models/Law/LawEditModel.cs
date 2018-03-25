using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Law
{
	public class LawEditModel
	{
		public int LawID { get; set; }

		[Display( Name = "Global_Text", ResourceType = typeof( GlobalLocalization ) )]
		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[UIHint( "tinymce_jquery_min" ), System.Web.Mvc.AllowHtml]
		public string Text { get; set; }

		[Display( Name = "Global_Title", ResourceType = typeof( GlobalLocalization ) )]
		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[System.Web.Mvc.AllowHtml]
		public string Title { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Global_Description", ResourceType = typeof( GlobalLocalization ) )]
		[UIHint( "tinymce_jquery_min" ), System.Web.Mvc.AllowHtml]
		public string Description { get; set; }

		[Display( Name = "Law_Submitter", ResourceType = typeof( GlobalLocalization ) )]
		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		public string Submitter { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[DisplayFormat( DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true )]
		public DateTime CreateDateTimeUtc { get; set; }

		[Display( Name = "Law_ExpetedVotingDay", ResourceType = typeof( GlobalLocalization ) )]
		[DisplayFormat( DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true )]
		public DateTime? ExpetedVotingDay { get; set; }

		public DateTime? PointedOutUtc { get; set; }

		public HttpPostedFileBase Image { get; set; }

		public string ImageRelativePath { get; set; }

		[Display( Name = "Law_TextFile", ResourceType = typeof( GlobalLocalization ) )]
		public HttpPostedFileBase TextFile { get; set; }

		public string TextFileRelativePath { get; set; }

		public int ParliamentID { get; set; }

		[Display( Name = "Global_Parliament", ResourceType = typeof( GlobalLocalization ) )]
		public string ParliamentName { get; set; }

		[Display( Name = "Law_StatusTitle", ResourceType = typeof( GlobalLocalization ) )]
		public string StatusTitle { get; set; }

		[Display( Name = "Law_StatusText", ResourceType = typeof( GlobalLocalization ) )]
		public string StatusText { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Law_Procedure", ResourceType = typeof( GlobalLocalization ) )]
		public int ProcedureID { get; set; }
		public string ProcedureName { get; set; }
		public List<ProcedureEditModel> Procedures { get; set; }

		public List<LawSectionSummaryEditModel> Sections { get; set; }

		public List<LawRepresentativeModel> Representatives { get; set; }

		public List<ExpertCommentEditModel> ExpertComments { get; set; }

		public List<CustomVoteEditModel> CustomVotes { get; set; }

		public List<PredefinedQuestionEditModel> Questions { get; set; }

		[Display( Name = "Law_IsActive", ResourceType = typeof( GlobalLocalization ) )]
		public bool IsActive { get; set; }

		public bool PointedOut { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "Law_Category", ResourceType = typeof( GlobalLocalization ) )]
		public int CategoryId { get; set; }
		public string CategoryTitle { get; set; }
		public List<LawCategoryEditModel> Categories { get; set; }
	}
}