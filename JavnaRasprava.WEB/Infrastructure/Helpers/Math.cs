
namespace JavnaRasprava.WEB.Infrastructure
{
	public class Math
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public static double Percentage( int percentageOf, int inTotal )
		{
			if ( inTotal == 0 )
				return 0;

			var result = ( (double)percentageOf / inTotal ) * 100;

			return System.Math.Round( result, 0 );
		}

		#endregion
	}
}