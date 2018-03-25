using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Models.Law
{
	public class PostAnswerModel
	{
		public int? LawID { get; set; }

		public string LawTitle { get; set; }

		public int QuestionID { get; set; }

		public string QuestionText { get; set; }

		public string RepresentativeDisplayName { get; set; }

		[UIHint( "tinymce_jquery_min" )]
		[AllowHtml]
		public string Answer { get; set; }

		public int RepresentativeID { get; set; }

		public string RepresentativeImage { get; set; }

		public Guid AnswerToken { get; set; }
	}
}