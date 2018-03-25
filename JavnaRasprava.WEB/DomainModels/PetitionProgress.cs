using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class PetitionProgress
	{
		public int PetitionProgressID { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string ImageToDoRelativePath { get; set; }

		public string ImageDoneRelativePath { get; set; }

		public int? NumberOfVotes { get; set; }

		public int ParliamentID { get; set; }

		public virtual Parliament Parliament { get; set; }

		public int? RepresentativeID { get; set; }

		public Representative Representative { get; set; }



	}
}