using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models.Representative
{
	public class RepresentativeSearchModel
	{
		public SortOrder SortOrder { get; set; }

		public string SearchName { get; set; }

		public List<PartyModel> Parties { get; set; }
		public int? SelectedParty { get; set; }

		public int ParliamentId { get; set; }
	}
}
