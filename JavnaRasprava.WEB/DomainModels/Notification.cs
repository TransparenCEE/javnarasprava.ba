using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Notification
	{
		public int NotificationID { get; set; }

		public bool Completed { get; set; }

		public string Message { get; set; }

		public DateTime TimeCreated { get; set; }

		public NotificationType Type { get; set; }

		public virtual ICollection<NotificationData> NotificationData { get; set; }
	}
}