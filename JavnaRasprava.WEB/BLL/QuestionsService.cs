using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using EntityFramework.Extensions;

namespace JavnaRasprava.WEB.BLL
{
	public class QuestionsService
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		#endregion

		#region == Constructors ==

		#endregion

		#region == Prepare Qestion Models Methods ==

		public AskLawQuestionModel GetQuestionsModel( int lawID, string userID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new AskLawQuestionModel
				{
					LawID = lawID,
					OtherRepresentatives = new List<AskRepresentativeModel>(),
					SuggestedRepresentatives = new List<AskRepresentativeModel>(),
					Questions = new List<AskPredefinedQuestionsModel>(),
				};

				var user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
				if ( user == null )
					return null;

				var law = context.Laws
					.Where( x => x.LawID == lawID )
					.Include( x => x.LawRepresentativeAssociations.Select( lra => lra.Representative.Party ) )
					.FirstOrDefault();
				if ( law == null )
					return null;

				foreach ( var ra in law.LawRepresentativeAssociations )
				{
					result.SuggestedRepresentatives.Add(
						new AskRepresentativeModel
						{
							ID = ra.RepresentativeID,
							FullName = ra.Representative.DisplayName,
							PartyName = ra.Representative.Party.Name,
							IsSuggested = true,
							Reason = ra.Reason,
							ImageRelativePath = ra.Representative.ImageRelativePath
						} );
				}

				var suggestedRepIDList = law.LawRepresentativeAssociations.Select( x => x.RepresentativeID ).ToList();
				result.OtherRepresentatives = context.Representatives
					.Include( x => x.Party )
					.Where( x => x.ParliamentHouse.ParliamentID == law.ParliamentID && !suggestedRepIDList.Contains( x.RepresentativeID ) )
					.ToList()
					.Select( x => new AskRepresentativeModel
					{
						ID = x.RepresentativeID,
						IsSuggested = false,
						FullName = x.DisplayName,
						PartyName = x.Party.Name,
						ImageRelativePath = x.ImageRelativePath
					} )
					.ToList();

				result.Questions = context.Questions
					.Where( x => x.LawID == lawID && x.IsSuggested )
					.Select( x => new AskPredefinedQuestionsModel
					{
						ID = x.QuestionID,
						Text = x.Text
					} )
					.ToList();

				return result;
			}

		}

		public AskRepresentativeQuestionModel GetQuestionsmodelForRepresentative( int representativeID, string userID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var representative = context.Representatives
					.Where( x => x.RepresentativeID == representativeID )
					.Include( x => x.Party )
					.FirstOrDefault();

				if ( representative == null )
					return null;

				return new AskRepresentativeQuestionModel
				{
					RepresentativeFullName = representative.DisplayName,
					RepresentativeID = representative.RepresentativeID,
					RepresentativePartyName = representative.Party.Name
				};
			}
		}

		#endregion

		#region == Post Questions Methods ==

		public void PostQuestion( AskLawQuestionModel model, string userID )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
				if ( user == null )
					return;

				var law = context.Laws.Where( x => x.LawID == model.LawID ).FirstOrDefault();
				if ( law == null )
					return;

				List<int> selectedPredefinedQuestionIDs = new List<int>();
				if ( model.Questions != null )
					selectedPredefinedQuestionIDs = model.Questions.Where( x => x.IsSelected ).Select( x => x.ID ).ToList();

				var predefinedQuestions = context.Questions.Where( x => selectedPredefinedQuestionIDs.Contains( x.QuestionID ) );

				var selectedRepresentativeIDs = model.SuggestedRepresentatives.Where( x => x.IsSelected ).Select( x => x.ID ).ToList();
				selectedRepresentativeIDs.AddRange( model.OtherRepresentatives.Where( x => x.IsSelected ).Select( x => x.ID ).ToList() );

				if ( selectedRepresentativeIDs.Count == 0 )
					return;

				// If user asked any of given questions 
				var existingUserQuestions = context.UserRepresentativeQuestions
					.Where( x => x.ApplicationUserID == user.Id
						&& selectedPredefinedQuestionIDs.Contains( x.QuestionID )
						&& selectedRepresentativeIDs.Contains( x.RepresentativeID ) )
					.ToList();

				// should check if any of users asked same question to skip creating token
				var existingQuestions = context.UserRepresentativeQuestions
					.Where( x => selectedPredefinedQuestionIDs.Contains( x.QuestionID )
						&& selectedRepresentativeIDs.Contains( x.RepresentativeID ) )
					.ToList();

				// Prepare custom question if needed
				DomainModels.Question customQuestion = null;

				if ( !string.IsNullOrWhiteSpace( model.Text ) )
				{
					customQuestion = new DomainModels.Question
					{
						LawID = law.LawID,
						Text = model.Text,
						IsSuggested = false,
						CreateTimeUtc = DateTime.UtcNow
					};
					context.Questions.Add( customQuestion );
				}

				var newQuestions = new List<DomainModels.UserRepresentativeQuestion>();
				AnswerToken answerToken = null;
				foreach ( var rep in selectedRepresentativeIDs.Distinct() )
				{
					foreach ( var question in selectedPredefinedQuestionIDs )
					{
						answerToken = null;
						if ( existingUserQuestions.Where( x => x.RepresentativeID == rep && x.QuestionID == question ).Any() )
							continue;

						if ( !existingQuestions.Where( x => x.RepresentativeID == rep && x.QuestionID == question ).Any() )
						{
							answerToken = new AnswerToken
							{
								RepresentativeID = rep,
								QuestionID = question,
								Token = Guid.NewGuid(),
								CreateTimeUtc = DateTime.UtcNow
							};
							context.AnswerTokens.Add( answerToken );
						}

						var newPredefinedQuestion = new DomainModels.UserRepresentativeQuestion
						{
							ApplicationUserID = user.Id,
							QuestionID = question,
							RepresentativeID = rep,
							CreateTimeUtc = DateTime.UtcNow,
							AnswerToken = answerToken
						};
						context.UserRepresentativeQuestions.Add( newPredefinedQuestion );
						// Notifications are being processed only for predefined questions at this point.
						if ( answerToken != null )
							newQuestions.Add( newPredefinedQuestion );
					}

					if ( !string.IsNullOrWhiteSpace( model.Text ) )
					{
						answerToken = new AnswerToken
						{
							RepresentativeID = rep,
							Question = customQuestion,
							Token = Guid.NewGuid(),
							CreateTimeUtc = DateTime.UtcNow
						};
						context.AnswerTokens.Add( answerToken );

						context.UserRepresentativeQuestions.Add(
							new DomainModels.UserRepresentativeQuestion
							{
								ApplicationUserID = user.Id,
								RepresentativeID = rep,
								CreateTimeUtc = DateTime.UtcNow,
								Question = customQuestion,
								AnswerToken = answerToken
							} );


					}
				}

				context.SaveChanges();
				new NotificationService().ProcessNewQuestions( newQuestions );
			}
		}

		public bool PostRepresentativeQuestion( AskRepresentativeQuestionModel model, string userID )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var representative = context.Representatives
					.Where( x => x.RepresentativeID == model.RepresentativeID )
					.Include( x => x.Party )
					.FirstOrDefault();

				if ( representative == null )
					return false;

				var user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
				if ( user == null )
					return false;

				var customQuestion = new Question
				{
					CreateTimeUtc = DateTime.UtcNow,
					Text = model.Text,
					IsSuggested = false,
					Verified = false
				};
				context.Questions.Add( customQuestion );

				var answerToken = new AnswerToken
				{
					Representative = representative,
					Question = customQuestion,
					Token = Guid.NewGuid(),
					CreateTimeUtc = DateTime.UtcNow
				};
				context.AnswerTokens.Add( answerToken );

				var question = new UserRepresentativeQuestion
				{
					ApplicationUserID = userID,
					CreateTimeUtc = DateTime.UtcNow,
					Question = customQuestion,
					AnswerToken = answerToken,
					Processed = false,
					Representative = representative
				};

				context.UserRepresentativeQuestions.Add( question );
				context.SaveChanges();
				return true;
			}
		}

		#endregion

		#region == Answer Methods ==

		public void PostAnswer( int quesitonID, int representativeID, string answer )
		{
			if ( string.IsNullOrEmpty( answer ) )
				return;

			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var question = context.Questions
					.Where( x => x.QuestionID == quesitonID )
					.FirstOrDefault();
				if ( question == null ) return;

				var rep = context.Representatives
					.Where( x => x.RepresentativeID == representativeID )
					.FirstOrDefault();
				if ( rep == null ) return;

				context.Answers.Add( new DomainModels.Answer
				{
					Question = question,
					Representative = rep,
					Text = answer,
					AnsweredTimeUtc = DateTime.UtcNow
				} );

				context.SaveChanges();
			}

		}

		#endregion

		#region == Other public methods ==

		public LawQuestionsModel GetQuestionsForLaw( int lawID, string userID )
		{
			var auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var law = context.Laws
					.Where( x => x.LawID == lawID )
					.Include( "Questions.UserRepresentativeQuestions.Representative.Party" )
					.FirstOrDefault();
				if ( law == null )
					return null;

				var result = new LawQuestionsModel
				{
					LawID = lawID,
					Law = law,
					Questions = new List<QuestionModel>(),
				};

				result.TotalQuestionsMade = law.Questions.SelectMany( x => x.UserRepresentativeQuestions ).Count();

				if ( result.TotalQuestionsMade == 0 )
					return result;

				var lawQuestionCounts = law.Questions
					.Where( x => x.Verified )
					.SelectMany( x => x.UserRepresentativeQuestions )
					.GroupBy( x => x.QuestionID )
					.Select( g => new { QuestionId = g.Key, Count = g.Count() } )
					.ToList();

				var representativeQuestionCount = law.Questions
					.Where( x => x.Verified )
					.SelectMany( x => x.UserRepresentativeQuestions )
					.GroupBy( x => new { x.RepresentativeID, x.QuestionID } )
					.Select( g => new { g.Key.QuestionID, g.Key.RepresentativeID, Count = g.Count() } )
					.ToList();

				var representativeIDs = law.Questions
					.Where( x => x.Verified )
					.SelectMany( x => x.UserRepresentativeQuestions )
					.Select( x => x.RepresentativeID )
					.ToList();

				var questionIDs = law.Questions
					.Where( x => x.Verified )
					.Select( x => x.QuestionID ).ToList();

				var questionLikeCounts = context.QuestionLikes
					.Where( x => questionIDs.Contains( x.QuestionID ) )
					.GroupBy( x => new { x.QuestionID, x.Vote } )
					.Select( g => new { QuestionID = g.Key.QuestionID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				var userQuestionLikes = context.QuestionLikes
					.Where( x => questionIDs.Contains( x.QuestionID ) && x.ApplicationUserID == userID )
					.ToList();

				var answers = context.Answers
					.Where( x => representativeIDs.Contains( x.RepresentativeID ) && questionIDs.Contains( x.QuestionID ) )
					.ToList();

				var answerIDs = answers.Select( x => x.AnswerID ).ToList();

				var answerLikesCounts = context.AnswerLikes
					.Where( x => answerIDs.Contains( x.AnswerID ) )
					.GroupBy( x => new { x.AnswerID, x.Vote } )
					.Select( g => new { AnswerID = g.Key.AnswerID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				var userAnswerLikes = context.AnswerLikes
					.Where( x => x.ApplicationUserID == userID && answerIDs.Contains( x.AnswerID ) )
					.ToList();

				foreach ( var question in law.Questions.Where( x => x.Verified ) )
				{
					QuestionModel qmodel = PopulateQuestionModel( question );

					qmodel.AskedCount = lawQuestionCounts.Where( x => x.QuestionId == question.QuestionID ).Select( x => x.Count ).FirstOrDefault();
					qmodel.LikesCount = questionLikeCounts.Where( x => x.QuestionID == question.QuestionID && x.Vote ).Select( x => x.Count ).FirstOrDefault();
					qmodel.DislikesCount = questionLikeCounts.Where( x => x.QuestionID == question.QuestionID && !x.Vote ).Select( x => x.Count ).FirstOrDefault();

					var userQuestionLike = userQuestionLikes.Where( x => x.ApplicationUserID == userID && x.QuestionID == question.QuestionID ).FirstOrDefault();

					if ( !string.IsNullOrWhiteSpace( userID ) )
					{
						qmodel.UserLiked = userQuestionLike == null ? (bool?)null : userQuestionLike.Vote;
					}
					else
					{
						qmodel.UserLiked = auService.GetUserQuestionLike( question.QuestionID );
					}
					qmodel.Representatives = new List<QustionRepresentativeModel>();

					foreach ( var rep in question.UserRepresentativeQuestions.Select( x => x.Representative ) )
					{
						// Collection has item per question so it has to be added only once
						if ( qmodel.Representatives.Any( x => x.ID == rep.RepresentativeID ) )
							continue;

						QustionRepresentativeModel repModel = PopulateQuestionRepresentativeModel( rep );
						repModel.AskedCount = representativeQuestionCount
							.Where( x => x.QuestionID == question.QuestionID && x.RepresentativeID == rep.RepresentativeID )
							.Select( x => x.Count )
							.First();

						var answer = answers.Where( x => x.RepresentativeID == rep.RepresentativeID && x.QuestionID == question.QuestionID ).FirstOrDefault();
						if ( answer != null )
						{
							repModel.Answered = true;
							repModel.Answer = PopulateAnswerModel( answer );
							repModel.Answer.LikesCount = answerLikesCounts.Where( x => x.AnswerID == answer.AnswerID && x.Vote ).Select( x => x.Count ).FirstOrDefault();
							repModel.Answer.DislikesCount = answerLikesCounts.Where( x => x.AnswerID == answer.AnswerID && !x.Vote ).Select( x => x.Count ).FirstOrDefault();

							if ( !string.IsNullOrWhiteSpace( userID ) )
							{
								var userAnswer = userAnswerLikes.Where( x => x.AnswerID == answer.AnswerID && x.ApplicationUserID == userID ).FirstOrDefault();
								repModel.Answer.UserLiked = userAnswer == null ? (bool?)null : userAnswer.Vote;
							}
							else
							{
								repModel.Answer.UserLiked = auService.GetUserAnswerLike( answer.AnswerID );
							}
						}

						qmodel.Representatives.Add( repModel );
					}

					qmodel.AnswersCount = qmodel.Representatives.Count( x => x.Answered );
					result.Questions.Add( qmodel );
				}

				return result;
			}
		}

		public AnswersListModel GetLatestAnswersForLaw( int lawID, string userID = null, int count = 3 )
		{
			AnonymousUserService auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var result = new AnswersListModel
				{
					LawID = lawID,
					Answers = new List<AnswerModel>()
				};

				var answersDictionary = new Dictionary<int, AnswerModel>();

				ApplicationUser user = null;
				if ( string.IsNullOrWhiteSpace( userID ) )
				{
					user = auService.GetAnonymousUser( context );
				}
				else
				{
					user = context.Users.Where( x => x.Id == userID ).FirstOrDefault();
					if ( user == null )
						return null;

				}

				var law = context.Laws.Where( x => x.LawID == lawID ).FirstOrDefault();
				if ( law == null )
					return null;

				context.Questions
					.Where( x => x.LawID == lawID )
					.Where( x => x.Verified )
					.SelectMany( x => x.Answers )
					.Include( x => x.Question )
					.Include( x => x.Representative.Party )
					.OrderByDescending( x => x.AnsweredTimeUtc )
					.Take( count )
					.ToList()
					.ForEach( x => answersDictionary[x.AnswerID] = PopulateAnswerModel( x ) );

				var answerIDs = answersDictionary.Keys.ToList();

				var answerLikesCounts = context.AnswerLikes
					.Where( x => answerIDs.Contains( x.AnswerID ) )
					.GroupBy( x => new { x.AnswerID, x.Vote } )
					.Select( g => new { AnswerID = g.Key.AnswerID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				foreach ( var ual in answerLikesCounts )
				{
					if ( ual.Vote )
						answersDictionary[ual.AnswerID].LikesCount = ual.Count;
					else
						answersDictionary[ual.AnswerID].DislikesCount = ual.Count;

				}

				if ( userID != null )
				{
					var userAnswerLikes = context.AnswerLikes
								.Where( x => x.ApplicationUserID == userID && answerIDs.Contains( x.AnswerID ) )
								.ToList();

					foreach ( var ual in userAnswerLikes )
					{
						answersDictionary[ual.AnswerID].UserLiked = ual.Vote;
					}
				}
				else
				{
					foreach ( var answer in answersDictionary.Values )
					{
						answer.UserLiked = auService.GetUserAnswerLike( answer.ID );
					}
				}

				result.Answers = answersDictionary.Values.ToList();
				result.Count = result.Answers.Count;

				return result;
			}
		}

		public AnswerModel LikeAnswer( int answerID, string userId, bool value )
		{
			AnonymousUserService auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				bool userLiked = false;
				ApplicationUser user = null;
				if ( userId == null )
				{
					user = auService.GetAnonymousUser( context );
					userLiked = auService.HasLikedAnswer( answerID );
				}
				else
				{
					user = context.Users.Where( x => x.Id == userId ).FirstOrDefault();
					if ( user == null )
						return null;

					var like = context.AnswerLikes.Where( x => x.ApplicationUserID == user.Id && x.AnswerID == answerID ).FirstOrDefault();
					userLiked = like != null;
				}

				var answer = context.Answers
					.Where( x => x.AnswerID == answerID )
					.Include( x => x.Question )
					.FirstOrDefault();
				if ( answer == null )
					return null;

				if ( !userLiked )
				{
					context.AnswerLikes.Add( new DomainModels.AnswerLike
					{
						AnswerID = answerID,
						ApplicationUserID = user.Id,
						Vote = value,
					} );

					context.SaveChanges();

					if ( userId == null )
					{
						auService.LikeAnswer( answerID, value );
					}
				}

				var answerLikesCounts = context.AnswerLikes
					.Where( x => x.AnswerID == answerID )
					.GroupBy( x => new { x.AnswerID, x.Vote } )
					.Select( g => new { AnswerID = g.Key.AnswerID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				var result = PopulateAnswerModel( answer );
				result.LikesCount = answerLikesCounts.Where( x => x.AnswerID == answer.AnswerID && x.Vote ).Select( x => x.Count ).FirstOrDefault();
				result.DislikesCount = answerLikesCounts.Where( x => x.AnswerID == answer.AnswerID && !x.Vote ).Select( x => x.Count ).FirstOrDefault();

				result.UserLiked = value;

				return result;
			}
		}

		public QuestionModel LikeQuestion( int questionID, string userId, bool value )
		{
			AnonymousUserService auService = new AnonymousUserService();
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				bool userLiked = false;
				ApplicationUser user = null;
				if ( userId == null )
				{
					user = auService.GetAnonymousUser( context );
					userLiked = auService.HasLikedQuestion( questionID );
				}
				else
				{
					user = context.Users.Where( x => x.Id == userId ).FirstOrDefault();
					if ( user == null )
						return null;

					var like = context.QuestionLikes.Where( x => x.ApplicationUserID == userId && x.QuestionID == questionID ).FirstOrDefault();
					userLiked = like != null;
				}

				var question = context.Questions.Where( x => x.QuestionID == questionID ).FirstOrDefault();
				if ( question == null )
					return null;

				if ( !userLiked )
				{
					context.QuestionLikes.Add( new DomainModels.QuestionLike
					{
						QuestionID = questionID,
						ApplicationUserID = user.Id,
						Vote = value,
					} );
					context.SaveChanges();


					if ( userId == null )
					{
						auService.LikeQuestion( questionID, value );
					}
				}

				var questionLikesCounts = context.QuestionLikes
					.Where( x => x.QuestionID == questionID )
					.GroupBy( x => new { x.QuestionID, x.Vote } )
					.Select( g => new { QuestionID = g.Key.QuestionID, Vote = g.Key.Vote, Count = g.Count() } )
					.ToList();

				var answersCount = context.Answers.Where( x => x.QuestionID == questionID ).Count();

				// TODO:
				return new QuestionModel
				{
					AnswersCount = answersCount,
					DislikesCount = questionLikesCounts.Where( x => !x.Vote ).Select( x => x.Count ).FirstOrDefault(),
					Id = questionID,
					LikesCount = questionLikesCounts.Where( x => x.Vote ).Select( x => x.Count ).FirstOrDefault(),
					UserLiked = value
				};
			}
		}

		#endregion

		#region == Private Methods ==

		private static AnswerModel PopulateAnswerModel( DomainModels.Answer answer )
		{
			var result = new AnswerModel
			{
				ID = answer.AnswerID,
				Text = answer.Text,
				TimeAnsweredUtc = answer.AnsweredTimeUtc
			};

			if ( answer.Representative != null )
			{
				result.Representative = PopulateQuestionRepresentativeModel( answer.Representative );
			}

			if ( answer.Question != null )
			{
				result.Question = PopulateQuestionModel( answer.Question );
			}

			return result;
		}

		private static QustionRepresentativeModel PopulateQuestionRepresentativeModel( DomainModels.Representative representative )
		{
			QustionRepresentativeModel result = new QustionRepresentativeModel
			{
				ID = representative.RepresentativeID,
				FullName = representative.DisplayName,
				ImageRelativePath = representative.ImageRelativePath,
			};

			if ( representative.Party != null )
				result.PartyName = representative.Party.Name;

			return result;
		}

		private static QuestionModel PopulateQuestionModel( DomainModels.Question question )
		{
			QuestionModel qmodel = new QuestionModel
			{
				Id = question.QuestionID,
				Text = question.Text,
				IsPredefined = question.IsSuggested,
				TimeCreatedUtc = question.CreateTimeUtc,
			};
			return qmodel;
		}

		#endregion

		#region == Check Question Message Status Methods ==

		public List<Models.QuestionMessage.QuestionMessageModel> GetQuestionMessagesForRepresentative( int repId )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var qustionData = context.UserRepresentativeQuestions
					//.Include( x => x.Representative )
					//.Include( x => x.Question.Law )
					//.Include("AnswerToken.Messages")
					.Where( x => x.RepresentativeID == repId )
					.GroupBy( x => new
					{
						x.RepresentativeID,
						x.QuestionID,
						x.Question.Verified,
						x.Question.IsSuggested,
						x.Question.Text,
						x.Question.LawID,
						x.Question.Law.Title
					} )
					.Select( x => new Models.QuestionMessage.QuestionMessageModel
					{
						RepresentativeId = x.Key.RepresentativeID,
						QuestionId = x.Key.QuestionID,
						QuestionVerified = x.Key.Verified,
						LawId = x.Key.LawID,
						LawTitle = x.Key.Title,
						IsSuggested = x.Key.IsSuggested,
						QuestionText = x.Key.Text,
						AskedCount = x.Count()
					} )
					.ToDictionary( k => k.QuestionId, v => v );

				var messageData = context.AnswerTokens
					.Where( x => x.RepresentativeID == repId )
					.Select( x => new
					{
						QuestionId = x.QuestionID,
						Answered = x.Answer != null,
						MessagesSent = x.Messages.Count()
					} )
					.ToList();


				messageData.ForEach( x =>
				 {
					 var question = qustionData[x.QuestionId];
					 question.Answered = x.Answered;
					 question.MessagesSentCount = x.MessagesSent;
				 } );

				return qustionData.Values.ToList();
			}

		}

		#endregion
	}
}