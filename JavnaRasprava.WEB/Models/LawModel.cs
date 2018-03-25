using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JavnaRasprava.WEB.Models
{
	public class LawModel
	{
		#region == Properties ==

		public JavnaRasprava.WEB.DomainModels.Law Law { get; set; }

		public int VotesUp { get; set; }

		public int VotesDown { get; set; }

		public ICollection<LawSectionModel> Sections { get; set; }



		#endregion

	}
}
