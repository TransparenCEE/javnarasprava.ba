using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.Petition
{
	public class PetitionProgressSummaryModel
	{
		public int PetitionProgressId { get; set; }

		[Display( Name = "Global_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }
	}
}