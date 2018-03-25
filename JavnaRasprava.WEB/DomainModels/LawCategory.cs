using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class LawCategory
	{
		public int LawCategoryId { get; set; }

		public string Title { get; set; }

		public virtual ICollection<Law> Laws { get; set; }
	}
}