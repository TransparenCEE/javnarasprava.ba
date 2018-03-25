using System.Collections.Generic;
using System.Linq;

namespace JavnaRasprava.WEB.Models
{
	public class LawSectionCustomVoteListModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public int LawID { get; set; }
		public int LawSectionID { get; set; }

		public List<DomainModels.LawSectionCustomVote> LawSectionCustomVotes { get; set; }

		public List<DomainModels.LawSectionCustomVote> PositiveLawSectionCustomVotes
		{
			get
			{
				return LawSectionCustomVotes
					.Where( x => x.Vote.HasValue && x.Vote.Value )
					.Select( x => x )
					.OrderBy( x => x.LawSectionCustomVoteID )
					.ToList();
			}
		}

		public List<DomainModels.LawSectionCustomVote> NegativeLawSectionCustomVotes
		{
			get
			{
				return LawSectionCustomVotes
					.Where( x => x.Vote.HasValue && !x.Vote.Value )
					.Select( x => x )
					.OrderBy( x => x.LawSectionCustomVoteID )
					.ToList();
			}
		}

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion


		public string Description { get; set; }

		public string Text { get; set; }

		public string Title { get; set; }
	}
}