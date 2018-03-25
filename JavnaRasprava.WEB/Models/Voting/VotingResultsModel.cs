using System.Collections.Generic;
using System.Linq;

namespace JavnaRasprava.WEB.Models
{
	public class VotingResultsModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

        public string ActionName { get; set; }

        public JavnaRasprava.WEB.Infrastructure.Age Age { get; set; }

		public JavnaRasprava.WEB.Infrastructure.Education Education { get; set; }

		public int LocationID { get; set; }
		public List<System.Web.Mvc.SelectListItem> Locations { get; set; }

		public int PartyID { get; set; }
		public List<System.Web.Mvc.SelectListItem> Parties { get; set; }

		public double VotesUpPercentage { get; set; }

		public int VotesUp { get; set; }

		public double VotesDownPercentage { get; set; }

		public int VotesDown { get; set; }

		public int ID { get; set; }

		public List<CustomVoteResultModel> CustomResultsDistribution { get; set; }

		public List<CustomVoteResultModel> PositiveCustomResultsDistribution
		{
			get
			{
				return CustomResultsDistribution
					.Where( x => x.Vote.HasValue && x.Vote.Value )
					.Select( x => x )
					.OrderBy( x => x.ID )
					.ToList();
			}
		}

		public List<CustomVoteResultModel> NegativeCustomResultsDistribution
		{
			get
			{
				return CustomResultsDistribution
					.Where( x => x.Vote.HasValue && !x.Vote.Value )
					.Select( x => x )
					.OrderBy( x => x.ID )
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