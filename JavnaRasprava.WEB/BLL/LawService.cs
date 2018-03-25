using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Infrastructure;
using JavnaRasprava.WEB.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Quartz;
using JavnaRasprava.WEB.Models;
using JavnaRasprava.WEB.Models.InfoBox;
using JavnaRasprava.Resources;

namespace JavnaRasprava.WEB.BLL
{
	public class LawService
	{
		#region == BLL Methods ==

		public JavnaRasprava.WEB.Models.LawModel GetLawModel( int lawID, string userId, CommentOrder commentOrder )
		{
			var auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new JavnaRasprava.WEB.Models.LawModel();

				var law = context.Laws
					.Where( x => x.LawID == lawID )
					.Include( x => x.LawRepresentativeAssociations.Select( y => y.Representative.Party ) )
					.Include( x => x.ExpertComments.Select( y => y.Expert ) )
					.Include( x => x.LawSections )
					.Include( x => x.Procedure )
					.Include( x => x.Parliament )
					.Include( x => x.Category )
					.FirstOrDefault();

				if ( law == null )
					return null;

				result.Law = law;
				result.ExpertCommentsCount = law.ExpertComments.Count;

				// Calculating votes for law
				var lawVotingResults = context.LawVotes
												.Where( x => x.LawID == lawID )
												.GroupBy( x => x.Vote )
												.Select( g => new { Key = g.Key, Count = g.Count() } )
												.ToList();

				var unsafeResult = lawVotingResults.Where( x => x.Key.HasValue && x.Key.Value ).FirstOrDefault();
				result.VotesUp = unsafeResult == null ? 0 : unsafeResult.Count;

				unsafeResult = lawVotingResults.Where( x => x.Key.HasValue && !x.Key.Value ).FirstOrDefault();
				result.VotesDown = unsafeResult == null ? 0 : unsafeResult.Count;

				result.VotesDownPercentage = Infrastructure.Math.Percentage( result.VotesDown, result.VotesDown + result.VotesUp );
				result.VotesUpPercentage = Infrastructure.Math.Percentage( result.VotesUp, result.VotesDown + result.VotesUp );

				if ( userId != null )
					result.UserVoted = context.LawVotes.Any( x => x.ApplicationUserID == userId && x.LawID == lawID );
				else
					result.UserVoted = auService.HasVotedLaw( lawID );

				result.Sections = PopulateSectionmodel( law.LawSections.ToList(), law, context, userId );

				result.CommentsCount = context.LawComments.Where( x => x.LawID == lawID ).Count();

				result.Statistics = GetStatsForLaw( context, law.LawID, law.ParliamentID, law.CategoryId );

				return result;
			}
		}



		private Models.Law.LawStatModel GetStatsForLaw( ApplicationDbContext context, int lawId, int parliamentId, int categoryId )
		{
			var result = new Models.Law.LawStatModel
			{
				LawId = lawId
			};

			result.TotalQuestions = context.UserRepresentativeQuestions.Where( x => x.Question.LawID == lawId ).Count();

			var mostActiveRepresentative = context.Answers
				.Where( x => x.Representative.ParliamentHouse.ParliamentID == parliamentId )
				.GroupBy( x => new
				{
					RepID = x.RepresentativeID,
					RepFirstName = x.Representative.FirstName,
					RepLastName = x.Representative.LastName
				} )
				.OrderByDescending( x => x.Count() )
				.Select( g => new
				{
					RepID = g.Key.RepID,
					RepFirstName = g.Key.RepFirstName,
					RepLastName = g.Key.RepLastName,
					AnswersCount = g.Count()
				} )
				.FirstOrDefault();

			if ( mostActiveRepresentative != null )
			{
				result.MostActiveRepresentativeName = NameFormatter.GetDisplayName( mostActiveRepresentative.RepFirstName, mostActiveRepresentative.RepLastName );
				result.MostActiveRepresentativeId = mostActiveRepresentative.RepID;
				result.MostActiveRepresentativeCount = mostActiveRepresentative.AnswersCount;
			}

			var mostActiveLaw = context.UserRepresentativeQuestions
				.Where( x => x.Question.Law != null && x.Question.Law.ParliamentID == parliamentId )
				.Where( x => x.Question.Law != null && x.Question.Law.CategoryId == categoryId )
				.GroupBy( x => new { x.Question.LawID, x.Question.Law.Title } )
				.OrderByDescending( x => x.Count() )
				.Select( x => new
				{
					LawId = x.Key.LawID,
					Title = x.Key.Title,
					Count = x.Count()
				} )
				.FirstOrDefault();

			if ( mostActiveLaw != null )
			{
				result.MostActiveLawId = mostActiveLaw.LawId.HasValue ? mostActiveLaw.LawId.Value : 0;
				result.MostActiveLawTitle = mostActiveLaw.Title;
				result.MostActiveLawCount = mostActiveLaw.Count;
			}

			return result;

		}

		public Models.LawCustomVoteListModel GetLawCustomVotesList( int lawID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return GetLawCustomVotesListInternal( lawID, context );
			}
		}

		internal static LawCustomVoteListModel GetLawCustomVotesListInternal( int lawID, ApplicationDbContext context )
		{
			var result = new Models.LawCustomVoteListModel
			{
				LawID = lawID,
				LawCustomVotes = new List<DomainModels.LawCustomVote>()
			};

			result.LawCustomVotes.Add( new DomainModels.LawCustomVote { LawCustomVoteID = -3, Text = GlobalLocalization.Global_VoteUp, Vote = true } );
			result.LawCustomVotes.AddRange( context.LawCustomVotes.Where( x => x.LawID == lawID && x.Vote == true && x.IsSuggested ) );

			result.LawCustomVotes.Add( new DomainModels.LawCustomVote { LawCustomVoteID = -2, Text = GlobalLocalization.Global_VoteDown, Vote = false } );
			result.LawCustomVotes.AddRange( context.LawCustomVotes.Where( x => x.LawID == lawID && x.Vote == false && x.IsSuggested ) );

			return result;
		}

		public void VoteLaw( int lawID, string userId, string clientAddress, int customVoteId, string customVoteText )
		{
			AnonymousUserService auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				ApplicationUser user = null;

				if ( userId == null )
				{
					if ( auService.HasVotedLaw( lawID ) )
						return;

					user = auService.GetAnonymousUser( context );
				}

				else
				{
					user = context.Users.Where( x => x.Id == userId ).FirstOrDefault();
					if ( user == null )
						return;

					if ( context.LawVotes.Any( x => x.LawID == lawID && x.ApplicationUserID == userId ) )
						return;
				}


				var law = context.Laws.Where( x => x.LawID == lawID ).FirstOrDefault();
				if ( law == null )
					return;

				if ( customVoteId > 0 )
				{
					var customVoteFromDb = context.LawCustomVotes.Where( x => x.LawID == law.LawID && x.LawCustomVoteID == customVoteId ).FirstOrDefault();
					if ( customVoteFromDb == null )
						return;

					// Exiting custom vote
					context.LawVotes.Add( new DomainModels.LawVote
					{
						ApplicationUser = user,
						Law = law,
						LawCustomVote = customVoteFromDb,
						Vote = customVoteFromDb.Vote,
						Time = DateTime.UtcNow,
						ClientAddress = clientAddress
					} );
				}
				else if ( customVoteId == -1 ) // new custom vote
				{
					// check for some predefined values.
					if ( customVoteText.Trim().ToLower().Equals( GlobalLocalization.Global_VoteUp.ToLowerInvariant() ) )
					{
						customVoteId = -3;
					}
					else if ( customVoteText.Trim().ToLower().Equals( GlobalLocalization.Global_VoteDown.ToLowerInvariant() ) )
					{
						customVoteId = -2;
					}
					else
					{
						context.LawCustomVotes.Add( new LawCustomVote
						{
							AdminIgnore = false,
							Description = null,
							Text = customVoteText,
							Law = law,
							Vote = null,
							Votes = new List<LawVote>{new DomainModels.LawVote
						{
							Law = law,
							ApplicationUser = user,
							Vote = null
						} }
						} );
					}
				}


				if ( customVoteId == -2 ) // vote no
				{
					// TODO : Create notification
					context.LawVotes.Add( new DomainModels.LawVote
					{
						Law = law,
						ApplicationUser = user,
						Vote = false,
						Time = DateTime.UtcNow,
						ClientAddress = clientAddress
					} );
				}
				else if ( customVoteId == -3 ) // vote yes
				{
					// TODO : Create notification
					context.LawVotes.Add( new DomainModels.LawVote
					{
						Law = law,
						ApplicationUser = user,
						Vote = true,
						Time = DateTime.UtcNow,
						ClientAddress = clientAddress
					} );
				}

				context.SaveChanges();
				auService.VoteLaw( lawID, customVoteId, customVoteText );
			}
		}

		public Models.LawSectionCustomVoteListModel getLawSectionCustomVotesList( int lawID, int sectionID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return GetLawSectionCustomVotesListInternal( lawID, sectionID, context );
			}
		}

		internal static LawSectionCustomVoteListModel GetLawSectionCustomVotesListInternal( int lawID, int sectionID, ApplicationDbContext context )
		{

			var section = context.LawSections
				.Include( x => x.CustomVotes )
				.SingleOrDefault( x => x.LawSectionID == sectionID );

			if ( section == null )
				return null;

			var result = new Models.LawSectionCustomVoteListModel
			{
				LawSectionCustomVotes = new List<DomainModels.LawSectionCustomVote>(),
				LawSectionID = sectionID,
				LawID = lawID,
				Title = section.Title,
				Text = section.Text,
				Description = section.Description
			};

			result.LawSectionCustomVotes.Add( new DomainModels.LawSectionCustomVote { LawSectionCustomVoteID = -3, Text = GlobalLocalization.Global_VoteUp, Vote = true } );
			result.LawSectionCustomVotes.AddRange( section.CustomVotes.Where( x => x.Vote == true && x.IsSuggested ).OrderBy( x => x.Text ) );

			result.LawSectionCustomVotes.Add( new DomainModels.LawSectionCustomVote { LawSectionCustomVoteID = -2, Text = GlobalLocalization.Global_VoteDown, Vote = false } );
			result.LawSectionCustomVotes.AddRange( section.CustomVotes.Where( x => x.Vote == false && x.IsSuggested ).OrderBy( x => x.Text ) );

			return result;
		}

		public void VoteLawSection( int sectionID, string userId, string clientAddress, int customVoteId, string customVoteText )
		{
			AnonymousUserService auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				ApplicationUser user = null;

				if ( userId == null )
				{
					if ( auService.HasVotedLawSection( sectionID ) )
						return;

					user = auService.GetAnonymousUser( context );
				}
				else
				{
					user = context.Users.Where( x => x.Id == userId ).FirstOrDefault();
					if ( user == null )
						return;

					if ( context.LawSectionVotes.Any( x => x.LawSectionID == sectionID && x.ApplicationUserID == userId ) )
						return;
				}



				var section = context.LawSections.Where( x => x.LawSectionID == sectionID ).FirstOrDefault();
				if ( section == null )
					return;

				if ( customVoteId > 0 ) // Exiting custom vote
				{
					var customVoteFromDb = context.LawSectionCustomVotes.Where( x => x.LawSectionID == section.LawSectionID && x.LawSectionCustomVoteID == customVoteId ).FirstOrDefault();
					if ( customVoteFromDb == null )
						return;

					context.LawSectionVotes.Add( new DomainModels.LawSectionVote
					{
						ApplicationUser = user,
						LawSection = section,
						LawSectionCustomVote = customVoteFromDb,
						Vote = customVoteFromDb.Vote,
						Time = DateTime.UtcNow,
						ClientAddress = clientAddress
					} );
				}
				else if ( customVoteId == -1 ) // new custom vote
				{
					if ( customVoteText.Trim().ToLower().Equals( GlobalLocalization.Global_VoteUp.ToLowerInvariant() ) )
					{
						customVoteId = -3;
					}
					else if ( customVoteText.Trim().ToLower().Equals( GlobalLocalization.Global_VoteDown.ToLowerInvariant() ) )
					{
						customVoteId = -2;
					}
					else
					{
						context.LawSectionCustomVotes.Add( new LawSectionCustomVote
						{
							AdminIgnore = false,
							LawSection = section,
							Text = customVoteText,
							Vote = null,
							Votes = new List<LawSectionVote>{ new DomainModels.LawSectionVote
							{
								LawSection = section,
								ApplicationUser = user,
								Vote = null
							}}
						} );
					}
				}
				else if ( customVoteId == -2 ) // vote no
				{
					context.LawSectionVotes.Add( new DomainModels.LawSectionVote
					{
						LawSection = section,
						ApplicationUser = user,
						Vote = false,
						Time = DateTime.UtcNow,
						ClientAddress = clientAddress
					} );
				}
				else if ( customVoteId == -3 ) // vote yes
				{
					context.LawSectionVotes.Add( new DomainModels.LawSectionVote
					{
						LawSection = section,
						ApplicationUser = user,
						Vote = true,
						Time = DateTime.UtcNow,
						ClientAddress = clientAddress
					} );
				}

				context.SaveChanges();
				auService.VoteLawSection( sectionID, customVoteId, customVoteText );
			}
		}

		public Models.LawSummaryListModel GetLatestLaws( int take, int parliamentID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new Models.LawSummaryListModel
				{
					Laws = new List<Models.LawSummaryModel>()
				};

				result.Laws = context.Laws
					.Include( x => x.LawSections )
					.Where( x => x.ParliamentID == parliamentID && x.IsActive )
					.Take( take )
					.OrderByDescending( x => x.CreateDateTimeUtc )
					.SelectLawSummaryModel()
					.ToList();

				FixLawSummaryListModelPercentages( result.Laws );

				result.Count = result.Laws.Count;

				return result;
			}
		}

		public Models.LawSummaryListModel GetNextLawsInVote( int take, int parliamentID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new Models.LawSummaryListModel
				{
					Laws = new List<Models.LawSummaryModel>()
				};

				DateTime now = DateTime.UtcNow;

				result.Laws = context.Laws
					.Include( x => x.LawSections )
					.Where( x => x.ExpetedVotingDay >= now && x.ParliamentID == parliamentID && x.IsActive )
					.OrderBy( x => x.ExpetedVotingDay )
					.Take( take )
					.SelectLawSummaryModel()
					.ToList();

				FixLawSummaryListModelPercentages( result.Laws );
				result.Count = result.Laws.Count;

				return result;
			}
		}

		public Models.LawSummaryListModel GetMostActive( int take, int parliamentID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new Models.LawSummaryListModel
				{
					Laws = new List<Models.LawSummaryModel>()
				};

				var peroid = DateTime.UtcNow.AddDays( -30 );

				var ids = context.UserRepresentativeQuestions
					.Where( x => x.CreateTimeUtc > peroid && x.Question.Law.ParliamentID == parliamentID && x.Question.Law.IsActive )
					.GroupBy( x => x.Question.LawID )
					.Select( g => new { LawID = g.Key, Questioncount = g.Count() } )
					.OrderByDescending( g => g.Questioncount )
					.Take( take )
					.Select( g => g.LawID )
					.ToList();


				DateTime now = DateTime.UtcNow;

				var resultList = context.Laws
					.Include( x => x.LawSections )
					.Where( x => ids.Contains( x.LawID ) )
					.SelectLawSummaryModel()
					.ToList();

				result.Laws = new List<Models.LawSummaryModel>();

				foreach ( var id in ids )
				{
					result.Laws.Add( resultList.Single( x => x.ID == id ) );
				}

				FixLawSummaryListModelPercentages( result.Laws );
				result.Count = result.Laws.Count;

				return result;
			}
		}

		public Models.LawSearchModel InitializeSearchModel()
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return new Models.LawSearchModel
				{
					Categories = context.LawCategories.Select( x => new System.Web.Mvc.SelectListItem { Value = x.LawCategoryId.ToString(), Text = x.Title } ).ToList(),
					Parliaments = context.Parliaments.Select( x => new System.Web.Mvc.SelectListItem { Value = x.ParliamentID.ToString(), Text = x.Name } ).ToList()
				};
			}
		}

		public IPagedList<Models.LawSummaryModel> SearchLaw( Models.LawSearchModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var query = context.Laws
					.Include( x => x.LawSections )
					.Where( x => x.IsActive );

				if ( model.CategoryId > 0 )
					query = query.Where( x => x.CategoryId == model.CategoryId );

				if ( model.ParliamentId != 0 )
					query = query.Where( x => x.ParliamentID == model.ParliamentId );

				if ( model.QueryString != null )
					query = query.Where( x => x.Title.Contains( model.QueryString ) || x.Description.Contains( model.QueryString ) );

				var selectQuery = query
					.SelectLawSummaryModelForSearch()
					.ToList();

				IOrderedEnumerable<Models.LawSummaryModel> selected = null;

				switch ( ( model.SortBy + '_' + model.Order ).ToLower() )
				{
					case "createtime_asc":
						selected = selectQuery.OrderBy( x => x.CreateDateTimeUtc );
						break;
					case "createtime_desc":
						selected = selectQuery.OrderByDescending( x => x.CreateDateTimeUtc );
						break;
					case "votetime_asc":
						selected = selectQuery.OrderBy( x => x.ExpetedVotingDayDateTime );
						break;
					case "votetime_desc":
						selected = selectQuery.OrderByDescending( x => x.ExpetedVotingDayDateTime );
						break;
					case "title_asc":
						selected = selectQuery.OrderBy( x => x.Title );
						break;
					case "title_desc":
						selected = selectQuery.OrderByDescending( x => x.Title );
						break;
					case "askedcount_asc":
						selected = selectQuery.OrderBy( x => x.AskedCount );
						break;
					case "askedcount_desc":
						selected = selectQuery.OrderByDescending( x => x.AskedCount );
						break;
					case "answerscount_asc":
						selected = selectQuery.OrderBy( x => x.AnswersCount );
						break;
					case "answerscount_desc":
						selected = selectQuery.OrderByDescending( x => x.AnswersCount );
						break;
					case "votesup_asc":
						selected = selectQuery.OrderBy( x => x.VotesUp );
						break;
					case "votesup_desc":
						selected = selectQuery.OrderByDescending( x => x.VotesUp );
						break;
					case "votesdown_asc":
						selected = selectQuery.OrderBy( x => x.VotesDown );
						break;
					case "votesdown_desc":
						selected = selectQuery.OrderByDescending( x => x.VotesDown );
						break;
					case "none":
						break;
					default:
						selected = selectQuery.OrderByDescending( x => x.CreateDateTimeUtc );
						break;
				}



				var result = selected.ToPagedList( model.page.HasValue ? model.page.Value : 1, model.PageItemCount );

				FixLawSummaryListModelPercentages( result );

				return result;


			}
		}

		internal IPagedList<Models.LawSectionSummaryModel> SearchLawSections( LawSearchModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var query = context.LawSections
					.Include( x => x.Law )
					.Where( x => x.Law.IsActive );

				if ( model.CategoryId > 0 )
					query = query.Where( x => x.Law.CategoryId == model.CategoryId );

				if ( model.ParliamentId != 0 )
					query = query.Where( x => x.Law.ParliamentID == model.ParliamentId );

				if ( model.QueryString != null )
					query = query.Where( x => x.Title.Contains( model.QueryString ) || x.Description.Contains( model.QueryString ) );

				var selectQuery = query
					.SelectLawSummaryModelForSearch()
					.ToList();

				IOrderedEnumerable<Models.LawSectionSummaryModel> selected = null;

				switch ( ( model.SortBy + '_' + model.Order ).ToLower() )
				{
					case "createtime_asc":
						selected = selectQuery.OrderBy( x => x.LawCreateDateTimeUtc );
						break;
					case "createtime_desc":
						selected = selectQuery.OrderByDescending( x => x.LawCreateDateTimeUtc );
						break;
					case "votetime_asc":
						selected = selectQuery.OrderBy( x => x.LawExpetedVotingDayDateTime );
						break;
					case "votetime_desc":
						selected = selectQuery.OrderByDescending( x => x.LawExpetedVotingDayDateTime );
						break;
					case "title_asc":
						selected = selectQuery.OrderBy( x => x.Title );
						break;
					case "title_desc":
						selected = selectQuery.OrderByDescending( x => x.Title );
						break;
					case "askedcount_asc":
						selected = selectQuery.OrderBy( x => x.LawAskedCount );
						break;
					case "askedcount_desc":
						selected = selectQuery.OrderByDescending( x => x.LawAskedCount );
						break;
					case "votesup_asc":
						selected = selectQuery.OrderBy( x => x.VotesUp );
						break;
					case "votesup_desc":
						selected = selectQuery.OrderByDescending( x => x.VotesUp );
						break;
					case "votesdown_asc":
						selected = selectQuery.OrderBy( x => x.VotesDown );
						break;
					case "votesdown_desc":
						selected = selectQuery.OrderByDescending( x => x.VotesDown );
						break;
					case "none":
						break;
					default:
						selected = selectQuery.OrderByDescending( x => x.LawCreateDateTimeUtc );
						break;
				}



				var result = selected.ToPagedList( model.page.HasValue ? model.page.Value : 1, model.PageItemCount );

				FixLawSectionSummaryListModelPercentages( result );

				return result;


			}
		}

		public List<LawSectionSummaryModel> GetLawSectionTitles( int lawId )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.LawSections
					.Where( x => x.LawID == lawId )
					.Select( x => new LawSectionSummaryModel
					{
						LawSectionID = x.LawSectionID,
						Title = x.Title
					} )
					.ToList();
			}
		}



		public void PointOut( int lawID )
		{
			PointOut( lawID, DateTime.UtcNow );
		}

		public void PointOut( int lawID, DateTime? dateTime )
		{
			if ( dateTime == DateTime.MinValue )
				dateTime = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{

				var law = context.Laws.Where( x => x.LawID == lawID ).Update( x => new Law { PointedOutUtc = dateTime } );
				context.SaveChanges();
			}
		}

		public void PointOutSection( int lawSectionID )
		{
			PointOutSection( lawSectionID, DateTime.UtcNow );
		}

		public void PointOutSection( int lawSectionID, DateTime? dateTime )
		{
			if ( dateTime == DateTime.MinValue )
				dateTime = System.Data.SqlTypes.SqlDateTime.MinValue.Value;

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var law = context.LawSections.Where( x => x.LawSectionID == lawSectionID ).Update( x => new LawSection { PointedOutUtc = dateTime } );
				context.SaveChanges();
			}
		}

		public Models.LawCommunicaitonSummaryModel GetLawCommunicaitonSummary( int lawID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new Models.LawCommunicaitonSummaryModel
				{
					LawID = lawID
				};

				var law = context.Laws
					.Where( x => x.LawID == lawID )
					.Include( "Questions.UserRepresentativeQuestions.Representative" )
					.Include( "Questions.Answers" )
					.FirstOrDefault();
				if ( law == null )
					return null;

				var mostFrequentQuestion = law.Questions
					.Select( x => new { ID = x.QuestionID, Text = x.Text, AskedCount = x.UserRepresentativeQuestions.Count } )
					.OrderByDescending( x => x.AskedCount )
					.Where( x => x.AskedCount > 0 )
					.FirstOrDefault();

				if ( mostFrequentQuestion == null )
					return result;

				result.MostFrequentQuestionID = mostFrequentQuestion.ID;
				result.MostFrequentQuestionText = mostFrequentQuestion.Text;
				result.MostFrequentQuestionCount = mostFrequentQuestion.AskedCount;


				var representativeQuestions = law.Questions
					.SelectMany( x => x.UserRepresentativeQuestions )
					.GroupBy( x => new
					{
						RepID = x.RepresentativeID,
						RepFirstName = x.Representative.FirstName,
						RepLastName = x.Representative.LastName
					} )
					.Select( g => new
					{
						RepID = g.Key.RepID,
						RepFirstName = g.Key.RepFirstName,
						RepLastName = g.Key.RepLastName,
						AskedCount = g.Count()
					} );

				var representativeAnswers = law.Questions
					.SelectMany( x => x.Answers )
					.GroupBy( x => x.RepresentativeID )
					.Select( g => new
					{
						RepID = g.Key,
						Count = g.Count()
					} );



				var totals = from q in representativeQuestions
							 join a in representativeAnswers on q.RepID equals a.RepID into gj
							 from qa in gj.DefaultIfEmpty()
							 select new
							 {
								 RepID = q.RepID,
								 RepFirstName = q.RepFirstName,
								 RepLastName = q.RepLastName,
								 AskedCount = q.AskedCount,
								 AnswerCount = qa == null ? 0 : qa.Count,
								 Percent = ( ( qa == null ? 0 : qa.Count ) / q.AskedCount ) * 100
							 };

				var best = totals.OrderByDescending( x => x.Percent ).First();
				result.BestRepresentativeID = best.RepID;
				result.BestRepresentativeName = NameFormatter.GetDisplayName( best.RepFirstName, best.RepLastName );
				result.BestRepresentativeAnswerCount = best.AnswerCount;
				result.BestRepresentativeQuestionCount = best.AskedCount;


				var worst = totals.OrderBy( x => x.Percent ).First();
				result.WorstRepresentativeID = worst.RepID;
				result.WorstRepresentativeName = NameFormatter.GetDisplayName( worst.RepFirstName, worst.RepLastName );
				result.WorstRepresentativeAnswerCount = worst.AnswerCount;
				result.WorstRepresentativeQuestionCount = worst.AskedCount;


				return result;
			}
		}

		#endregion

		#region == Admin Methods ==

		public List<Models.Law.LawTitleModel> GetLawTitles( int parliamentID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.Laws
					.Where( x => x.ParliamentID == parliamentID )
					.Select( x => new Models.Law.LawTitleModel { LawID = x.LawID, Title = x.Title } )
					.ToList();
			}
		}

		public Models.Law.LawEditModel InitializeLawEditModel( int parliamentID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return InitializeLawEditModelInternal( parliamentID, context );
			}
		}

		public void InitializeExistingLawEditModel( Models.Law.LawEditModel model, int parliamentID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				InitializeExistingLawEditModelInternal( model, parliamentID, context );
			}
		}

		public Models.Law.LawEditModel GetLawEditModel( int lawID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetLawEditModelInternal( lawID, context );
			}
		}

		public Models.Law.LawEditModel UpdateLaw( Models.Law.LawEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Where( x => x.LawID == model.LawID )
					.FirstOrDefault();

				if ( law == null )
					return null;

				law.Description = model.Description;
				law.ExpetedVotingDay = model.ExpetedVotingDay;
				law.StatusTitle = model.StatusTitle;
				law.StatusText = model.StatusText;
				//law.PointedOutUtc=model.PointedOutUtc;
				law.ProcedureID = model.ProcedureID;
				law.IsActive = model.IsActive;
				law.Submitter = model.Submitter;
				law.Text = model.Text;
				law.Title = model.Title;
				law.CategoryId = model.CategoryId;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					law.ImageRelativePath = fileService.UploadFile( new List<string> { "LawBackground" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					law.ImageRelativePath = model.ImageRelativePath;
				}

				if ( model.TextFile != null )
				{
					var fileService = new FileService();
					law.TextFileRelativePath = fileService.UploadFile( new List<string> { "LawText" }, model.TextFile.FileName, model.TextFile.InputStream );
				}
				else
				{
					law.TextFileRelativePath = model.TextFileRelativePath;
				}
				context.SaveChanges();

				return GetLawEditModelInternal( model.LawID, context );
			}
		}

		public Models.Law.LawEditModel CreateLaw( Models.Law.LawEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = new Law();
				law.CreateDateTimeUtc = DateTime.UtcNow;
				law.Description = model.Description;
				law.ExpetedVotingDay = model.ExpetedVotingDay;
				law.StatusTitle = model.StatusTitle;
				law.StatusText = model.StatusText;
				law.ParliamentID = model.ParliamentID;
				//law.PointedOutUtc=model.PointedOutUtc;
				law.ProcedureID = model.ProcedureID;
				law.IsActive = model.IsActive;
				law.Submitter = model.Submitter;
				law.Text = model.Text;
				law.Title = model.Title;
				law.CategoryId = model.CategoryId;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					law.ImageRelativePath = fileService.UploadFile( new List<string> { "LawBackground" }, model.Image.FileName, model.Image.InputStream );

				}

				if ( model.TextFile != null )
				{
					var fileService = new FileService();
					law.TextFileRelativePath = fileService.UploadFile( new List<string> { "LawText" }, model.TextFile.FileName, model.TextFile.InputStream );

				}

				context.Laws.Add( law );
				context.SaveChanges();

				return GetLawEditModelInternal( law.LawID, context );
			}
		}



		public Models.Law.LawSectionEditModel InitializeLawSectionEditModel( int lawID )
		{
			return new Models.Law.LawSectionEditModel
			{
				LawID = lawID
			};
		}

		public Models.Law.LawSectionEditModel GetLawSectionEditModel( int lawSectionID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetLawSectionEditModelinternal( lawSectionID, context );
			}
		}

		public Models.Law.LawSectionEditModelList GetLawSectionEditModelList( int lawID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws
							.Where( x => x.LawID == lawID )
							.Include( "LawSections.CustomVotes" )
							.FirstOrDefault();

				if ( law == null )
					return null;

				return new Models.Law.LawSectionEditModelList
				{
					LawID = law.LawID,
					LawTitle = law.Title,
					Sections = law.LawSections.Select( x => TransfromLawSectionToEditModel( x ) ).ToList()
				};
			}
		}

		public Models.Law.LawSectionEditModel CreateLawSectionEditModel( Models.Law.LawSectionEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Where( x => x.LawID == model.LawID )
					.FirstOrDefault();

				if ( law == null )
					return null;

				var lawSection = new LawSection
				{
					LawID = model.LawID,
					Text = model.Text,
					Title = model.Title,
					Description = model.Description
				};

				if ( model.Image != null )
				{
					var fileService = new FileService();
					lawSection.ImageRelativePath = fileService.UploadFile( new List<string> { "LawSectionBackground" }, model.Image.FileName, model.Image.InputStream );

				}

				context.LawSections.Add( lawSection );
				context.SaveChanges();

				return GetLawSectionEditModelinternal( lawSection.LawSectionID, context );
			}
		}

		public Models.Law.LawSectionEditModel UpdateLawSectionEditModel( Models.Law.LawSectionEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var lawSection = context.LawSections
					.Where( x => x.LawSectionID == model.LawSectionID )
					.Include( x => x.CustomVotes )
					.FirstOrDefault();

				if ( lawSection == null )
					return null;

				lawSection.Text = model.Text;
				lawSection.Title = model.Title;
				lawSection.Description = model.Description;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					lawSection.ImageRelativePath = fileService.UploadFile( new List<string> { "LawSectionBackground" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					lawSection.ImageRelativePath = model.ImageRelativePath;
				}

				context.SaveChanges();

				return GetLawSectionEditModelinternal( model.LawSectionID, context );


			}
		}

		public bool DeleteLawSection( int lawSectionID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var section = context.LawSections.Where( x => x.LawSectionID == lawSectionID ).FirstOrDefault();

				if ( section == null )
					return false;

				new InfoBoxService().DeleteItems( lawSectionID, InfoBoxItemType.LawSection, context );

				context.LawSections.Remove( section );
				context.SaveChanges();

				return true;
			}
		}

		public Models.Law.LawSectionCustomVoteModel InitializeSectionCustomVoteEditModel( int sectionID, int lawID )
		{
			return new Models.Law.LawSectionCustomVoteModel
			{
				LawSectionID = sectionID,
				LawID = lawID
			};
		}

		public Models.Law.LawSectionCustomVoteModel GetLawSectionCustomVoteEditModel( int customVoteID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetLawSectionCustomVoteEditModelInternal( customVoteID, context );
			}
		}

		public Models.Law.LawSectionCustomVoteModel UpdateLawSectionCustomVote( Models.Law.LawSectionCustomVoteModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var customVote = context.LawSectionCustomVotes
					.Where( x => x.LawSectionCustomVoteID == model.LawSectionCustomVoteID )
					.Include( x => x.Votes )
					.FirstOrDefault();

				if ( customVote == null )
					return null;

				customVote.LawSectionCustomVoteID = customVote.LawSectionCustomVoteID;
				customVote.Text = model.Text;
				customVote.Description = model.Description;
				customVote.Vote = model.Vote;
				customVote.AdminIgnore = model.AdminIgnore;


				foreach ( var vote in customVote.Votes )
				{
					vote.Vote = customVote.Vote;
				}

				context.SaveChanges();

				return GetLawSectionCustomVoteEditModelInternal( model.LawSectionCustomVoteID, context );
			}


		}

		public Models.Law.LawSectionCustomVoteModel CreateLawSectionCustomVote( Models.Law.LawSectionCustomVoteModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var section = context.LawSections
					.Where( x => x.LawSectionID == model.LawSectionID )
					.FirstOrDefault();

				if ( section == null )
					return null;

				var customVote = new LawSectionCustomVote
				{
					Text = model.Text,
					Description = model.Description,
					Vote = model.Vote,
					LawSectionID = model.LawSectionID
				};

				context.LawSectionCustomVotes.Add( customVote );

				context.SaveChanges();

				return GetLawSectionCustomVoteEditModelInternal( model.LawSectionCustomVoteID, context );
			}
		}

		public bool DeleteLawSectionCustomVote( int voteId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var vote = context.LawSectionCustomVotes.FirstOrDefault( x => x.LawSectionCustomVoteID == voteId );

				if ( vote == null )
					return false;

				context.LawSectionCustomVotes.Remove( vote );
				context.LawSectionVotes.Where( x => x.LawSectionCustomVoteID == voteId ).Delete();
				context.SaveChanges();

				return true;
			}
		}

		public Models.Law.CreateExpertcommentModel InitializeCreateExpertCommentModel( int lawID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var result = new Models.Law.CreateExpertcommentModel
				{
					LawID = lawID,
					Experts = context.Experts.Select( x => new Models.Law.ExpertSummaryModel
					{
						ExpertID = x.ExpertID,
						FormattedName = x.FirstName + " " + x.LastName
					} ).ToList()
				};

				return result;
			}
		}

		public Models.Law.ExpertCommentEditModel CreateExpertComment( Models.Law.CreateExpertcommentModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws.Where( x => x.LawID == model.LawID ).FirstOrDefault();
				if ( law == null )
					return null;

				var expert = context.Experts.Where( x => x.ExpertID == model.ExpertID ).FirstOrDefault();
				if ( expert == null )
					return null;

				var expertComment = new ExpertComment
				{
					ExpertID = model.ExpertID,
					LawID = model.LawID,
					Text = model.Text
				};

				context.ExpertComments.Add( expertComment );
				context.SaveChanges();

				expertComment = context.ExpertComments
					.Where( x => x.ExpertCommentID == expertComment.ExpertCommentID )
					.Include( x => x.Expert )
					.FirstOrDefault();

				return new Models.Law.ExpertCommentEditModel
				{
					ExpertAbout = expertComment.Expert.About,
					Text = expertComment.Text,
					LawID = expertComment.LawID,
					ExpertLastName = expertComment.Expert.LastName,
					ExpertID = expertComment.ExpertID,
					ExpertFirstName = expertComment.Expert.FirstName,
					ExpertCommentID = expertComment.ExpertCommentID
				};

			}
		}

		public bool DeleteExpertComment( int expertCommentID )
		{

			using ( var context = ApplicationDbContext.Create() )
			{
				var expertComment = context.ExpertComments
					.Where( x => x.ExpertCommentID == expertCommentID )
					.FirstOrDefault();

				if ( expertComment == null )
					return false;

				context.ExpertComments.Remove( expertComment );
				context.SaveChanges();

				return true;
			}
		}

		public Models.Law.CreateLawRepresentativeModel InitializeCreateLawRepresentativeModel( int lawID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Where( x => x.LawID == lawID )
					.Include( x => x.LawRepresentativeAssociations )
					.FirstOrDefault();

				if ( law == null )
					return null;

				var representativeIds = law.LawRepresentativeAssociations.Select( x => x.RepresentativeID ).ToList();

				var model = new Models.Law.CreateLawRepresentativeModel
				{
					LawID = lawID,
					ParliamentID = law.ParliamentID,
					Representatives = context.Representatives
									  .Include( x => x.Party )
									  .Where( x => x.ParliamentHouse.ParliamentID == law.ParliamentID )
									  .Where( x => !representativeIds.Contains( x.RepresentativeID ) )
									  .ToList()
									  .Select( x => new Models.Law.RepresentativeSummaryModel
									  {
										  RepresentativeID = x.RepresentativeID,
										  FormattedName = x.DisplayName + ", " + x.Party.Name
									  } )
									  .OrderBy( x => x.FormattedName )
									  .ToList()
				};
				return model;
			}
		}

		public Models.Law.LawRepresentativeModel AddLawRepresentative( Models.Law.CreateLawRepresentativeModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Where( x => x.LawID == model.LawID )
					.Include( x => x.LawRepresentativeAssociations )
					.FirstOrDefault();

				if ( law == null )
					return null;

				if ( law.LawRepresentativeAssociations.Any( x => x.RepresentativeID == model.RepresentativeID ) )
					return null;

				var representative = context.Representatives.Where( x => x.RepresentativeID == model.RepresentativeID ).FirstOrDefault();
				if ( representative == null )
					return null;

				var lawRepresentative = new LawRepresentativeAssociation
				{
					LawID = model.LawID,
					RepresentativeID = model.RepresentativeID,
					Reason = model.Reason
				};

				context.LawRepresentativeAssociations.Add( lawRepresentative );
				context.SaveChanges();

				lawRepresentative = context.LawRepresentativeAssociations
					.Where( x => x.LawRepresentativeAssociationID == lawRepresentative.LawRepresentativeAssociationID )
					.Include( x => x.Representative.Party )
					.FirstOrDefault();

				var result = new Models.Law.LawRepresentativeModel
				{
					FirstName = lawRepresentative.Representative.FirstName,
					LastName = lawRepresentative.Representative.LastName,
					LawRepresentativeAssociationID = lawRepresentative.LawRepresentativeAssociationID,
					PartyName = lawRepresentative.Representative.Party.Name,
					Reason = lawRepresentative.Reason,
					RepresentativeID = lawRepresentative.RepresentativeID
				};

				return result;

			}
		}

		public bool DeleteLawRepresentative( int lawRepresentativeAssociationID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var lawRepresentative = context.LawRepresentativeAssociations
					.Where( x => x.LawRepresentativeAssociationID == lawRepresentativeAssociationID )
					.FirstOrDefault();

				if ( lawRepresentative == null )
					return false;

				context.LawRepresentativeAssociations.Remove( lawRepresentative );
				context.SaveChanges();

				return true;
			}
		}

		public Models.Law.CustomVoteEditModel InitializeCustomVoteEditModel( int lawID )
		{
			return new Models.Law.CustomVoteEditModel
			{
				LawID = lawID
			};
		}

		public Models.Law.CustomVoteEditModel GetCustomVoteEditModel( int customVoteID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var customVote = context.LawCustomVotes
					.Where( x => x.LawCustomVoteID == customVoteID )
					.FirstOrDefault();

				if ( customVote == null )
					return null;

				return new Models.Law.CustomVoteEditModel
				{
					LawCustomVoteID = customVote.LawCustomVoteID,
					LawID = customVote.LawID,
					Text = customVote.Text,
					Description = customVote.Description,
					Vote = customVote.Vote,
					AdminIgnore = customVote.AdminIgnore,
					IsSuggested = customVote.IsSuggested
				};
			}
		}

		public Models.Law.CustomVoteEditModel UpdateCustomVote( Models.Law.CustomVoteEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var customVote = context.LawCustomVotes
					.Where( x => x.LawCustomVoteID == model.LawCustomVoteID )
					.Include( x => x.Votes )
					.FirstOrDefault();

				if ( customVote == null )
					return null;

				customVote.LawCustomVoteID = customVote.LawCustomVoteID;
				customVote.Text = model.Text;
				customVote.Description = model.Description;
				customVote.Vote = model.Vote;
				customVote.AdminIgnore = model.AdminIgnore;
				customVote.IsSuggested = model.IsSuggested;

				foreach ( var vote in customVote.Votes )
				{
					vote.Vote = customVote.Vote;
				}

				context.SaveChanges();
			}

			return GetCustomVoteEditModel( model.LawCustomVoteID );
		}

		public Models.Law.CustomVoteEditModel CreateCustomVote( Models.Law.CustomVoteEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Where( x => x.LawID == model.LawID )
					.FirstOrDefault();

				if ( law == null )
					return null;

				var customVote = new LawCustomVote
				{
					Text = model.Text,
					Description = model.Description,
					Vote = model.Vote,
					LawID = model.LawID,
					IsSuggested = model.IsSuggested
				};

				context.LawCustomVotes.Add( customVote );

				context.SaveChanges();
			}

			return GetCustomVoteEditModel( model.LawCustomVoteID );
		}

		public bool DeleteCustomVote( int voteId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var vote = context.LawCustomVotes.FirstOrDefault( x => x.LawCustomVoteID == voteId );

				if ( vote == null )
					return false;

				context.LawCustomVotes.Remove( vote );
				context.LawVotes.Where( x => x.LawCustomVoteID == voteId ).Delete();

				context.SaveChanges();

				return true;
			}
		}

		internal static Models.Law.LawSectionCustomVoteModel GetLawSectionCustomVoteEditModelInternal( int customVoteID, ApplicationDbContext context )
		{
			var customVote = context.LawSectionCustomVotes
							.Where( x => x.LawSectionCustomVoteID == customVoteID )
							.Include( x => x.LawSection )
							.FirstOrDefault();

			if ( customVote == null )
				return null;

			return new Models.Law.LawSectionCustomVoteModel
			{
				LawSectionCustomVoteID = customVote.LawSectionCustomVoteID,
				LawSectionID = customVote.LawSectionID,
				Text = customVote.Text,
				Description = customVote.Description,
				Vote = customVote.Vote,
				LawID = customVote.LawSection.LawID,
				IsSuggested = customVote.IsSuggested
			};
		}

		public List<Models.Law.CustomVoteEditModel> GetUnverifiedCustomVotes()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.LawCustomVotes
					.Where( x => !x.AdminIgnore && !x.Vote == null )
					.ToList()
					.Select( x => new Models.Law.CustomVoteEditModel
					{
						AdminIgnore = x.AdminIgnore,
						Description = x.Description,
						LawCustomVoteID = x.LawCustomVoteID,
						LawID = x.LawID,
						Text = x.Text,
						Vote = x.Vote,
						IsSuggested = x.IsSuggested
					} )
					.ToList();
			}
		}

		public int GetUnverifiedCustomVotesCount()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.LawCustomVotes
					.Where( x => !x.AdminIgnore && !x.Vote == null )
					.Count();
			}
		}

		public List<Models.Law.LawSectionCustomVoteModel> GetUnverifiedLawSectionCustomVotes()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.LawSectionCustomVotes
					.Where( x => !x.AdminIgnore && !x.Vote == null )
					.Include( x => x.LawSection )
					.ToList()
					.Select( x => new Models.Law.LawSectionCustomVoteModel
					{
						AdminIgnore = x.AdminIgnore,
						Description = x.Description,
						LawID = x.LawSection.LawID,
						LawSectionCustomVoteID = x.LawSectionCustomVoteID,
						LawSectionID = x.LawSectionID,
						Text = x.Text,
						Vote = x.Vote
					} )
					.ToList();
			}
		}

		public int GetUnverifiedLawSectionCustomVotesCount()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.LawSectionCustomVotes
					.Where( x => !x.AdminIgnore && !x.Vote == null )
					.Count();
			}
		}

		internal static Models.Law.LawEditModel InitializeLawEditModelInternal( int parliamentID, ApplicationDbContext context )
		{
			var model = new Models.Law.LawEditModel();
			InitializeExistingLawEditModelInternal( model, parliamentID, context );
			return model;
		}
		internal static void InitializeExistingLawEditModelInternal( Models.Law.LawEditModel model, int parliamentID, ApplicationDbContext context )
		{
			model.Procedures = context.Procedures
						   .Select( x => new Models.Law.ProcedureEditModel { ProcedureID = x.ProcedureID, Title = x.Title, Description = x.Description } )
						   .ToList();
			model.ParliamentID = parliamentID;
			model.Categories = context.LawCategories
							.Select( x => new Models.Law.LawCategoryEditModel { LawCategoryId = x.LawCategoryId, Title = x.Title } )
							.ToList();
		}

		internal static Models.Law.LawSectionEditModel GetLawSectionEditModelinternal( int lawSectionID, ApplicationDbContext context )
		{
			var lawSection = context.LawSections
							.Where( x => x.LawSectionID == lawSectionID )
							.Include( x => x.CustomVotes )
							.FirstOrDefault();

			if ( lawSection == null )
				return null;

			return TransfromLawSectionToEditModel( lawSection );
		}

		private static Models.Law.LawSectionEditModel TransfromLawSectionToEditModel( LawSection lawSection )
		{
			return new Models.Law.LawSectionEditModel
			{
				LawSectionID = lawSection.LawSectionID,
				Text = lawSection.Text,
				Title = lawSection.Title,
				Description = lawSection.Description,
				LawID = lawSection.LawID,
				ImageRelativePath = lawSection.ImageRelativePath,
				LawSectionVotes = lawSection.CustomVotes.Select( x => new Models.Law.LawSectionCustomVoteModel
				{
					LawSectionCustomVoteID = x.LawSectionCustomVoteID,
					LawSectionID = x.LawSectionID,
					Description = x.Description,
					Text = x.Text,
					Vote = x.Vote,
					IsSuggested = x.IsSuggested
				} )
				.ToList()
			};
		}

		private static Models.Law.LawEditModel GetLawEditModelInternal( int lawID, ApplicationDbContext context )
		{
			var law = context.Laws
							.Where( x => x.LawID == lawID )
							.Include( x => x.LawRepresentativeAssociations.Select( y => y.Representative.Party ) )
							.Include( x => x.ExpertComments.Select( y => y.Expert ) )
							.Include( x => x.CustomVotes )
							.Include( x => x.LawSections )
							.Include( x => x.Procedure )
							.Include( x => x.Parliament )
							.Include( x => x.Questions )
							.Include( x => x.Category )
							.FirstOrDefault();

			if ( law == null )
				return null;

			var model = InitializeLawEditModelInternal( law.ParliamentID, context );

			model.LawID = law.LawID;
			model.CreateDateTimeUtc = law.CreateDateTimeUtc;
			model.Description = law.Description;
			model.ExpetedVotingDay = law.ExpetedVotingDay;
			model.StatusText = law.StatusText;
			model.StatusTitle = law.StatusTitle;
			model.ParliamentID = law.ParliamentID;
			model.ParliamentName = law.Parliament.Name;
			model.PointedOutUtc = law.PointedOutUtc;
			model.PointedOut = law.PointedOutUtc.HasValue;
			model.ProcedureID = law.ProcedureID;
			model.ProcedureName = law.Procedure.Title;
			model.IsActive = law.IsActive;
			model.Submitter = law.Submitter;
			model.Text = law.Text;
			model.Title = law.Title;
			model.ImageRelativePath = law.ImageRelativePath;
			model.TextFileRelativePath = law.TextFileRelativePath;
			model.CategoryId = law.CategoryId;
			model.CategoryTitle = law.Category.Title;

			model.ExpertComments = law.ExpertComments
									.Select( x => new Models.Law.ExpertCommentEditModel
									{
										ExpertAbout = x.Expert.About,
										ExpertCommentID = x.ExpertCommentID,
										ExpertFirstName = x.Expert.FirstName,
										ExpertID = x.ExpertID,
										ExpertLastName = x.Expert.LastName,
										LawID = law.LawID,
										Text = x.Text
									} )
									.ToList();

			model.Representatives = law.LawRepresentativeAssociations
									.Select( x => new Models.Law.LawRepresentativeModel
									{
										FirstName = x.Representative.FirstName,
										LastName = x.Representative.LastName,
										LawRepresentativeAssociationID = x.LawRepresentativeAssociationID,
										PartyName = x.Representative.Party.Name,
										Reason = x.Reason,
										RepresentativeID = x.RepresentativeID
									} )
									.ToList();

			model.CustomVotes = law.CustomVotes
								.Select( x => new Models.Law.CustomVoteEditModel
								{
									Description = x.Description,
									LawCustomVoteID = x.LawCustomVoteID,
									LawID = law.LawID,
									Text = x.Text,
									Vote = x.Vote,
									IsSuggested = x.IsSuggested
								} )
								.ToList();

			model.Sections = law.LawSections.Select( x => new Models.Law.LawSectionSummaryEditModel
			{
				LawSectionId = x.LawSectionID,
				Title = x.Title
			} )
			.ToList();

			model.Questions = law.Questions.Select( x => new Models.Law.PredefinedQuestionEditModel
			{
				AdminIgnore = x.AdminIgnore,
				CreateTimeUtc = x.CreateTimeUtc,
				IsSuggested = x.IsSuggested,
				LawID = x.LawID.Value,
				QuestionID = x.QuestionID,
				Text = x.Text,
				Verified = x.Verified
			} )
			.ToList();

			return model;
		}

		private static Models.Law.PredefinedQuestionEditModel GetPredefinedQuestionEditModelInternal( int questionID, ApplicationDbContext context )
		{
			var question = context.Questions
							.Where( x => x.QuestionID == questionID )
							.Include( x => x.Law )
							.Include( "UserRepresentativeQuestions.Representative.Party" )
							.FirstOrDefault();

			if ( question == null )
				return null;

			return new Models.Law.PredefinedQuestionEditModel
			{

				AdminIgnore = question.AdminIgnore,
				CreateTimeUtc = question.CreateTimeUtc,
				IsSuggested = question.IsSuggested,
				LawID = question.LawID.Value,
				QuestionID = question.QuestionID,
				Text = question.Text,
				Verified = question.Verified,
				AskedRepresentatives = question.UserRepresentativeQuestions.Select( x => string.Format( "{0}, {1}", x.Representative.DisplayName, x.Representative.Party.Name ) ).ToList()
			};
		}

		public Models.Law.PredefinedQuestionEditModel InitializePredefinedQuestionEditModel( int lawID )
		{
			return new Models.Law.PredefinedQuestionEditModel
			{
				LawID = lawID
			};
		}

		public JavnaRasprava.WEB.Models.Law.PredefinedQuestionEditModel GetPredefinedQuestionEditModel( int questionID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetPredefinedQuestionEditModelInternal( questionID, context );
			}
		}

		public WEB.Models.Law.PredefinedQuestionEditModel UpdatePredefinedQuestionEditModel( WEB.Models.Law.PredefinedQuestionEditModel model )
		{
			var newQuestions = new List<DomainModels.UserRepresentativeQuestion>();
			using ( var context = ApplicationDbContext.Create() )
			{
				var question = context.Questions
					.Where( x => x.QuestionID == model.QuestionID )
					.FirstOrDefault();

				if ( question == null )
					return null;

				question.AdminIgnore = model.AdminIgnore;
				question.IsSuggested = model.IsSuggested;
				question.Text = model.Text;
				question.Verified = model.Verified;

				if ( model.Verified )
				{
					newQuestions.AddRange( context.UserRepresentativeQuestions.Where( x => x.QuestionID == question.QuestionID ).ToList() );
				}

				context.SaveChanges();

				new NotificationService().ProcessNewQuestions( newQuestions );

				return GetPredefinedQuestionEditModelInternal( model.QuestionID, context );
			}

		}

		public WEB.Models.Law.PredefinedQuestionEditModel CreatePredefinedQuestionEditModel( WEB.Models.Law.PredefinedQuestionEditModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var question = new Question();
				question.AdminIgnore = false;
				question.CreateTimeUtc = DateTime.UtcNow;
				question.IsSuggested = true;
				question.LawID = model.LawID;
				question.Text = model.Text;
				question.Verified = true;

				context.Questions.Add( question );
				context.SaveChanges();

				return GetPredefinedQuestionEditModelInternal( question.QuestionID, context );
			}
		}

		public bool DeletePredefinedQuestionEditModel( int questionID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var question = context.Questions
					.Where( x => x.QuestionID == questionID )
					.FirstOrDefault();

				if ( question == null )
					return false;

				context.AnswerTokens
					.Where( x => x.QuestionID == questionID )
					.Delete();

				context.Questions.Remove( question );
				context.SaveChanges();

				return true;
			}
		}

		public List<WEB.Models.Law.PredefinedQuestionEditModel> GetUnverifiedQuestions()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.Questions
					.Where( x => !x.Verified && !x.AdminIgnore && !x.IsSuggested )
					.Where( x => x.Law != null )
					.Include( x => x.Law )
					.ToList()
					.Select( x => new WEB.Models.Law.PredefinedQuestionEditModel
					{
						AdminIgnore = x.AdminIgnore,
						CreateTimeUtc = x.CreateTimeUtc,
						IsSuggested = x.IsSuggested,
						LawID = x.LawID == null ? 0 : x.LawID.Value,
						LawTitle = x.Law == null ? "Direktno pitanje" : x.Law.Title,
						IsDirectQuestion = x.Law == null,
						QuestionID = x.QuestionID,
						Text = x.Text,
						Verified = x.Verified
					} )
					.ToList();
			}
		}

		public int GetUnverifiedQuestionsCount()
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.Questions
					.Where( x => !x.Verified && !x.AdminIgnore && !x.IsSuggested )
					.Where( x => x.Law != null )
					.Count();
			}
		}

		public List<WEB.Models.Law.RepresentativeQuestionEditModel> GetUnverifiedRepresentativeQuestions( int? representativeId = null )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.UserRepresentativeQuestions
					.Where( x => !x.Question.Verified && !x.Question.AdminIgnore && !x.Question.IsSuggested )
					.Where( x => x.Question.Law == null )
					.Where( x => !representativeId.HasValue || x.RepresentativeID == representativeId.Value )
					.Include( x => x.Representative )
					.Include( x => x.Question )
					.ToList()
					.Select( x => new WEB.Models.Law.RepresentativeQuestionEditModel
					{
						AdminIgnore = x.Question.AdminIgnore,
						CreateTimeUtc = x.Question.CreateTimeUtc,
						QuestionID = x.Question.QuestionID,
						Text = x.Question.Text,
						Verified = x.Question.Verified,
						RepresentativeId = x.RepresentativeID,
						RperesentativeName = x.Representative.DisplayName
					} )
					.ToList();
			}
		}

		public int GetUnverifiedRepresentativeQuestionsCount( int? representativeId = null )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.UserRepresentativeQuestions
					.Where( x => !x.Question.Verified && !x.Question.AdminIgnore && !x.Question.IsSuggested )
					.Where( x => x.Question.Law == null )
					.Where( x => !representativeId.HasValue || x.RepresentativeID == representativeId.Value )
					.Count();
			}
		}

		public WEB.Models.Law.RepresentativeQuestionEditModel GetRepresentativeQuestionEditModel( int questionId )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return context.UserRepresentativeQuestions
					.Where( x => x.QuestionID == questionId )
					.Where( x => x.Question.Law == null )
					.Include( x => x.Representative.ParliamentHouse.Parliament )
					.Include( x => x.Question )
					.ToList()
					.Select( x => new WEB.Models.Law.RepresentativeQuestionEditModel
					{
						AdminIgnore = x.Question.AdminIgnore,
						CreateTimeUtc = x.Question.CreateTimeUtc,
						QuestionID = x.Question.QuestionID,
						Text = x.Question.Text,
						Verified = x.Question.Verified,
						RepresentativeId = x.RepresentativeID,
						RperesentativeName = x.Representative.DisplayName,
						ParliamentId = x.Representative.ParliamentHouse.ParliamentID,
						ParliamentName = x.Representative.ParliamentHouse.Parliament.Name
					} )
					.FirstOrDefault();
			}
		}

		public bool UpdateRepresentativeQuestionEditModel( WEB.Models.Law.RepresentativeQuestionEditModel model )
		{
			var newQuestions = new List<DomainModels.UserRepresentativeQuestion>();
			using ( var context = ApplicationDbContext.Create() )
			{
				var question = context.Questions.SingleOrDefault( x => x.QuestionID == model.QuestionID );

				if ( question == null )
					return false;

				question.Text = model.Text;
				question.Verified = model.Verified;
				question.AdminIgnore = model.AdminIgnore;

				if ( model.Verified )
				{
					newQuestions.AddRange( context.UserRepresentativeQuestions.Where( x => x.QuestionID == question.QuestionID ).ToList() );
				}

				context.SaveChanges();

				new NotificationService().ProcessNewQuestions( newQuestions );
				return true;
			}
		}

		#endregion

		#region == Private business methods ==

		private void FixLawSectionSummaryListModelPercentages( IPagedList<LawSectionSummaryModel> models )
		{
			foreach ( var section in models )
			{
				section.VotesDownPercentage = Infrastructure.Math.Percentage( section.VotesDown, section.VotesDown + section.VotesUp );
				section.VotesUpPercentage = Infrastructure.Math.Percentage( section.VotesUp, section.VotesDown + section.VotesUp );
			}
		}

		private static void FixLawSummaryListModelPercentages( IEnumerable<Models.LawSummaryModel> models )
		{
			foreach ( var law in models )
			{
				law.VotesDownPercentage = Infrastructure.Math.Percentage( law.VotesDown, law.VotesDown + law.VotesUp );
				law.VotesUpPercentage = Infrastructure.Math.Percentage( law.VotesUp, law.VotesDown + law.VotesUp );

				if ( law.Sections == null || law.Sections.Count() == 0 )
					continue;

				foreach ( var section in law.Sections )
				{
					section.VotesDownPercentage = Infrastructure.Math.Percentage( section.VotesDown, section.VotesDown + section.VotesUp );
					section.VotesUpPercentage = Infrastructure.Math.Percentage( section.VotesUp, section.VotesDown + section.VotesUp );
				}
			}


		}



		private ICollection<Models.LawSectionModel> PopulateSectionmodel( List<DomainModels.LawSection> sections, DomainModels.Law law, DomainModels.ApplicationDbContext context, string userId )
		{
			var auService = new AnonymousUserService();
			//var sectionVotingResults = context.LawSectionVotes
			//									.Where( x => x.Law == law )
			//									.GroupBy( x => new { x.LawSectionID, x.Vote } )
			//									.Select( g => new { SectionID = g.Key.LawSectionID, Vote = g.Key.Vote, Count = g.Count() } )
			//									.ToList();

			var sectionVotingResults = context.LawSections
										.Where( x => x.LawID == law.LawID )
										.SelectMany( x => x.LawSectionVotes )
										.GroupBy( x => new { x.LawSectionID, x.Vote } )
										.Select( g => new { SectionID = g.Key.LawSectionID, Vote = g.Key.Vote, Count = g.Count() } )
										.ToList();

			var results = new List<Models.LawSectionModel>();

			foreach ( var section in sections )
			{
				var result = new Models.LawSectionModel
				{
					LawSection = section,
					ImageRelativePath = section.ImageRelativePath
				};

				var nullableResult = sectionVotingResults.Where( x => x.SectionID == section.LawSectionID && x.Vote == true ).FirstOrDefault();
				result.VotesUp = nullableResult == null ? 0 : nullableResult.Count;

				nullableResult = sectionVotingResults.Where( x => x.SectionID == section.LawSectionID && x.Vote == false ).FirstOrDefault();
				result.VotesDown = nullableResult == null ? 0 : nullableResult.Count;

				result.VotesDownPercentage = Infrastructure.Math.Percentage( result.VotesDown, result.VotesDown + result.VotesUp );
				result.VotesUpPercentage = Infrastructure.Math.Percentage( result.VotesUp, result.VotesDown + result.VotesUp );


				// TODO : optimize this to a single DB call as in comments service
				if ( userId != null )
					result.UserVoted = context.LawSectionVotes.Any( x => x.ApplicationUserID == userId && x.LawSectionID == section.LawSectionID );
				else
					result.UserVoted = auService.HasVotedLawSection( section.LawSectionID );

				results.Add( result );
			}

			return results;
		}


		#endregion

		#region == Private Helper Methods ==

		#endregion

		public void DeleteLaw( int lawID )
		{

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{

				var law = context.Laws
					.Include( x => x.LawSections )
					.SingleOrDefault( x => x.LawID == lawID );
				if ( law == null )
					return;

				var lawQuestionPairs = context.Laws
					.Include( x => x.LawSections )
					.Where( x => x.LawID == lawID )
					.Select( x => new { LawID = x.LawID, QuestionIDs = x.Questions.Select( y => y.QuestionID ) } )
					.FirstOrDefault();
				if ( lawQuestionPairs == null )
					return;

				context.Questions
					.Where( x => x.LawID == lawID )
					.ToList()
					.ForEach( x => context.Questions.Remove( x ) );

				context.UserRepresentativeQuestions
					.Where( x => lawQuestionPairs.QuestionIDs.Contains( x.QuestionID ) )
					.ToList()
					.ForEach( x => context.UserRepresentativeQuestions.Remove( x ) );

				context.AnswerTokens
					.Where( x => lawQuestionPairs.QuestionIDs.Contains( x.QuestionID ) )
					.ToList()
					.ForEach( x => context.AnswerTokens.Remove( x ) );

				var boxService = new InfoBoxService();
				foreach ( var section in law.LawSections )
					boxService.DeleteItems( section.LawSectionID, InfoBoxItemType.LawSection, context );

				boxService.DeleteItems( lawID, InfoBoxItemType.Law, context );

				context.Laws
					.Remove( law );

				context.SaveChanges();
			}

		}

		#region == Info box ==

		public Models.LawSummaryListModel GetInfoBoxLaws( int parliamentID )
		{
			InfoBoxService ibService = new InfoBoxService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var items = ibService.GetItemsForInfoBox( "Law", parliamentID ).Select( x => x.Reference ).ToList();

				return GetLawSummaryListModelInternal( context, items );
			}
		}



		public Models.LawSectionSummaryModelCollection GetInfoBoxLawSections( int parliamentID )
		{
			InfoBoxService ibService = new InfoBoxService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var items = ibService.GetItemsForInfoBox( "LawSection", parliamentID ).Select( x => x.Reference ).ToList();

				return GetLawSectionSummaryModelCollectionInternal( context, items );
			}
		}

		internal TopInfoBoxCollectionModel GetInfoBoxTop( int parliamentId )
		{
			InfoBoxService ibService = new InfoBoxService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var items = ibService.GetItemsForInfoBox( "Top", parliamentId );

				var sections = GetLawSectionSummaryModelCollectionInternal( context,
					items.Where( x => x.Type == InfoBoxItemType.LawSection ).Select( x => x.Reference ).ToList() );

				var laws = GetLawSummaryListModelInternal( context,
					items.Where( x => x.Type == InfoBoxItemType.Law ).Select( x => x.Reference ).ToList() );

				var quizList = QuizService.GetQuizSummaryModelListInternal( context,
					items.Where( x => x.Type == InfoBoxItemType.Quiz ).Select( x => x.Reference ).ToList() );

				var newsList = NewsService.GetNewsListInternal( context,
					items.Where( x => x.Type == InfoBoxItemType.News ).Select( x => x.Reference ).ToList() );

				var result = new TopInfoBoxCollectionModel
				{
					Items = new List<TopInfoBoxItemModel>()
				};

				foreach ( var item in items )
				{
					switch ( item.Type )
					{
						case InfoBoxItemType.Law:
							var law = laws.Laws.SingleOrDefault( x => x.ID == item.Reference );
							result.Items.Add( new TopInfoBoxItemModel( law, InfoBoxItemType.Law ) );
							break;
						case InfoBoxItemType.LawSection:
							var section = sections.Sections.SingleOrDefault( x => x.LawSectionID == item.Reference );
							result.Items.Add( new TopInfoBoxItemModel( section, InfoBoxItemType.LawSection ) );
							break;
						case InfoBoxItemType.Quiz:
							var quiz = quizList.SingleOrDefault( x => x.QuizId == item.Reference );
							result.Items.Add( new TopInfoBoxItemModel( quiz, InfoBoxItemType.Quiz ) );
							break;
						case InfoBoxItemType.News:
							var news = newsList.SingleOrDefault( x => x.NewsId == item.Reference );
							result.Items.Add( new TopInfoBoxItemModel( news, InfoBoxItemType.News ) );
							break;
					}
				}

				return result;
			}
		}


		private static LawSectionSummaryModelCollection GetLawSectionSummaryModelCollectionInternal( ApplicationDbContext context, List<int> items )
		{
			var result = new Models.LawSectionSummaryModelCollection
			{
				Sections = new List<Models.LawSectionSummaryModel>()
			};

			result.Sections = context.LawSections
				.Include( x => x.Law )
				.Where( x => items.Contains( x.LawSectionID ) )
				.Select( x => new Models.LawSectionSummaryModel
				{
					ImageRelativePath = x.ImageRelativePath,
					Title = x.Title,
					Text = x.Text,
					Description = x.Description,
					LawSectionID = x.LawSectionID,
					LawID = x.LawID,
					LawTitle = x.Law.Title
				} )
				.ToList()
				.OrderBy( x => items.IndexOf( x.LawSectionID ) )
				.ToList();

			var sectionIds = result.Sections.Select( x => x.LawSectionID ).ToList();

			var sectionVotingResults = context.LawSections
										.Where( x => sectionIds.Contains( x.LawSectionID ) )
										.SelectMany( x => x.LawSectionVotes )
										.GroupBy( x => new { x.LawSectionID, x.Vote } )
										.Select( g => new { SectionID = g.Key.LawSectionID, Vote = g.Key.Vote, Count = g.Count() } )
										.ToList();


			foreach ( var section in result.Sections )
			{
				var nullableResult = sectionVotingResults.Where( x => x.SectionID == section.LawSectionID && x.Vote == true ).FirstOrDefault();
				section.VotesUp = nullableResult == null ? 0 : nullableResult.Count;

				nullableResult = sectionVotingResults.Where( x => x.SectionID == section.LawSectionID && x.Vote == false ).FirstOrDefault();
				section.VotesDown = nullableResult == null ? 0 : nullableResult.Count;

				section.VotesDownPercentage = Infrastructure.Math.Percentage( section.VotesDown, section.VotesDown + section.VotesUp );
				section.VotesUpPercentage = Infrastructure.Math.Percentage( section.VotesUp, section.VotesDown + section.VotesUp );
			}

			return result;
		}

		private static LawSummaryListModel GetLawSummaryListModelInternal( ApplicationDbContext context, List<int> items )
		{
			var result = new Models.LawSummaryListModel
			{
				Laws = new List<Models.LawSummaryModel>()
			};

			result.Laws = context.Laws
				.Include( x => x.LawSections )
				.Where( x => items.Contains( x.LawID ) )
				.SelectLawSummaryModel()
				.ToList()
				.OrderBy( x => items.IndexOf( x.ID ) )
				.ToList();

			FixLawSummaryListModelPercentages( result.Laws );

			result.Count = result.Laws.Count;

			return result;
		}

		#endregion


	}
}