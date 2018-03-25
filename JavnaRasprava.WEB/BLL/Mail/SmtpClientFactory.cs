using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Mail
{
    public class SmtpClientFactory
    {
        public static SmtpClient Create()
        {
            SmtpClient smtpClient = new SmtpClient();

            smtpClient.Host = ConfigurationManager.AppSettings[ "EMAIL.SMTP" ];
            smtpClient.Port = Convert.ToInt32( ConfigurationManager.AppSettings[ "EMAIL.PORT" ] );
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new System.Net.NetworkCredential( ConfigurationManager.AppSettings[ "EMAIL.USERNAME" ], ConfigurationManager.AppSettings[ "EMAIL.PASSWORD" ] );
            return smtpClient;

        }
    }
}