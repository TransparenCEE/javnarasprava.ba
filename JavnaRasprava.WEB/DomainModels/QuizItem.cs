using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class QuizItem
	{
		public int QuizItemId { get; set; }

		public int Order { get; set; }
		
		public QuizItemType Type { get; set; }

		public Law Law { get; set; }

		public LawSection LawSection { get; set; }

		public int QuizId { get; set; }
		public virtual Quiz Quiz { get; set; }
		
	}
}