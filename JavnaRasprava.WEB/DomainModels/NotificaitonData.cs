using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class NotificationData
	{
		public int NotificationDataID { get; set; }

		public string Key { get; set; }

		public string Value { get; set; }

		public virtual Notification Notification { get; set; }
		public int NotificationID { get; set; }

	}
}