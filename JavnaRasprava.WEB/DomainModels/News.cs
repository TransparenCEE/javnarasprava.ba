using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class News
	{
		public int NewsId { get; set; }

		public string Title { get; set; }

		public string Summary { get; set; }

		public string Text { get; set; }

		public string ImageRelativePath { get; set; }

		public int ParliamentId { get; set; }

		public virtual Parliament Parliament { get; set; }

		public DateTime? CreateDateTimeUtc { get; internal set; }

		public News()
		{
			CreateDateTimeUtc = DateTime.UtcNow;
		}
	}
}