using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizResultsModel
	{
		public int QuizId { get; set; }

		public DateTime TimeCreated { get; set; }

		[Display( Name = "Quiz_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		[Display( Name = "Quiz_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Description { get; set; }

		public List<QuizItemResultsModel> Items { get; set; }
		public string ImageRelativePath { get; internal set; }
	}
}