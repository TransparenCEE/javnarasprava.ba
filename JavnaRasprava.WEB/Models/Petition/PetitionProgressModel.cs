using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class PetitionProgressModel
	{
		public int PetitionProgresID { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string ImageRelativePath { get; set; }

		public string RepresentativeImageRelativePath { get; set; }

		public int? VotesCount { get; set; }

	}
}