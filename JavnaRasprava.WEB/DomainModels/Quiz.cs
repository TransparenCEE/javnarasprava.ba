using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Quiz
	{
		public int QuizId { get; set; }

		public DateTime TimeCreated { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public int ParliamentId { get; set; }
		public virtual Parliament Parliament { get; set; }

		public ICollection<QuizItem> Items { get; set; }
		public string ImageRelativePath { get; internal set; }

		public Quiz()
		{
			TimeCreated = DateTime.UtcNow;
		}
	}
}