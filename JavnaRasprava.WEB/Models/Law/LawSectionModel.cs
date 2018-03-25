using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class LawSectionModel
	{
		#region == Properties == 

		public JavnaRasprava.WEB.DomainModels.LawSection LawSection { get; set; }

		public int VotesUp { get; set; }

		public int VotesDown { get; set; }

		public double VotesUpPercentage { get; set; }

		public double VotesDownPercentage { get; set; }

		public bool UserVoted { get; set; }

		#endregion

		public string ImageRelativePath { get; set; }
	}
}