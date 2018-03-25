using System;
using System.Configuration;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class AppConfig
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public static string GetStringValue( string key )
		{
			return ConfigurationManager.AppSettings[key];
		}

        public static int GetIntValue( string key, int defaultValue )
        {
            int result = 0;
            if ( !Int32.TryParse( ConfigurationManager.AppSettings[ key ], out result ) )
                return defaultValue;
            return result;
        }

		#endregion
	}
}