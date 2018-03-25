
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Representative
	{
		public int RepresentativeID { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Resume { get; set; }

		public string Email { get; set; }

		public string ImageRelativePath { get; set; }

		public int NumberOfVotes { get; set; }

		public string EletorialUnit { get; set; }

		public string Function { get; set; }



		public virtual ICollection<ExternalLink> ExternalLinks { get; set; }

		public int PartyID { get; set; }
		public virtual Party Party { get; set; }

		public int ParliamentHouseID { get; set; }
		public virtual ParliamentHouse ParliamentHouse { get; set; }

		public virtual ICollection<UserRepresentativeQuestion> UserRepresentativeQuestions { get; set; }

		public virtual ICollection<RepresentativeAssignment> Assignments { get; set; }

		[NotMapped]
		public string DisplayName
		{
			get
			{
				return Infrastructure.Helpers.NameFormatter.GetDisplayName( FirstName, LastName );
			}
		}
	}
}