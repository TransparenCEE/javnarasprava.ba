using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
	public class Message
	{
		public string To { get; set; }

		public string Bcc { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public string From { get; set; }
	}
}