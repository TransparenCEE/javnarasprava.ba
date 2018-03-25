using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute( typeof( JavnaRasprava.WEB.Startup ) )]
namespace JavnaRasprava.WEB
{
	public partial class Startup
	{
		public void Configuration( IAppBuilder app )
		{
			ConfigureAuth( app );
		}
	}
}
