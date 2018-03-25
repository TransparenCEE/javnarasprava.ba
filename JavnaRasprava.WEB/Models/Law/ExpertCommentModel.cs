using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models.Law
{
	public class ExpertCommentEditModel
	{
		public int ExpertCommentID { get; set; }

		public string Text { get; set; }

		public int LawID { get; set; }

		public int ExpertID { get; set; }
		public List<JavnaRasprava.WEB.DomainModels.Expert> Experts { get; set; }

		public string ExpertFirstName { get; set; }

		public string ExpertLastName { get; set; }

		public string ExpertAbout { get; set; }
	}
}
