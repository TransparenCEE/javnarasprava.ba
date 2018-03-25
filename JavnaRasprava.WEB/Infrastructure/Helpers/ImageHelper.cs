using System;
using System.IO;

namespace JavnaRasprava.WEB.Infrastructure
{
	public class ImageHelper
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public static string GetRepresentativeImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "RepresentativeImage.RootPath" ), relativePath );
		}

		public static string GetLawImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "LawImage.RootPath" ), relativePath );
		}

		public static string GetLawSectionImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "LawSectionImage.RootPath" ), relativePath );
		}

		public static string GetLawDocument( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "LawText.RootPath" ), relativePath );
		}

		public static string GetPetitionImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "PetitionImage.RootPath" ), relativePath );
		}

		public static string GetPetitionProgressImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "PetitionProgressImage.RootPath" ), relativePath );
		}

		public static string GetQuizImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "QuizImage.RootPath" ), relativePath );
		}

		public static string GetNewsImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return Path.Combine( AppConfig.GetStringValue( "NewsImage.RootPath" ), relativePath );
		}

		#endregion

		public static string GetImage( string relativePath )
		{
			if ( String.IsNullOrWhiteSpace( relativePath ) )
				return String.Empty;

			return AppConfig.GetStringValue( "BaseAddress" ) + Path.Combine( "/Content/Images/", relativePath );
		}


	}
}