using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Petition
{
	public class PetitionProgressEditModel
	{
		public int PetitionProgresID { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "PetitionProgress_Title", ResourceType = typeof( GlobalLocalization ) )]
		[System.Web.Mvc.AllowHtml]
		public string Title { get; set; }

		[Required( ErrorMessageResourceName = "Global_RequiredMessage", ErrorMessageResourceType = typeof( GlobalLocalization ) )]
		[Display( Name = "PetitionProgress_Description", ResourceType = typeof( GlobalLocalization ) )]
		[UIHint( "tinymce_jquery_min" ), System.Web.Mvc.AllowHtml]
		public string Description { get; set; }

		[Display( Name = "PetitionProgress_ImageToDo", ResourceType = typeof( GlobalLocalization ) )]
		public HttpPostedFileBase ImageToDo { get; set; }
		[Display( Name = "PetitionProgress_ImageToDo", ResourceType = typeof( GlobalLocalization ) )]
		public string ImageToDoRelativePath { get; set; }

		[Display( Name = "Global_NumberOfVotes", ResourceType = typeof( GlobalLocalization ) )]
		public int? NumberOfVotes { get; set; }

		[Display( Name = "PetitionProgress_ImageDone", ResourceType = typeof( GlobalLocalization ) )]
		public HttpPostedFileBase ImageDone { get; set; }
		[Display( Name = "PetitionProgress_ImageDone", ResourceType = typeof( GlobalLocalization ) )]
		public string ImageDoneRelativePath { get; set; }

		public int ParliamentID { get; set; }
		[Display( Name = "PetitionProgress_ParliamentName", ResourceType = typeof( GlobalLocalization ) )]
		public string ParliamentName { get; set; }

		[Display( Name = "PetitionProgress_Representative", ResourceType = typeof( GlobalLocalization ) )]
		public int? SelectedRepresentativeID { get; set; }
		public List<RepresentativeModel> Representatives { get; set; }

		[Display( Name = "PetitionProgress_Representative", ResourceType = typeof( GlobalLocalization ) )]
		public string SelectedRepresentativeName { get; set; }
	}
}