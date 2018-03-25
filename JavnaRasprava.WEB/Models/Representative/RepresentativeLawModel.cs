using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JavnaRasprava.WEB.Models.Representative;

namespace JavnaRasprava.WEB.Models
{
	public class RepresentativeLawModel
	{
		public int ID { get; set; }

		public string Title { get; set; }

		public int VotesUp { get; set; }

		public double VotesUpPercentage { get; set; }

		public int VotesDown { get; set; }

		public double VotesDownPercentage { get; set; }

		public List<RepresentativeQuestionModel> Questions { get; set; }

		public DateTime? LatestAnswerTime { get; internal set; }
	}
}
