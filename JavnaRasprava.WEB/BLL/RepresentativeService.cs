using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using EntityFramework.Extensions;
using JavnaRasprava.WEB.Models.Representative;

namespace JavnaRasprava.WEB.BLL
{
	public class RepresentativeService
	{
		#region == Methods ==

		public RepresentativeModel GetRepresentative( int representativeID, string userId = null )
		{
			Dictionary<int, bool> userLikedQuestions = null;
			Dictionary<int, bool> userLikedAnswers = null;

			var auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new RepresentativeModel
				{
					Laws = new List<RepresentativeLawModel>()
				};

				var representative = context.Representatives
					.Where( x => x.RepresentativeID == representativeID )
					.Include( x => x.Party )
					.Include("ParliamentHouse.Parliament.ParliamentHouses" )
                    .Include( x => x.ExternalLinks )
					.Include( x => x.Assignments )
					.FirstOrDefault();

				if ( representative == null )
					return null;

				var questions = context.UserRepresentativeQuestions
					.Where( x => x.RepresentativeID == representativeID )
					.Where( x => x.Question.Verified )
					.GroupBy( x => new
					{
						RepID = x.RepresentativeID,
						QuestionID = x.QuestionID,
						QuestionText = x.Question.Text,
						LawID = x.Question.LawID,
						AskedTimeUtc = x.Question.CreateTimeUtc
					} )
					.Select( g => new QuestionHelper
					{
						QuestionID = g.Key.QuestionID,
						QuestionText = g.Key.QuestionText,
						RepID = g.Key.RepID,
						LawID = g.Key.LawID,
						AskedTimeUtc = g.Key.AskedTimeUtc,
						Count = g.Count()
					} )
					.ToList();

				var lawIDs = questions.Select( x => x.LawID ).ToList();
				var questionIDs = questions.Select( x => x.QuestionID ).ToList();

				var laws = context.Laws
					.Where( x => lawIDs.Contains( x.LawID ) )
					.ToList();

				var answers = context.Answers
					.Where( a => a.RepresentativeID == representativeID )
					.Where( x => questionIDs.Contains( x.QuestionID ) )
					.ToList();

				var answerIDs = answers.Select( x => x.AnswerID ).ToList();

				var lawVotingResults = context.LawVotes
												.Where( x => lawIDs.Contains( x.LawID ) )
												.GroupBy( x => x.Vote )
												.Select( g => new { Key = g.Key, Count = g.Count() } )
												.ToList();

				var questionLikeCounts = context.QuestionLikes
					.Where( x => questionIDs.Contains( x.QuestionID ) )
					.GroupBy( x => new { x.QuestionID, x.Vote } )
					.Select( g => new LikeHelper { ObjectID = g.Key.QuestionID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				var answerLikesCounts = context.AnswerLikes
					.Where( x => answerIDs.Contains( x.AnswerID ) )
					.GroupBy( x => new { x.AnswerID, x.Vote } )
					.Select( g => new LikeHelper { ObjectID = g.Key.AnswerID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				if ( userId != null )
				{
					userLikedAnswers = context.AnswerLikes
						.Where( x => x.ApplicationUserID == userId )
						.Where( x => x.Answer.RepresentativeID == representativeID )
						.ToDictionary( k => k.AnswerID, v => v.Vote );

					userLikedQuestions = context.QuestionLikes
						.Where( x => x.Question.UserRepresentativeQuestions.Select( y => y.RepresentativeID ).Contains( representativeID ) )
						.Where( x => x.ApplicationUserID == userId )
						.ToDictionary( k => k.QuestionID, v => v.Vote );
				}
				else
				{
					userLikedQuestions = auService.GetUserQuestionLikes();
					userLikedAnswers = auService.GetUserAnswerLikes();
				}

				result.TotalQuestions = questionIDs.Distinct().Count();
				result.TotalAnswers = answerIDs.Distinct().Count();
				CalculateFlagsForRepresentative( result );

				// local vars
				List<JavnaRasprava.WEB.Models.Representative.RepresentativeQuestionModel> questionModelList = null;
				IEnumerable<QuestionHelper> questionsToIterate = null;

				// Process laws to get questions and answers on laws
				foreach ( var law in laws )
				{
					questionModelList = new List<Models.Representative.RepresentativeQuestionModel>();
					var lawModel = new RepresentativeLawModel
					{
						ID = law.LawID,
						Title = law.Title,
						Questions = new List<Models.Representative.RepresentativeQuestionModel>()
					};
					var unsafeResult = lawVotingResults.Where( x => x.Key.HasValue && x.Key.Value ).FirstOrDefault();
					lawModel.VotesUp = unsafeResult == null ? 0 : unsafeResult.Count;

					unsafeResult = lawVotingResults.Where( x => x.Key.HasValue && !x.Key.Value ).FirstOrDefault();
					lawModel.VotesDown = unsafeResult == null ? 0 : unsafeResult.Count;

					lawModel.VotesDownPercentage = Infrastructure.Math.Percentage( lawModel.VotesDown, lawModel.VotesDown + lawModel.VotesUp );
					lawModel.VotesUpPercentage = Infrastructure.Math.Percentage( lawModel.VotesUp, lawModel.VotesDown + lawModel.VotesUp );

					questionsToIterate = questions.Where( x => x.LawID == law.LawID );
					questionModelList = PopulateQuestionsModelInternal( questionsToIterate, answers, questionLikeCounts, answerLikesCounts, userLikedQuestions, userLikedAnswers );

					lawModel.Questions.AddRange( questionModelList );
					lawModel.LatestAnswerTime = questionModelList.First().AnswerTime;
					result.Laws.Add( lawModel );
				}

				// process questions asked directly to rep
				questionsToIterate = questions.Where( x => x.LawID == null );
				questionModelList = PopulateQuestionsModelInternal( questionsToIterate, answers, questionLikeCounts, answerLikesCounts, userLikedQuestions, userLikedAnswers );
				result.Questions = new List<Models.Representative.RepresentativeQuestionModel>( questionModelList.OrderByDescending( x => x.AnswerTime ) );

				result.Representative = representative;
                result.IsSingleHouseParliament = representative.ParliamentHouse.Parliament.ParliamentHouses.Count() == 1;
				result.Laws = result.Laws.OrderByDescending( x => x.LatestAnswerTime ).ToList();

				return result;
			}
		}


		private static List<JavnaRasprava.WEB.Models.Representative.RepresentativeQuestionModel> PopulateQuestionsModelInternal( IEnumerable<QuestionHelper> questionsToIterate,
			List<Answer> answers, List<LikeHelper> questionLikeCounts, List<LikeHelper> answerLikesCounts,
			Dictionary<int, bool> userLikedQuestions, Dictionary<int, bool> userLikedAnswers )
		{
			var questionModelList = new List<JavnaRasprava.WEB.Models.Representative.RepresentativeQuestionModel>();
			foreach ( var question in questionsToIterate )
			{
				var qmodel = new JavnaRasprava.WEB.Models.Representative.RepresentativeQuestionModel
				{
					ID = question.QuestionID,
					Title = question.QuestionText,
					AskedTimeUtc = question.AskedTimeUtc
				};

				qmodel.AskedCount = question.Count;
				qmodel.LikesCount = questionLikeCounts.Where( x => x.ObjectID == question.QuestionID && x.Vote ).Select( x => x.Count ).FirstOrDefault();
				qmodel.DislikesCount = questionLikeCounts.Where( x => x.ObjectID == question.QuestionID && !x.Vote ).Select( x => x.Count ).FirstOrDefault();
				if ( userLikedQuestions != null )
				{
					if ( userLikedQuestions.ContainsKey( question.QuestionID ) )
						qmodel.UserLiked = userLikedQuestions[ question.QuestionID ];
				}


				var answer = answers.Where( x => x.QuestionID == question.QuestionID ).FirstOrDefault();

				if ( answer != null )
				{
					var answerModel = new JavnaRasprava.WEB.Models.Representative.RepresentativeAnswerModel
					{
						ID = answer.AnswerID,
						Answer = answer.Text,
						AnswerTime = answer.AnsweredTimeUtc
					};

					answerModel.LikesCount = answerLikesCounts.Where( x => x.ObjectID == answer.AnswerID && x.Vote ).Select( x => x.Count ).FirstOrDefault();
					answerModel.DislikesCount = answerLikesCounts.Where( x => x.ObjectID == answer.AnswerID && !x.Vote ).Select( x => x.Count ).FirstOrDefault();
					if ( userLikedAnswers != null )
					{
						if ( userLikedAnswers.ContainsKey( answer.AnswerID ) )
							answerModel.UserLiked = userLikedAnswers[ answer.AnswerID ];
					}

					qmodel.Answer = answerModel;
					qmodel.AnswerTime = answerModel.AnswerTime;
				}
				questionModelList.Add( qmodel );
			}

			return questionModelList.OrderByDescending( x => x.AnswerTime ).ToList();
		}

		public RepresentativeListModel SearchForRepresentativesForParliament( Models.Representative.RepresentativeSearchModel searchModel )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				InitializerepresentativeSearchModelInternal( context, searchModel );
                var results = SearchRepresentativesInternal(searchModel, context);
                results.Title = context.Parliaments.SingleOrDefault(x => x.ParliamentID == searchModel.ParliamentId)?.RepresentativesScreenTitle;
                return results;
            }
		}

		public RepresentativeListModel GetAllRepresentativesForParliament( int parliamentID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
                var searchModel = new Models.Representative.RepresentativeSearchModel();
				searchModel.ParliamentId = parliamentID;
				InitializerepresentativeSearchModelInternal( context, searchModel );
				var results = SearchRepresentativesInternal( searchModel, context );
                results.Title = context.Parliaments.SingleOrDefault(x => x.ParliamentID == parliamentID)?.RepresentativesScreenTitle;
                return results;
			}
		}

		private static void InitializerepresentativeSearchModelInternal( ApplicationDbContext context, Models.Representative.RepresentativeSearchModel searchModel )
		{
			searchModel.Parties = context.ParliamentHouses
				.Where( x => x.ParliamentID == searchModel.ParliamentId )
				.SelectMany( x => x.Representatives )
				.Select( x => x.Party )
				.Distinct()
				.Select( x => new PartyModel { FullName = x.FullName, Name = x.Name, PartyID = x.PartyID } )
				.ToList();
		}

		private static RepresentativeListModel SearchRepresentativesInternal( Models.Representative.RepresentativeSearchModel searchModel,
			ApplicationDbContext context )
		{
			var parliament = context
							.Parliaments
							.Where( x => x.ParliamentID == searchModel.ParliamentId )
							.Include( x => x.ParliamentHouses.Select( ph => ph.Representatives.Select( y => y.Party ) ) )
							.FirstOrDefault();

			if ( parliament == null )
				return null;

			var allRepresentativeIds = parliament.ParliamentHouses
				.SelectMany( x => x.Representatives )
				.ToList()
				.Where( x => searchModel.SearchName == null || ( x.FirstName + " " + x.LastName ).ToLowerInvariant().Contains( searchModel.SearchName.ToLowerInvariant() )
					|| ( x.LastName + " " + x.FirstName ).ToLowerInvariant().Contains( searchModel.SearchName.ToLowerInvariant() ) )
				.Where( x => searchModel.SelectedParty == null || x.PartyID == searchModel.SelectedParty )
				.Select( x => x.RepresentativeID ).ToList();

			var questions = GetRepresentativeQuestionCount( context, allRepresentativeIds );

			var answers = GetRepresentativeAnswerCount( context, allRepresentativeIds );


			var result = new RepresentativeListModel
			{
				SearchModel = searchModel,
				ParliamentHouses = new List<ParliamentHouseModel>(),
				ParliamentName = parliament.Name
			};

			foreach ( var house in parliament.ParliamentHouses )
			{
				var houseModel = new ParliamentHouseModel
				{
					Name = house.Name,
					ParliamentHouseID = house.ParliamentHouseID,
				};

				var representatives = new List<RepresentativeModel>();
				foreach ( var rep in house.Representatives.Where( x => allRepresentativeIds.Contains( x.RepresentativeID ) ) )
				{
					var repModel = new RepresentativeModel
					{
						Representative = rep,
						TotalAnswers = answers.ContainsKey( rep.RepresentativeID ) ? answers[ rep.RepresentativeID ] : 0,
						TotalQuestions = questions.ContainsKey( rep.RepresentativeID ) ? questions[ rep.RepresentativeID ] : 0
					};

					CalculateFlagsForRepresentative( repModel );
					representatives.Add( repModel );
				}

				switch ( searchModel.SortOrder )
				{
					case Models.Representative.SortOrder.MostQuestions:
						houseModel.Representatives = representatives.OrderByDescending( x => x.TotalQuestions ).ToList();
						break;
					case Models.Representative.SortOrder.MostAnswers:
					case Models.Representative.SortOrder.None:
					default:
						houseModel.Representatives = representatives
							.OrderByDescending( x => x.PercentageAnswered )
							.ThenByDescending( x => x.TotalQuestions )
							.ToList();
						break;
				}
				result.ParliamentHouses.Add( houseModel );
			}

			return result;
		}

		internal static Dictionary<int, int> GetRepresentativeQuestionCount( ApplicationDbContext context, ICollection<int> representativeIds )
		{
			return context.UserRepresentativeQuestions
				.Where( x => representativeIds.Contains( x.RepresentativeID ) )
				.Where( x => x.Question.Verified )
				.GroupBy( x => new
				{
					RepID = x.RepresentativeID
				} )
				.Select( g => new
				{
					RepId = g.Key.RepID,
					Count = g.Select( s => s.QuestionID ).Distinct().Count()
				} )
				.ToDictionary( x => x.RepId, y => y.Count );
		}

		internal static Dictionary<int, int> GetRepresentativeAnswerCount( ApplicationDbContext context, ICollection<int> representativeIds )
		{
			return context.Answers
				.Where( x => representativeIds.Contains( x.RepresentativeID ) )
				.Where( x => x.Question.Verified )
				.GroupBy( x => x.RepresentativeID )
				.Select( g => new
				{
					RepId = g.Key,
					Count = g.Count()
				} )
				.ToDictionary( x => x.RepId, y => y.Count );
		}

		private static void CalculateFlagsForRepresentative( RepresentativeModel repModel )
		{
			repModel.PercentageAnswered = repModel.TotalAnswers == 0 || repModel.TotalQuestions == 0 ? 0 :
				Infrastructure.Math.Percentage( repModel.TotalAnswers, repModel.TotalQuestions );

			if ( repModel.TotalQuestions == 0 )
			{
				repModel.Gray = true;
				return;
			}

			repModel.Green = repModel.PercentageAnswered > 66;
			repModel.Red = repModel.PercentageAnswered < 33;
			repModel.Yellow = !repModel.Green && !repModel.Red;
		}

		public TopRepresentativesModel GetTopRepresentatives( int take, int parliamentId )
		{
			var result = new TopRepresentativesModel
			{
				MostActive = new List<RepresentativeModel>(),
				MostInactive = new List<RepresentativeModel>()
			};

			using ( var context = ApplicationDbContext.Create() )
			{
				var representatives = context.ParliamentHouses
					.Where( x => x.ParliamentID == parliamentId )
					.SelectMany( x => x.Representatives )
					.Include( x => x.Party )
					.ToList();

				var representativeIds = representatives.Select( x => x.RepresentativeID ).ToList();


				var questions = GetRepresentativeQuestionCount( context, representativeIds );

				var answers = GetRepresentativeAnswerCount( context, representativeIds );


				var representativeModels = new List<RepresentativeModel>();
				foreach ( var rep in representatives )
				{
					var repModel = new RepresentativeModel
					{
						Representative = rep,
						TotalAnswers = answers.ContainsKey( rep.RepresentativeID ) ? answers[ rep.RepresentativeID ] : 0,
						TotalQuestions = questions.ContainsKey( rep.RepresentativeID ) ? questions[ rep.RepresentativeID ] : 0
					};

					CalculateFlagsForRepresentative( repModel );
					representativeModels.Add( repModel );
				}

				result.MostActive = representativeModels
					.OrderByDescending( x => x.PercentageAnswered )
					.ThenByDescending( x => x.TotalQuestions )
					.Take( take )
					.ToList();

				result.MostInactive = representativeModels
					.OrderBy( x => x.PercentageAnswered )
					.ThenByDescending( x => x.TotalQuestions )
					.Take( take )
					.ToList();

				return result;
			}
		}

		public RepresentativeEditModel GetRepresentativeEditModel( int representativeID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var rep = context.Representatives
					.Where( x => x.RepresentativeID == representativeID )
					.Include( x => x.ExternalLinks )
					.Include( x => x.ParliamentHouse.Parliament )
					.Include( x => x.Party )
					.Include( x => x.Assignments )
					.FirstOrDefault();

				if ( rep == null )
					return null;

				RepresentativeEditModel model = InitializeRepresentativeEditModel( context );

				model.RepresentativeID = rep.RepresentativeID;
				model.FirstName = rep.FirstName;
				model.LastName = rep.LastName;
				model.Resume = rep.Resume;
				model.Email = rep.Email;
				model.ImageRelativePath = rep.ImageRelativePath;
				model.SelectedPartyID = rep.PartyID;
				model.ExternalLinks = TransformExternalLinks( rep.ExternalLinks );
				model.Assignments = TransformAssignments( rep.Assignments );
				model.PartyName = rep.Party.Name;
				model.ParliamentHouseName = rep.ParliamentHouse.Name;
				model.ParliamentName = rep.ParliamentHouse.Parliament.Name;
				model.NumberOfVotes = rep.NumberOfVotes;
				model.EletorialUnit = rep.EletorialUnit;
				model.Function = rep.Function;

				model.UserRepresentativeQuestions = context.UserRepresentativeQuestions
					.Where( x => x.RepresentativeID == representativeID )
					.Where( x => x.Question.Law == null )
					.Select( x => new JavnaRasprava.WEB.Models.Representative.RepresentativeQuestionModel
					{
						ID = x.QuestionID,
						Title = x.Question.Text
					} )
					.ToList();

				return model;
			}
		}



		public RepresentativeEditModel InitializeRepresentativeModelForCreate( int parliamentHouseID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var model = InitializeRepresentativeEditModel( context );
				model.ParliamentHouseID = parliamentHouseID;

				return model;
			}
		}

		public RepresentativeEditModel UpdateRepresentative( RepresentativeEditModel model )
		{
			if ( model == null )
				return null;

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var party = context.Parties.Where( x => x.PartyID == model.SelectedPartyID ).FirstOrDefault();

				if ( party == null )
					return null;

				var representative = context.Representatives.Where( x => x.RepresentativeID == model.RepresentativeID ).FirstOrDefault();

				if ( representative == null )
					return null;

				representative.FirstName = model.FirstName;
				representative.LastName = model.LastName;
				// representative.ImageRelativePath = model.ImageRelativePath;
				representative.Resume = model.Resume;
				representative.Email = model.Email;
				representative.PartyID = model.SelectedPartyID;

				representative.NumberOfVotes = model.NumberOfVotes;
				representative.EletorialUnit = model.EletorialUnit;
				representative.Function = model.Function;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					representative.ImageRelativePath = fileService.UploadFile( new List<string> { "Representatives" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					representative.ImageRelativePath = model.ImageRelativePath;
				}

				context.SaveChanges();
			}

			return GetRepresentativeEditModel( model.RepresentativeID );
		}

		public bool DeleteRepresentative( int representativeID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var representative = context.Representatives.Where( x => x.RepresentativeID == representativeID ).FirstOrDefault();

				if ( representative == null )
					return false;

				context.LawRepresentativeAssociations.Where( x => x.RepresentativeID == representativeID ).Delete();
				context.UserRepresentativeQuestions.Where( x => x.RepresentativeID == representativeID ).Delete();
				context.Answers.Where( x => x.RepresentativeID == representativeID ).Delete();
				context.Representatives.Remove( representative );

				context.SaveChanges();

				return true;
			}
		}

		public int CreateRepresentative( RepresentativeEditModel model )
		{
			if ( model == null ||
				model.SelectedPartyID <= 0 )
				return 0;

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var party = context.Parties.Where( x => x.PartyID == model.SelectedPartyID ).FirstOrDefault();

				if ( party == null )
					return 0;

				var house = context.ParliamentHouses
					.Where( x => x.ParliamentHouseID == model.ParliamentHouseID )
					.FirstOrDefault();

				if ( house == null )
					return 0;


				var representative = new Representative
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Resume = model.Resume,
					Email = model.Email,
					ParliamentHouseID = model.ParliamentHouseID,
					PartyID = model.SelectedPartyID,
					NumberOfVotes = model.NumberOfVotes,
					EletorialUnit = model.EletorialUnit,
					Function = model.Function
				};


				if ( model.Image != null )
				{
					var fileService = new FileService();
					representative.ImageRelativePath = fileService.UploadFile( new List<string> { "Representatives" }, model.Image.FileName, model.Image.InputStream );

				}

				context.Representatives.Add( representative );
				context.SaveChanges();

				return representative.RepresentativeID;
			}
		}

		public Models.Representative.RepresentativeAssignmentModel CreateAssignment( int representativeID, string text )
		{

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var rep = context.Representatives
					.Where( x => x.RepresentativeID == representativeID )
					.Include( x => x.ExternalLinks )
					.FirstOrDefault();

				if ( rep == null )
					return null;

				var assignment = new RepresentativeAssignment
				{
					Title = text,
					RepresentativeID = representativeID
				};

				context.RepresentativeAssignments.Add( assignment );
				context.SaveChanges();

				return new Models.Representative.RepresentativeAssignmentModel
				{
					RepresentativeAssignmentID = assignment.RepresentativeAssignmentID,
					RepresentativeID = assignment.RepresentativeID,
					Title = assignment.Title
				};
			}
		}

		public bool DeleteAssignment( int id )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var ass = context.RepresentativeAssignments.Where( x => x.RepresentativeAssignmentID == id ).FirstOrDefault();

				if ( ass == null )
					return false;

				context.RepresentativeAssignments.Remove( ass );

				context.SaveChanges();

				return true;
			}
		}

		public bool DeleteExternalLink( int externalLinkID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var link = context.ExternalLinks.Where( x => x.ExternalLinkID == externalLinkID ).FirstOrDefault();

				if ( link == null )
					return false;

				context.ExternalLinks.Remove( link );

				context.SaveChanges();

				return true;
			}
		}



		public Models.RepresentativeExternalLinkModel CreateLink( int representativeID, string descirption, string url )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var rep = context.Representatives
					.Where( x => x.RepresentativeID == representativeID )
					.Include( x => x.ExternalLinks )
					.FirstOrDefault();

				if ( rep == null )
					return null;

				var link = new ExternalLink
				{
					Description = descirption,
					Url = url,
					RepresentativeID = representativeID
				};

				context.ExternalLinks.Add( link );
				context.SaveChanges();

				return new RepresentativeExternalLinkModel
				{
					ExternalLinkID = link.ExternalLinkID,
					Url = link.Url,
					Description = link.Description
				};
			}
		}

		private List<RepresentativeExternalLinkModel> TransformExternalLinks( ICollection<DomainModels.ExternalLink> collection )
		{
			return collection.Select( x =>
				new RepresentativeExternalLinkModel
				{
					ExternalLinkID = x.ExternalLinkID,
					Description = x.Description,
					Url = x.Url
				} )
				.ToList();
		}

		private List<Models.Representative.RepresentativeAssignmentModel> TransformAssignments( ICollection<RepresentativeAssignment> collection )
		{
			return collection.Select( x => new Models.Representative.RepresentativeAssignmentModel
			{
				RepresentativeAssignmentID = x.RepresentativeAssignmentID,
				RepresentativeID = x.RepresentativeID,
				Title = x.Title
			} )
				.ToList();
		}

		private RepresentativeEditModel InitializeRepresentativeEditModel( DomainModels.ApplicationDbContext context )
		{
			return new RepresentativeEditModel
			{
				Parties = PartyService.GetAllPartiesInternal( context )
			};
		}

		#endregion

		#region == PrivateClasses ==

		private class QuestionHelper
		{
			public int QuestionID { get; set; }
			public string QuestionText { get; set; }
			public int RepID { get; set; }
			public int? LawID { get; set; }
			public int Count { get; set; }
			public DateTime AskedTimeUtc { get; set; }


		}

		private class LikeHelper
		{
			public int ObjectID { get; set; }
			public bool Vote { get; set; }
			public int Count { get; set; }
		}
		#endregion
	}
}