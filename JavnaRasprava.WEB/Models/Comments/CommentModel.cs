using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class CommentModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public LawComment Comment { get; set; }

		public int VotesUp { get; set; }

		public int VotesDown { get; set; }

		public double VotesUpPercentage { get; set; }

		public double VotesDownPercentage { get; set; }

		public bool UserVoted { get; set; }

		public List<CommentModel> SubComments { get; set; }

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion
	}
}