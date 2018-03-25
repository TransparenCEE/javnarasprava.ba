using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
	public class DirectMessageModel
	{
		public UserRepresentativeQuestion Question { get; set; }

		public string BaseUrl { get; set; }
	}
}