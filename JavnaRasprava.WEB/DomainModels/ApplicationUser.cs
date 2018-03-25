using JavnaRasprava.WEB.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JavnaRasprava.WEB.DomainModels
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		#region == Properties ==

		public Age? Age { get; set; }

		public Education? Education { get; set; }

		public int? LocationID { get; set; }
		public virtual Location Location { get; set; }

		public int? PartyID { get; set; }

		public virtual Party Party { get; set; }

		public bool IsDisabled { get; set; }

		#endregion

		#region == Methods ==

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync( UserManager<ApplicationUser> manager )
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync( this, DefaultAuthenticationTypes.ApplicationCookie );
			// Add custom user claims here
			return userIdentity;
		}

		public ClaimsIdentity GenerateUserIdentity( UserManager<ApplicationUser> manager )
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = manager.CreateIdentity( this, DefaultAuthenticationTypes.ApplicationCookie );
			// Add custom user claims here
			return userIdentity;
		}

		#endregion
	}


}