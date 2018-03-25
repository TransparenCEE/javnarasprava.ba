using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure.Helpers
{
	public class NameFormatter
	{
		public static string GetDisplayName( string firstName, string lastName )
		{
			return $"{firstName} {lastName}";
		}
	}
}