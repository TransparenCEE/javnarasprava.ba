using System;

namespace JavnaRasprava.WEB.Models
{
	public class AnswerModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public int ID { get; set; }

		public string Text { get; set; }

		public DateTime TimeAnsweredUtc { get; set; }

		public QustionRepresentativeModel Representative { get; set; }

		public QuestionModel Question { get; set; }

		public int LikesCount { get; set; }

		public int DislikesCount { get; set; }

		public bool? UserLiked { get; set; }

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion

	}
}
