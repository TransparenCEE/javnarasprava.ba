using System;
using System.Collections.Generic;

namespace JavnaRasprava.WEB.Models
{
	public class QuestionModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public int Id { get; set; }

		public string Text { get; set; }

		public DateTime TimeCreatedUtc { get; set; }

		public bool IsPredefined { get; set; }

		public int AskedCount { get; set; }

		public int AnswersCount { get; set; }

		public int LikesCount { get; set; }

		public int DislikesCount { get; set; }

		public bool? UserLiked { get; set; }

		public List<QustionRepresentativeModel> Representatives { get; set; }

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion
	}
}
