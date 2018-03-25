using JavnaRasprava.WEB.BLL.Mail;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class EmailService : IIdentityMessageService
	{
		public Task SendAsync( IdentityMessage identityMessage )
		{
			Message messageObject = new Message
			{
				Body = identityMessage.Body,
				To = identityMessage.Destination,
				Subject = identityMessage.Subject
			};
			var message = EmailSender.GetMailMessage( messageObject );
			var client = SmtpClientFactory.Create();

			return Task.Run( () => client.Send( message ) );
		}
	}


}