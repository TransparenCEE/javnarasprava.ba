using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
	public class EmailSender : IDisposable
	{
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private bool _disposed = false;
        private SmtpClient _smtpClient;

        public EmailSender()
        {
            _smtpClient = SmtpClientFactory.Create();
        }


        public bool SendMessage( Message email )
        {
            try
            {
                var message = GetMailMessage( email );
                _smtpClient.Send( message );
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error( "Failed to send message to {0} with subject {1} = {2}", email.To, email.Subject, ex );
                return false;
            }
        }

        private void Send( MailMessage message )
        {
            _smtpClient.Send( message );
        }


        public static void SendEmails( List<Message> emails )
        {
            using ( var emailSender = new EmailSender() )
            {
                foreach ( var email in emails )
                {
                    MailMessage message = GetMailMessage( email );
                    emailSender.Send( message );
                }
            }
        }

        

        public static MailMessage GetMailMessage( Message email )
		{
            MailMessage message = new MailMessage()
            {
                From = GetFromEmailAddress(email)
            };
            message.To.Add( email.To );
			if (!string.IsNullOrWhiteSpace(email.Bcc))
				message.Bcc.Add( email.Bcc );
			message.Subject = email.Subject;
			message.IsBodyHtml = true;
			message.Body = email.Body;
			message.Priority = MailPriority.Normal;
			return message;

		}

		public static MailAddress GetFromEmailAddress( Message email )
		{
			if ( string.IsNullOrWhiteSpace( email.From ) )
			{
				return new MailAddress( ConfigurationManager.AppSettings["EMAIL.FROM"] );
			}
			else
			{
				return new MailAddress( email.From );
			}
		}

        ~EmailSender()
        {
            Dispose( false );
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( !_disposed )
            {
                if ( disposing )
                {
                    // Free other state (managed objects).
                }
                // Free your own state (unmanaged objects).
                // Set large fields to null.

                

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );

        }


    }
}