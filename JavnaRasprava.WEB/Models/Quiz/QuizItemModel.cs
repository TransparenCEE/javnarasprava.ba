using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizItemModel
	{
		public int QuizItemId { get; set; }

		public int? LawId { get; set; }

		public int? LawSectionId { get; set; }
	}
}