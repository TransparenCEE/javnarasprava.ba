using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.Models.Law
{
	public class LawSectionCustomVoteModel
	{
		public int LawSectionCustomVoteID { get; set; }

		[Display( Name = "Global_Text", ResourceType = typeof( GlobalLocalization ) )]
		public string Text { get; set; }

		[Display( Name = "Global_Description", ResourceType = typeof( GlobalLocalization ) )]
		public string Description { get; set; }

		[Display( Name = "Global_aVote", ResourceType = typeof( GlobalLocalization ) )]
		public bool? Vote { get; set; }

		[Display( Name = "Global_AdminIgnore", ResourceType = typeof( GlobalLocalization ) )]
		public bool AdminIgnore { get; set; }

		public int LawSectionID { get; set; }

		public int LawID { get; set; }

		[Display( Name = "Global_IsSuggested", ResourceType = typeof( GlobalLocalization ) )]
		public bool IsSuggested { get; set; }
	}
}
