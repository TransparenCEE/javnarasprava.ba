using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Helpers
{
	public static class Html
	{
		/// <summary>
		/// Creates required field indicator label if property is decorated with Required attribute
		/// </summary>
		public static MvcHtmlString RequiredFieldFor<TModel, TValue>( this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression )
		{
			// Get the metadata for the model
			var metadata = ModelMetadata.FromLambdaExpression( expression, html.ViewData );

			// Check if the field is required
			var isRequired = metadata
						  .ContainerType.GetProperty( metadata.PropertyName )
						  .GetCustomAttributes( typeof( RequiredAttribute ), false )
						  .Length == 1;

			// If the field is required generate label with an asterix
			if ( isRequired )
				return html.RequiredField();

			return null;
		}

		/// <summary>
		/// Creates required field indicator label
		/// </summary>
		public static MvcHtmlString RequiredField( this HtmlHelper html )
		{

			var labelText = "*";

			var tag = new TagBuilder( "label" );
			tag.AddCssClass( "requiredIndicator" );
			tag.SetInnerText( labelText );

			return MvcHtmlString.Create( tag.ToString( TagRenderMode.Normal ) );
		}

		public static string Checked( this HtmlHelper html, bool condition )
		{
			return condition ? "checked" : "";
		}

		public static MvcHtmlString DescriptionFor<TModel, TValue>( this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression )
		{
			var metadata = ModelMetadata.FromLambdaExpression( expression, self.ViewData );
			var description = metadata.Description;

			return string.IsNullOrWhiteSpace( description ) ? MvcHtmlString.Empty : MvcHtmlString.Create( string.Format( @"<span>{0}</span>", description ) );
		}
	}
}