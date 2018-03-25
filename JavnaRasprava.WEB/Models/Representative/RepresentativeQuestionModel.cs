using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models.Representative
{
	public class RepresentativeQuestionModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public DateTime AskedTimeUtc { get; set; }

		public int AskedCount { get; set; }
				
		public int LikesCount { get; set; }

		public int DislikesCount { get; set; }

		public RepresentativeAnswerModel Answer { get; set; }

		public bool? UserLiked { get; set; }

		public DateTime? AnswerTime { get; internal set; }
	}
}
