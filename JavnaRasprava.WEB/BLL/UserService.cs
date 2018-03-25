using EntityFramework.Extensions;
using JavnaRasprava.WEB.Models.User;
using PagedList;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;

namespace JavnaRasprava.WEB.BLL
{
	public class UserService
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		public void DeleteUser( JavnaRasprava.WEB.DomainModels.ApplicationUser user )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				context.Users.Attach( user );

				DeleteUserInternal( user, context );

				context.SaveChanges();
			}
		}

		public void DeleteUser( string id )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var user = context.Users.Where( x => x.Id == id ).FirstOrDefault();

				if ( user == null )
					return;

				DeleteUserInternal( user, context );

				context.SaveChanges();
			}
		}

		private static void DeleteUserInternal( JavnaRasprava.WEB.DomainModels.ApplicationUser user, DomainModels.ApplicationDbContext context )
		{
			context.LawComments.Where( x => x.ApplicationUserID == user.Id ).Delete();
			context.LawCommentLikes.Where( x => x.ApplicationUserID == user.Id ).Delete();
			context.LawVotes.Where( x => x.ApplicationUserID == user.Id ).Delete();
			context.LawSectionVotes.Where( x => x.ApplicationUserID == user.Id ).Delete();
			context.PetitionSignatures.Where( x => x.ApplicationUserID == user.Id ).Delete();
			context.Petitions.Where( x => x.SubmitterUserID == user.Id ).Delete();

			context.Users.Remove( user );
		}

		public JavnaRasprava.WEB.DomainModels.ApplicationUser GetUser( string id )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.Users.Where( x => x.Id == id ).FirstOrDefault();
			}
		}

		public List<JavnaRasprava.WEB.DomainModels.Location> GetAllLocations()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.Locations
					.Select( x => x )
					.OrderBy( x => x.Name )
					.Include( x => x.Region )
					.ToList();
			}
		}

		public List<SelectListItem> GetAllLocationsSelectList()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return GetAllLocationsSelectListInternal( context );
			}
		}

		internal static List<SelectListItem> GetAllLocationsSelectListInternal( DomainModels.ApplicationDbContext context )
		{
			return context.Locations
							.Select( x => new SelectListItem
							{
								Text = x.Name,
								Value = x.LocationID.ToString()
							} )
							.ToList();
		}

		public List<JavnaRasprava.WEB.DomainModels.Party> GetAllParties()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.Parties
					.Select( x => x )
					.OrderBy( x => x.Name )
					.ToList();
			}
		}

		public List<SelectListItem> GetAllPartiesSelectList()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return GetAllPartiesSelectListInternal( context );
			}
		}

		internal static List<SelectListItem> GetAllPartiesSelectListInternal( DomainModels.ApplicationDbContext context )
		{
			return context.Parties
							.Select( party => new SelectListItem
							{
								Text = party.Name,
								Value = party.PartyID.ToString()
							} )
							.ToList();
		}

		public UserSearchModel InitializeSearchModel( UserSearchModel model = null )
		{
			if ( model == null )
				return new UserSearchModel { };

			return model;
		}

		public IPagedList<UserModel> GetSearchUsers( UserSearchModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{

				var adminRole = context.Roles.Single( x => x.Name == "Admin" );

				var query = context.Users.AsQueryable();

				if ( model.QueryString != null )
					query = query.Where( x => x.UserName.Contains( model.QueryString ) );

				var result = query
					.OrderBy( x => x.UserName )
					.Select( x => new UserModel
					{
						Id = x.Id,
						UserName = x.UserName,
						Email = x.Email,
						EmailConfirmed = x.EmailConfirmed,
						AccessFailedCount = x.AccessFailedCount,
						LockoutEnabled = x.LockoutEnabled,
						LockoutEndDateUtc = x.LockoutEndDateUtc,
						Age = x.Age,
						Education = x.Education,
						Party = x.Party == null ? "N/A" : x.Party.Name,
						Location = x.Location == null ? "N/A" : x.Location.Name,
						IsAdmin = x.Roles.Any( r => r.RoleId == adminRole.Id ),
						IsDisabled = x.IsDisabled
					} )
					.ToPagedList( model.Page.HasValue ? model.Page.Value : 1, model.PageItemCount );

				return result;
			}
		}

		internal UserModel Get( string id )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{

				var adminRole = context.Roles.Single( x => x.Name == "Admin" );

				var result = context.Users
					.Where( x => x.Id == id )
					.Select( x => new UserModel
					{
						Id = x.Id,
						UserName = x.UserName,
						Email = x.Email,
						EmailConfirmed = x.EmailConfirmed,
						AccessFailedCount = x.AccessFailedCount,
						LockoutEnabled = x.LockoutEnabled,
						LockoutEndDateUtc = x.LockoutEndDateUtc,
						Age = x.Age,
						Education = x.Education,
						Party = x.Party == null ? "N/A" : x.Party.Name,
						Location = x.Location == null ? "N/A" : x.Location.Name,
						IsAdmin = x.Roles.Any( r => r.RoleId == adminRole.Id ),
						IsDisabled = x.IsDisabled
					} )
					.SingleOrDefault();

				return result;
			}
		}

		internal void SetAdmin( string id, bool adminState, ApplicationUserManager manager )
		{
			if ( adminState )
			{
				if ( manager.IsInRoleAsync( id, "admin" ).Result )
				{
					return;
				}
				var addRes = manager.AddToRoleAsync( id, "admin" ).Result;
			}
			else
			{
				if ( manager.IsInRoleAsync( id, "admin" ).Result )
				{
					var remRes = manager.RemoveFromRoleAsync( id, "admin" ).Result;
				}
				return;
			}

		}

		internal void SetDisabled( string id, bool disabledState )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var user = context.Users
					.Where( x => x.Id == id )
					.SingleOrDefault();


				if ( disabledState )
				{
					if ( user.IsDisabled )
					{
						return;
					}
					user.IsDisabled = true;
					context.SaveChanges();
				}
				else
				{
					if ( user.IsDisabled )
					{
						user.IsDisabled = false;
						context.SaveChanges();
					}
					return;
				}
			}

		}

		#endregion
	}
}