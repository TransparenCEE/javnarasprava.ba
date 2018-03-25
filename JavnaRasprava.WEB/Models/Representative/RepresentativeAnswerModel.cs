using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models.Representative
{
	public class RepresentativeAnswerModel
	{
		public int ID { get; set; }

		public string Answer { get; set; }

		public int LikesCount { get; set; }

		public int DislikesCount { get; set; }

		public DateTime AnswerTime { get; set; }

		public bool? UserLiked { get; set; }
	}
}
