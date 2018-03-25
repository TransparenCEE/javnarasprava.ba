using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using PagedList;

namespace JavnaRasprava.WEB.BLL
{
	public class PetitionService
	{
		#region == Methods ==

		public Models.PetitionModel InitializePetitionModel()
		{
			var result = new Models.PetitionModel();
			using ( var context = ApplicationDbContext.Create() )
			{
				InitializePetitionModelInternal( result, context );
			}
			return result;
		}

		private static void InitializePetitionModelInternal( Models.PetitionModel result, ApplicationDbContext context )
		{
			result.TargetInstitutionList = context.Parliaments.Select( x => new Models.TargetInstitutionModel { Id = x.ParliamentID, Name = x.Name } ).ToList();
		}

		public int CreateNewPetition( Models.PetitionModel model )
		{
			if ( model == null )
				return 0;

			if ( string.IsNullOrWhiteSpace( model.SubmitterUserID ) )
				return 0;

			using ( var context = ApplicationDbContext.Create() )
			{
				var user = context.Users.Where( x => x.Id == model.SubmitterUserID ).FirstOrDefault();
				if ( user == null )
					return 0;

				Petition petition = new Petition
				{
					SubmitterUser = user,
					Verified = false,
					TargetInstitutionId = model.SelectedTargetInstitution,
					AdminIgnore = false,
					Description = model.Description,
					SubmitterName = model.SubmitterName,
					Title = model.Title
				};


				if ( string.IsNullOrWhiteSpace( petition.Title ) ||
					string.IsNullOrWhiteSpace( petition.Description ) ||
					string.IsNullOrWhiteSpace( petition.SubmitterName )
					)
					return 0;

				// Fix some values 


				var youtubeCode = EEP.Utility.Youtube.GetYoutubeIdFromUrl( model.YoutubeUrl );
				if ( youtubeCode != null )
					petition.YoutubeCode = youtubeCode;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					petition.ImageRelativePath = fileService.UploadFile( new List<string> { "PetitionImage" }, model.Image.FileName, model.Image.InputStream );

				}

				context.Petitions.Add( petition );

				context.SaveChanges();

				return petition.PetitionID;
			}
		}

		public Models.PetitionModel GetPetition( int id, string userID = null )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				ApplicationUser user = null;
				if ( !string.IsNullOrWhiteSpace( userID ) )
				{
					user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
					if ( user == null )
						return null;
				}

				var model = context.Petitions
					.Where( x => x.PetitionID == id )
					.Select( x => new Models.PetitionModel
					{
						PetitionID = x.PetitionID,
						Title = x.Title,
						Description = x.Description,
						TargetInstitution = x.TargetInstitution.Name,
						SelectedTargetInstitution = x.TargetInstitutionId,
						SubmitterName = x.SubmitterName,
						SubmitterUserID = x.SubmitterUserID,
						Verified = x.Verified,
						Signatures = x.PetitionSignatures.Count(),
						YoutubeCode = x.YoutubeCode,
						ImageRelativePath = x.ImageRelativePath
					} )
					.FirstOrDefault();
				if ( model == null )
					return null;

				InitializePetitionModelInternal( model, context );

				if ( user != null )
					model.UserSigned = context.PetitionSignatures.Any( x => x.PetitionID == id && x.ApplicationUserID == userID );

				model.SigningEnabled = model.Verified;

				var progress = context.PetitionProgresses
					.Where( x => x.ParliamentID == model.SelectedTargetInstitution )
					.Include( x => x.Representative )
					.OrderBy( x => x.Representative.NumberOfVotes )
					.ToList();

				var progressModelList = progress
					.Select( x => new Models.PetitionProgressModel
					{
						PetitionProgresID = x.PetitionProgressID,
						Description = x.Description,
						Title = x.Representative?.DisplayName ?? x.Title,
						ImageRelativePath = x.ImageToDoRelativePath,
						RepresentativeImageRelativePath = x.Representative?.ImageRelativePath,
						VotesCount = x.Representative?.NumberOfVotes ?? x.NumberOfVotes
					} );

				model.NextProgress = progressModelList.Where( x => x.VotesCount > model.Signatures )
					.OrderBy( x => x.VotesCount )
					.FirstOrDefault();

				return model;
			}
		}

		public Models.PetitionModel UpdatePetition( Models.PetitionModel model, string currentUserID )
		{
			if ( model == null )
				return null;

			if ( string.IsNullOrWhiteSpace( currentUserID ) )
				return null;

			using ( var context = ApplicationDbContext.Create() )
			{
				//var user = context.Users.Where( x => x.Id == currentUserID ).FirstOrDefault();
				//if ( user == null )
				//    return null;

				var petition = context.Petitions.Where( x => model.PetitionID == x.PetitionID ).FirstOrDefault();
				if ( petition == null )
					return null;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					petition.ImageRelativePath = fileService.UploadFile( new List<string> { "PetitionImage" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					petition.ImageRelativePath = model.ImageRelativePath;
				}

				AutoMapper.Mapper.Map<Models.PetitionModel, Petition>( model, petition );

				var youtubeCode = EEP.Utility.Youtube.GetYoutubeIdFromUrl( model.YoutubeUrl );
				if ( youtubeCode != null )
					petition.YoutubeCode = youtubeCode;

				context.SaveChanges();
			}

			return GetPetition( model.PetitionID, currentUserID );
		}

		public void DeletePetition( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var petition = context.Petitions.Where( x => id == x.PetitionID ).FirstOrDefault();
				if ( petition == null )
					return;

				context.Petitions.Remove( petition );
				context.SaveChanges();
			}
		}

		internal List<Models.PetitionSummaryModel> GetAllPetitions()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var now = DateTime.UtcNow;

				var query = context.Petitions
					.Include( x => x.TargetInstitution )
					.Where( x => 1 == 1 );

				var selectedQuery = query.Select( x => new Models.PetitionSummaryModel
				{
					Id = x.PetitionID,
					Title = x.Title,
					CurrentCount = x.PetitionSignatures.Count(),
					Verified = x.Verified,
					TargetInstitutionName = x.TargetInstitution.Name
				} );

				return selectedQuery.ToList();
			}
		}

		public void Sign( int petitionID, string userId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var user = context.Users.Where( x => x.Id == userId ).FirstOrDefault();
				if ( user == null )
					return;

				var petition = context.Petitions.Where( x => petitionID == x.PetitionID ).FirstOrDefault();
				if ( petition == null )
					return;

				var signature = context.PetitionSignatures.Where( x => x.ApplicationUserID == userId && x.PetitionID == petitionID ).FirstOrDefault();
				if ( signature != null )
					return;

				var newSignature = new PetitionSignature
				{
					PetitionID = petitionID,
					ApplicationUserID = userId,
					SignedTime = DateTime.UtcNow
				};

				context.PetitionSignatures.Add( newSignature );

				context.SaveChanges();
			}
		}

		public IPagedList<Models.PetitionSummaryModel> Search( string order, string queryString, int? pageNumber,
			bool onlyActive = false, bool mostActive = false, int pageItemCount = 5, int? parliamentId = null )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var now = DateTime.UtcNow;
				var lastMonth = now.AddDays( -30 );

				var query = context.Petitions
					.Include( x => x.TargetInstitution )
					.Where( x => 1 == 1 );

				if ( onlyActive )
				{
					order = "none";
					query = query.OrderByDescending( x => x.PetitionSignatures.Max( y => y.SignedTime ) );
				}

				if ( mostActive )
				{
					order = "none";
					query = query.OrderByDescending( x => x.PetitionSignatures.Count( y => y.SignedTime > lastMonth ) )
						.ThenByDescending( x => x.PetitionSignatures.Max( y => y.SignedTime ) );
				}

				if ( !string.IsNullOrWhiteSpace( queryString ) )
				{
					query = query.Where( x => x.Title.Contains( queryString ) || x.Description.Contains( queryString ) );
				}

				if ( parliamentId.HasValue )
				{
					query = query.Where( x => x.TargetInstitutionId == parliamentId.Value );
				}

				switch ( order )
				{
					case "title":
						query = query.OrderBy( x => x.Title );
						break;
					case "title_desc":
						query = query.OrderByDescending( x => x.Title );
						break;
					case "none":
						break;
					default:
						query = query.OrderBy( x => x.CreateTime );
						break;
				}


				var selectedQuery = query.Select( x => new Models.PetitionSummaryModel
				{
					Id = x.PetitionID,
					Title = x.Title,
					CurrentCount = x.PetitionSignatures.Count(),
					Verified = x.Verified,
					TargetInstitutionName = x.TargetInstitution.Name
				} );

				return selectedQuery.ToPagedList( pageNumber.HasValue ? pageNumber.Value : 1, pageItemCount );
			}
		}

		public IPagedList<Models.PetitionSummaryModel> GetTopActivePetitions( int pageItemCount )
		{
			return Search( null, null, null, pageItemCount: pageItemCount );
		}

		public IPagedList<Models.PetitionSummaryModel> GatLatestPetitions( int pageItemCount )
		{
			return Search( null, null, null, onlyActive: true, pageItemCount: pageItemCount );
		}

		public void VerifyPetition( int petitionId, string userId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var user = context.Users.Where( x => x.Id == userId ).FirstOrDefault();
				if ( user == null )
					return;

				var petition = context.Petitions.Where( x => x.PetitionID == petitionId ).FirstOrDefault();
				if ( petition == null )
					return;

				petition.Verified = true;

				context.SaveChanges();
			}
		}

		public List<Models.PetitionModel> GetUnverifiedPetitions()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.Petitions
					.Where( x => !x.Verified && !x.AdminIgnore )
					.ToList()
					.Select( x => AutoMapper.Mapper.Map<Petition, Models.PetitionModel>( x ) )
					.ToList();
			}
		}

		public int GetUnverifiedPetitionsCount()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.Petitions
					.Where( x => !x.Verified && !x.AdminIgnore )
					.Count();
			}
		}

		public List<Models.Petition.PetitionProgressSummaryModel> GetAllProgresses( int parliamentId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.PetitionProgresses
					.Where( x => x.ParliamentID == parliamentId )
					.Select( x => new Models.Petition.PetitionProgressSummaryModel
					{
						PetitionProgressId = x.PetitionProgressID,
						Title = x.Title
					} )
					.ToList();
			}

		}

		public Models.Petition.PetitionProgressEditModel InitializePetitionProgressEditModel( int parliamentId )
		{
			var result = new Models.Petition.PetitionProgressEditModel()
			{
				ParliamentID = parliamentId
			};

			using ( var context = ApplicationDbContext.Create() )
			{
				PopulateRepresentativesListForPetitionProgresEditModel( parliamentId, result, context );
			}

			return result;
		}

		private static void PopulateRepresentativesListForPetitionProgresEditModel( int parliamentId, Models.Petition.PetitionProgressEditModel model, ApplicationDbContext context, int? selectedRepresentative = null )
		{
			var configuredRepresentativeIds = context.PetitionProgresses
							.Where( x => x.ParliamentID == parliamentId )
							.Select( x => x.RepresentativeID )
							.ToList();

			if ( selectedRepresentative.HasValue )
				configuredRepresentativeIds.Remove( selectedRepresentative.Value );

			model.Representatives = context.Representatives
				.Where( x => x.ParliamentHouse.ParliamentID == parliamentId )
				.Where( x => !configuredRepresentativeIds.Contains( x.RepresentativeID ) )
				.ToList()
				.Select( x => new Models.Petition.RepresentativeModel
				{
					ID = x.RepresentativeID,
					Name = x.DisplayName
				} )
				.OrderBy( x => x.Name )
				.ToList();

			model.ParliamentName = context.Parliaments.Single( x => x.ParliamentID == parliamentId ).Name;
		}

		public Models.Petition.PetitionProgressEditModel GetPetitionProgressEditModel( int progressId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var progressItem = context.PetitionProgresses
					.Include( x => x.Representative )
					.Where( x => x.PetitionProgressID == progressId )
					.ToList()
					.Select( x => new Models.Petition.PetitionProgressEditModel
					{
						Description = x.Description,
						ImageDoneRelativePath = x.ImageDoneRelativePath,
						ImageToDoRelativePath = x.ImageToDoRelativePath,
						ParliamentID = x.ParliamentID,
						PetitionProgresID = x.PetitionProgressID,
						SelectedRepresentativeID = x.RepresentativeID,
						SelectedRepresentativeName = x.Representative?.DisplayName,
						NumberOfVotes = x.NumberOfVotes,
						Title = x.Title
					} )
					.FirstOrDefault();

				if ( progressItem == null )
					return null;

				PopulateRepresentativesListForPetitionProgresEditModel( progressItem.ParliamentID, progressItem, context, progressItem.SelectedRepresentativeID );

				return progressItem;
			}
		}

		public int CreatePetitionProgressEditModel( Models.Petition.PetitionProgressEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var progress = new PetitionProgress
				{
					Description = model.Description,
					ParliamentID = model.ParliamentID,
					RepresentativeID = model.SelectedRepresentativeID,
					Title = model.Title,
					NumberOfVotes = model.NumberOfVotes
				};

				if ( model.ImageToDo != null )
				{
					var fileService = new FileService();
					progress.ImageToDoRelativePath = fileService.UploadFile( new List<string> { "PetitionProgress" }, model.ImageToDo.FileName, model.ImageToDo.InputStream );

				}

				if ( model.ImageDone != null )
				{
					var fileService = new FileService();
					progress.ImageDoneRelativePath = fileService.UploadFile( new List<string> { "PetitionProgress" }, model.ImageDone.FileName, model.ImageDone.InputStream );
				}

				context.PetitionProgresses.Add( progress );
				context.SaveChanges();

				return progress.PetitionProgressID;
			}
		}

		public Models.Petition.PetitionProgressEditModel UpdatePetitionProgressEditModel( Models.Petition.PetitionProgressEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var progress = context.PetitionProgresses.SingleOrDefault( x => x.PetitionProgressID == model.PetitionProgresID );

				if ( progress == null )
					return null;

				progress.Description = model.Description;
				progress.ParliamentID = model.ParliamentID;
				progress.RepresentativeID = model.SelectedRepresentativeID;
				progress.Title = model.Title;
				progress.NumberOfVotes = model.NumberOfVotes;


				if ( model.ImageToDo != null )
				{
					var fileService = new FileService();
					progress.ImageToDoRelativePath = fileService.UploadFile( new List<string> { "PetitionProgress" }, model.ImageToDo.FileName, model.ImageToDo.InputStream );

				}

				if ( model.ImageDone != null )
				{
					var fileService = new FileService();
					progress.ImageDoneRelativePath = fileService.UploadFile( new List<string> { "PetitionProgress" }, model.ImageDone.FileName, model.ImageDone.InputStream );
				}

				context.SaveChanges();
			}

			return GetPetitionProgressEditModel( model.PetitionProgresID );
		}

		public bool DeletePetitionProgressEditModel( int progressId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var petitionProgress = context.PetitionProgresses.FirstOrDefault( x => x.PetitionProgressID == progressId );

				if ( petitionProgress == null )
					return false;

				context.PetitionProgresses.Remove( petitionProgress );
				context.SaveChanges();

				return true;
			}
		}

		#endregion


	}
}