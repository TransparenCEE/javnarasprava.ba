using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace JavnaRasprava.WEB.Helpers
{
	public static class ExtensionMethods
	{
		public static T AddOrGetExisting<T>( this ObjectCache cache,
									string key,
									Func<T> valueFactory,
									CacheItemPolicy policy )
		{
			var newValue = new Lazy<T>( valueFactory );
			var oldValue = cache.AddOrGetExisting( key, newValue, policy ) as Lazy<T>;

			try
			{
				return ( oldValue ?? newValue ).Value;
			}
			catch
			{
				cache.Remove( key );
				throw;
			}
		}
	}
}