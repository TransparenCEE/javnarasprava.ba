using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Models.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavnaRasprava.WEB.Infrastructure.Helpers;
using System.Data.Entity;
using PagedList;

namespace JavnaRasprava.WEB.BLL
{
	public class QuizService
	{
		public List<QuizSummaryModel> GetAllQuiz( int parliamentId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context
					.Quizes
					.Where( x => x.ParliamentId == parliamentId )
					.Select( x => new QuizSummaryModel { Title = x.Title, QuizId = x.QuizId } )
					.ToList();
			}
		}

		public List<QuizSummaryModel> GetQuizSummaryModelList( List<int> ids )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetQuizSummaryModelListInternal( context, ids );
			}
		}

		internal QuizListModel InitializeSearchModel( int parliamentId )
		{
			var searchModel = new QuizSearchModel
			{
				ParliamentId = parliamentId
			};
			var resultModel = SearchQuiz( searchModel );
			return new QuizListModel
			{
				SearchModel = new QuizSearchModel(),
				Results = resultModel
			};
		}


		internal IPagedList<QuizSummaryModel> SearchQuiz( QuizSearchModel searchModel )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var query = context.Quizes
					.Where( x => 1 == 1 );

				if ( searchModel.ParliamentId != 0 )
					query = query.Where( x => x.ParliamentId == searchModel.ParliamentId );

				if ( searchModel.QueryString != null )
					query = query.Where( x => x.Title.Contains( searchModel.QueryString ) ||
					x.Description.Contains( searchModel.QueryString ) );



				switch ( searchModel.Sort ?? QuizSort.CreateTime )
				{
					case QuizSort.CreateTime:
						query = query.OrderBy( x => x.TimeCreated );
						break;
					case QuizSort.Title:
						query = query.OrderBy( x => x.Title );
						break;
				}

				var result = query.Select( x => new QuizSummaryModel
				{
					Title = x.Title,
					Description = x.Description,
					ImageRelativePath = x.ImageRelativePath,
					QuizId = x.QuizId
				} )
				.ToPagedList( searchModel.page.HasValue ? searchModel.page.Value : 1, 16 );

				return result;
			}
		}

		internal static List<QuizSummaryModel> GetQuizSummaryModelListInternal( ApplicationDbContext context, List<int> ids )
		{
			return context
								.Quizes
								.Where( x => ids.Contains( x.QuizId ) )
								.Select( x => new QuizSummaryModel
								{
									Title = x.Title,
									QuizId = x.QuizId,
									Description = x.Description,
									ImageRelativePath = x.ImageRelativePath
								} )
								.ToList();
		}

		public QuizEditModel GetQuizEditModel( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context
					.Quizes
					.Where( x => x.QuizId == id )
					.Select( x => new QuizEditModel
					{
						QuizId = x.QuizId,
						TimeCreated = x.TimeCreated,
						Title = x.Title,
						Description = x.Description,
						ImageRelativePath = x.ImageRelativePath,
						Items = x.Items.Select( i => new QuizItemSummaryModel
						{
							QuizItemId = i.QuizItemId,
							QuizId = i.QuizId,
							Type = i.Type,
							Order = i.Order,
							LawId = i.Type == QuizItemType.Law ? i.Law.LawID : i.LawSection.LawID,
							LawTitle = i.Type == QuizItemType.Law ? i.Law.Title : i.LawSection.Law.Title,
							SectionId = i.Type == QuizItemType.LawSection ? i.LawSection.LawSectionID : (int?)null,
							SectionTitle = i.Type == QuizItemType.LawSection ? i.LawSection.Title : null
						} ).ToList()
					} )
					.FirstOrDefault();
			}
		}

		internal void UpdateQuiz( QuizEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = context.Quizes
					.Where( x => x.QuizId == model.QuizId )
					.FirstOrDefault();

				if ( quiz == null )
					return;


				quiz.Title = model.Title;
				quiz.Description = model.Description;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					quiz.ImageRelativePath = fileService.UploadFile( new List<string> { "QuizImage" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					quiz.ImageRelativePath = model.ImageRelativePath;
				}

				context.SaveChanges();
				return;
			}
		}

		internal void DeleteQuiz( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = context.Quizes
					.Where( x => x.QuizId == id )
					.FirstOrDefault();

				if ( quiz == null )
					return;

				new InfoBoxService().DeleteItems( id, InfoBoxItemType.Quiz, context );

				context.Quizes.Remove( quiz );
				context.SaveChanges();
			}
		}

		internal QuizEditModel InitializeQuizEditModel( int parliamentId )
		{
			return new QuizEditModel { ParliamentId = parliamentId };
		}

		internal void CreateQuiz( QuizEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = new Quiz();
				quiz.Title = model.Title;
				quiz.Description = model.Description;
				quiz.ParliamentId = model.ParliamentId;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					quiz.ImageRelativePath = fileService.UploadFile( new List<string> { "QuizImage" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					quiz.ImageRelativePath = model.ImageRelativePath;
				}

				context.Quizes.Add( quiz );
				context.SaveChanges();
				return;
			}
		}

		internal QuizItemEditModel InitializeEditQuizItemModel( int id, int? itemId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = context.Quizes
					.Include( x => x.Items )
					.Where( x => x.QuizId == id )
					.FirstOrDefault();

				if ( quiz == null )
					return null;

				var result = new QuizItemEditModel
				{
					QuizId = id,
					QuizTitle = quiz.Title,
					Order = quiz.Items.Count == 0 ? 1 : quiz.Items.Max( x => x.Order ) + 1
				};

				if ( itemId.HasValue )
				{
					var existingItem = quiz.Items.SingleOrDefault( x => x.QuizItemId == itemId );
					if ( existingItem == null )
						return null;

					result.QuizItemId = itemId.Value;
					result.Order = existingItem.Order;
				}

				result.Laws = context.Laws
					.Where( x => x.ParliamentID == quiz.ParliamentId )
					.OrderBy( x => x.Title )
					.Select( x => new System.Web.Mvc.SelectListItem { Text = x.Title, Value = x.LawID.ToString() } )
					.ToList();

				return result;
			}

		}


		internal void CreateNewQuizItemModel( QuizItemEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = context.Quizes
					.Include( x => x.Items )
					.Where( x => x.QuizId == model.QuizId )
					.FirstOrDefault();

				if ( quiz == null )
					return;

				var law = context.Laws.SingleOrDefault( x => x.LawID == model.LawId );
				if ( law == null )
					return;
				LawSection section = null;

				if ( model.LawSectionId > 0 )
				{
					section = context.LawSections.SingleOrDefault( x => x.LawSectionID == model.LawSectionId );
					if ( section == null )
						return;
				}

				var newItem = new QuizItem
				{
					Quiz = quiz,
					Order = model.Order,
					Law = section != null ? null : law,
					LawSection = section != null ? section : null,
					Type = section != null ? QuizItemType.LawSection : QuizItemType.Law,
				};

				context.QuizItems.Add( newItem );
				context.SaveChanges();


				return;
			}
		}

		internal void UpdateQuizItem( QuizItemEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = context.Quizes
					.Include( x => x.Items )
					.Where( x => x.QuizId == model.QuizId )
					.FirstOrDefault();

				if ( quiz == null )
					return;

				var item = quiz.Items.SingleOrDefault( x => x.QuizItemId == model.QuizItemId );
				if ( item == null )
					return;

				var law = context.Laws.SingleOrDefault( x => x.LawID == model.LawId );
				if ( law == null )
					return;
				LawSection section = null;

				if ( model.LawSectionId > 0 )
				{
					section = context.LawSections.SingleOrDefault( x => x.LawSectionID == model.LawSectionId );
					if ( section == null )
						return;
				}



				item.Law = section != null ? null : law;
				item.LawSection = section != null ? section : null;
				item.Type = section != null ? QuizItemType.LawSection : QuizItemType.Law;

				context.SaveChanges();


				return;
			}
		}

		internal int DeleteQuizItem( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var item = context.QuizItems
					.Where( x => x.QuizItemId == id )
					.FirstOrDefault();

				if ( item == null )
					return 0;

				context.QuizItems.Remove( item );
				context.SaveChanges();

				return item.QuizId;
			}
		}

		#region Doing Quiz

		public QuizAnsweringModel GetQuizAnsweringModel( int id, int? itemOrderId, string userId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var result = new QuizAnsweringModel
				{
					QuizId = id
				};

				var quiz = context
					.Quizes
					.Include( x => x.Items )
					.Include( "Items.Law" )
					.Include( "Items.LawSection.Law" )
					.Where( x => x.QuizId == id )
					.FirstOrDefault();

				if ( quiz == null )
					return null;

				var item = quiz.Items.SingleOrDefault( x => x.Order == itemOrderId );
				if ( itemOrderId.HasValue && item == null )
					return null;

				// populate quiz general data
				result.Title = quiz.Title;
				result.Description = quiz.Description;
				result.QuestionType = itemOrderId.HasValue ? item.Type : (QuizItemType?)null;



				// populate next item
				if ( quiz.Items.Count > 0 ) // if no items in list quiz is already over.
				{
					var orderedItems = quiz.Items.OrderBy( x => x.Order ).ToList();
					if ( !itemOrderId.HasValue )
					{
						result.NextQuestionId = orderedItems.First().Order;
					}
					else
					{
						int currentItemIndex = orderedItems.FindIndex( x => x.Order == itemOrderId );
						if ( orderedItems.Count > currentItemIndex + 1 )
						{
							result.NextQuestionId = orderedItems[ currentItemIndex + 1 ].Order;
						}
						else
						{
							result.NextQuestionId = -1;
						}
					}

					result.ProgressPercentage = JavnaRasprava.WEB.Infrastructure.Math.Percentage( itemOrderId.HasValue ? itemOrderId.Value : 0,
					quiz.Items.Count ).ToString();
				}

				result.CurrentItemIndex = itemOrderId;
				result.TotalItems = quiz.Items.Count;


				if ( item == null )
				{
					result.ImageRelativePath = quiz.ImageRelativePath;
				}
				else
				{
					AnonymousUserService auService = new AnonymousUserService();


					// Populate law
					if ( item.Type == QuizItemType.Law )
					{
						result.LawId = item.Law.LawID;
						result.LawTitle = item.Law.Title;
						result.QuestionDescription = item.Law.Text;
						result.LawVotes = LawService.GetLawCustomVotesListInternal( item.Law.LawID, context );
						result.ImageRelativePath = item.Law.ImageRelativePath;

						if ( userId == null )
						{
							if ( auService.HasVotedLaw( item.Law.LawID ) )
							{
								result.UserVoteId = auService.GetUserLawVote( item.Law.LawID );
								result.CustomUserVoteText = auService.GetUserLawVoteCustomText( item.Law.LawID );
							}
						}
						else
						{
							var userLawVote = context.LawVotes
								.Include( x => x.LawCustomVote )
								.Where( x => x.ApplicationUserID == userId )
								.Where( x => x.LawID == item.Law.LawID )
								.FirstOrDefault();

							if ( userLawVote != null )
							{
								if ( userLawVote.LawCustomVote != null )
								{
									result.UserVoteId = -1;
									result.CustomUserVoteText = userLawVote.LawCustomVote.Text;
								}
								else
								{
									result.UserVoteId = userLawVote.Vote.Value ? -3 : -2;
								}
							}

						}
					}
					else
					{
						result.LawId = item.LawSection.LawID;
						result.LawTitle = item.LawSection.Law.Title;
						result.SectionId = item.LawSection.LawSectionID;
						result.SectionTitle = item.LawSection.Title;
						result.QuestionDescription = item.LawSection.Text;
						result.SectionVotes = LawService.GetLawSectionCustomVotesListInternal( item.LawSection.LawID, item.LawSection.LawSectionID, context );
						result.ImageRelativePath = item.LawSection.ImageRelativePath;

						if ( userId == null )
						{
							if ( auService.HasVotedLawSection( item.LawSection.LawSectionID ) )
							{
								result.UserVoteId = auService.GetUserLawSectionVote( item.LawSection.LawSectionID );
								result.CustomUserVoteText = auService.GetUserLawSectionVoteCustomText( item.LawSection.LawSectionID );
							}
						}
						else
						{
							var userLawSectionVote = context.LawSectionVotes
								.Include( x => x.LawSectionCustomVote )
								.Where( x => x.LawSectionID == item.LawSection.LawSectionID )
								.Where( x => x.ApplicationUserID == userId )
								.FirstOrDefault();

							if ( userLawSectionVote != null )
							{
								if ( userLawSectionVote.LawSectionCustomVote != null )
								{
									result.UserVoteId = -1;
									result.CustomUserVoteText = userLawSectionVote.LawSectionCustomVote.Text;
								}
								else
								{
									result.UserVoteId = userLawSectionVote.Vote.Value ? -3 : -2;
								}
							}

						}
					}



				}

				return result;
			}
		}

		internal QuizResultsModel GetQuizResults( int id, string userId )
		{
			var auService = new AnonymousUserService();
			using ( var context = ApplicationDbContext.Create() )
			{
				var quiz = context
					.Quizes
					.Include( x => x.Items )
					.Include( "Items.Law" )
					.Include( "Items.LawSection.Law" )
					.Where( x => x.QuizId == id )
					.FirstOrDefault();

				if ( quiz == null )
					return null;

				var result = new QuizResultsModel
				{
					QuizId = quiz.QuizId,
					TimeCreated = quiz.TimeCreated,
					Description = quiz.Description,
					Title = quiz.Title,
					ImageRelativePath = quiz.ImageRelativePath,
					Items = new List<QuizItemResultsModel>()
				};


				foreach ( var questionItem in quiz.Items )
				{
					QuizItemResultsModel item = null;
					if ( questionItem.Type == QuizItemType.Law )
					{
						item = PopulateResultsForLawAnswer( userId, auService, context, questionItem );
					}
					else if ( questionItem.Type == QuizItemType.LawSection )
					{
						item = PopulateResultsForLawSectionAnswer( userId, auService, context, questionItem );
					}
					result.Items.Add( item );
				}

				return result;
			}

		}

		private QuizItemResultsModel PopulateResultsForLawSectionAnswer( string userId, AnonymousUserService auService, ApplicationDbContext context, QuizItem questionItem )
		{
			// Populate general law data
			var item = new QuizItemResultsModel
			{
				LawId = questionItem.LawSection.LawID,
				LawTitle = questionItem.LawSection.Law.Title,
				SectionId = questionItem.LawSection.LawSectionID,
				SectionTitle = questionItem.LawSection.Title,
				QuestionDescription = questionItem.LawSection.Text,
				ImageRelativePath = questionItem.LawSection.ImageRelativePath,
				QuestionType = QuizItemType.LawSection
			};

			// This is copied from Law service. Look to make it generic with expression trees

			var sectionVotingResults = context.LawSectionVotes
										.Where( x => x.LawSectionID == questionItem.LawSection.LawSectionID )
										.GroupBy( x => x.Vote )
										.Select( g => new { Key = g.Key, Count = g.Count() } )
										.ToList();

			var unsafeResult = sectionVotingResults.Where( x => x.Key.HasValue && x.Key.Value ).FirstOrDefault();
			item.VotesUp = unsafeResult == null ? 0 : unsafeResult.Count;

			unsafeResult = sectionVotingResults.Where( x => x.Key.HasValue && !x.Key.Value ).FirstOrDefault();
			item.VotesDown = unsafeResult == null ? 0 : unsafeResult.Count;

			item.VotesDownPercentage = Infrastructure.Math.Percentage( item.VotesDown, item.VotesDown + item.VotesUp );
			item.VotesUpPercentage = Infrastructure.Math.Percentage( item.VotesUp, item.VotesDown + item.VotesUp );

			if ( userId != null )
			{
				var userVote = context.LawSectionVotes
					.Include( x => x.LawSectionCustomVote )
					.Where( x => x.ApplicationUserID == userId && x.LawSectionID == questionItem.LawSection.LawSectionID )
					.FirstOrDefault();

				if ( userVote != null )
				{
					item.UserVote = userVote.Vote;
					if ( userVote.LawSectionCustomVote != null )
					{
						item.CustomUserVoteText = userVote.LawSectionCustomVote.Text;
					}

				}
			}
			else
			{
				item.UserVote = auService.GetUserLawSectionVoteBool( questionItem.LawSection.LawSectionID );
				item.CustomUserVoteText = auService.GetUserLawSectionVoteCustomText( questionItem.LawSection.LawSectionID );
			}

			return item;
		}

		private static QuizItemResultsModel PopulateResultsForLawAnswer( string userId, AnonymousUserService auService, ApplicationDbContext context, QuizItem questionItem )
		{
			// Populate general law data
			var item = new QuizItemResultsModel
			{
				LawId = questionItem.Law.LawID,
				LawTitle = questionItem.Law.Title,
				QuestionDescription = questionItem.Law.Text,
				ImageRelativePath = questionItem.Law.ImageRelativePath,
				QuestionType = QuizItemType.Law
			};

			// This is copied from Law service. Look to make it generic with expression trees
			var lawVotingResults = context.LawVotes
									.Where( x => x.LawID == questionItem.Law.LawID )
									.GroupBy( x => x.Vote )
									.Select( g => new { Key = g.Key, Count = g.Count() } )
									.ToList();

			var unsafeResult = lawVotingResults.Where( x => x.Key.HasValue && x.Key.Value ).FirstOrDefault();
			item.VotesUp = unsafeResult == null ? 0 : unsafeResult.Count;

			unsafeResult = lawVotingResults.Where( x => x.Key.HasValue && !x.Key.Value ).FirstOrDefault();
			item.VotesDown = unsafeResult == null ? 0 : unsafeResult.Count;

			item.VotesDownPercentage = Infrastructure.Math.Percentage( item.VotesDown, item.VotesDown + item.VotesUp );
			item.VotesUpPercentage = Infrastructure.Math.Percentage( item.VotesUp, item.VotesDown + item.VotesUp );

			if ( userId != null )
			{
				var userVote = context.LawVotes
					.Include( x => x.LawCustomVote )
					.Where( x => x.ApplicationUserID == userId && x.LawID == questionItem.Law.LawID )
					.FirstOrDefault();

				if ( userVote != null )
				{
					item.UserVote = userVote.Vote;
					if ( userVote.LawCustomVote != null )
					{
						item.CustomUserVoteText = userVote.LawCustomVote.Text;
					}

				}
			}
			else
			{
				item.UserVote = auService.GetUserLawVoteBool( questionItem.Law.LawID );
				item.CustomUserVoteText = auService.GetUserLawVoteCustomText( questionItem.Law.LawID );
			}

			return item;
		}










		#endregion
	}
}