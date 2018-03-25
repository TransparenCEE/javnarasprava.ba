using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class LawCustomVoteListModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

        public int LawID { get; set; }
		public List<DomainModels.LawCustomVote> LawCustomVotes { get; set; }

		public List<DomainModels.LawCustomVote> PositiveLawSectionCustomVotes
		{
			get
			{
				return LawCustomVotes
					.Where( x => x.Vote.HasValue && x.Vote.Value )
					.Select( x => x )
					.OrderBy( x => x.LawCustomVoteID )
					.ToList();
			}
		}

		public List<DomainModels.LawCustomVote> NegativeLawSectionCustomVotes
		{
			get
			{
				return LawCustomVotes
					.Where( x => x.Vote.HasValue && !x.Vote.Value )
					.Select( x => x )
					.OrderBy( x => x.LawCustomVoteID )
					.ToList();
			}
		}

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion
	}
}