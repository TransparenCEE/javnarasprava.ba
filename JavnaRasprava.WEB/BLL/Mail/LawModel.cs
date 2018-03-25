using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
	public class LawModel
	{
		public int LawID { get; set; }

		public string Title { get; set; }

		public List<QuestionModel> Questions { get; set; }
	}
}