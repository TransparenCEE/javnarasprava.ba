using Microsoft.ApplicationInsights;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class CustomHandleErrorAttribute : HandleErrorAttribute
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public override void OnException( ExceptionContext filterContext )
		{
			if ( filterContext != null && filterContext.HttpContext != null && filterContext.Exception != null )
			{
				// If customError is Off, then AI HTTPModule will report the exception
				// If it is On, or RemoteOnly (default) - then we need to explicitly track the exception
				if ( filterContext.HttpContext.IsCustomErrorEnabled )
				{
					var ai = new TelemetryClient();
					ai.TrackException( filterContext.Exception );
				}

				// For now I want to keep all exceptions in log file
				logger.Error( filterContext.Exception.ToString() );
			}
			base.OnException( filterContext );
		}
	}
}