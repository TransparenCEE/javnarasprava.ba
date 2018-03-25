using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models
{
	public class PetitionSummaryModel
	{
		public int Id { get; set; }

		[Display( Name = "Petition_Title", ResourceType = typeof( GlobalLocalization ) )]
		public string Title { get; set; }

		[Display( Name = "Petition_StartTime", ResourceType = typeof( GlobalLocalization ) )]
		public DateTime? StartTime { get; set; }

		[Display( Name = "Petition_EndTime", ResourceType = typeof( GlobalLocalization ) )]
		public DateTime? EndTime { get; set; }

		[Display( Name = "Petition_TargetInsitution", ResourceType = typeof( GlobalLocalization ) )]
		public string TargetInstitutionName { get; set; }

		[Display( Name = "Petition_CurrentCount", ResourceType = typeof( GlobalLocalization ) )]
		public int CurrentCount { get; set; }

		[Display( Name = "Global_Verified", ResourceType = typeof( GlobalLocalization ) )]
		public bool Verified { get; set; }
	}
}