using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class StatusCodeOverrideTelemetryInitializer : ITelemetryInitializer
	{
		private readonly List<int> _overrideStatusCode = new List<int> { 404 };

		void ITelemetryInitializer.Initialize( ITelemetry telemetry )
		{
			if ( telemetry is RequestTelemetry requestTelemetry )
			{
				bool parsed = int.TryParse( requestTelemetry.ResponseCode, out int code );
				if ( !parsed )
				{
					return;
				}

				if ( _overrideStatusCode.Contains( code ) )
				{
					requestTelemetry.Success = true;
					requestTelemetry.Context.Properties["OverriddenHttpStatusCode"] = "true";
				}
			}
		}
	}
}