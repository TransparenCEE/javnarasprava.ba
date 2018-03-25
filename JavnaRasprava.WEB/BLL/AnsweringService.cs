using JavnaRasprava.WEB.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using EntityFramework.Extensions;

namespace JavnaRasprava.WEB.BLL
{
	public class AnsweringService
	{		
		public Models.Law.PostAnswerModel InitializePostAnswerModel( string tokenstring )
		{
			Guid token = Guid.Empty;

			if ( !Guid.TryParse( tokenstring, out token ) )
				return null;


			using ( var context = ApplicationDbContext.Create() )
			{
				var answerToken = context.AnswerTokens
					.Where( x => x.Token == token )
					.Include( x => x.Question.Law )
					.Include( x => x.Representative )
					.FirstOrDefault();

				if ( answerToken == null )
					return null;
				return new Models.Law.PostAnswerModel
				{
					LawID = answerToken.Question.LawID ?? 0,
					LawTitle = answerToken.Question.Law == null ? "Direktno pitanje" : answerToken.Question.Law.Title,
					QuestionID = answerToken.Question.QuestionID,
					QuestionText = answerToken.Question.Text,
					RepresentativeDisplayName = answerToken.Representative.DisplayName,
					RepresentativeID = answerToken.RepresentativeID,
					RepresentativeImage = answerToken.Representative.ImageRelativePath,
					AnswerToken = token,
					Answer = string.Empty
				};
			}
		}

		public bool PostAnswerModel( Models.Law.PostAnswerModel model )
		{

			using ( var context = ApplicationDbContext.Create() )
			{
				var answerToken = context.AnswerTokens
					.Where( x => x.Token == model.AnswerToken )
					.Include( x => x.Question.Law )
					.Include( x => x.Representative )
					.FirstOrDefault();

				if ( answerToken == null )
					return false;

				if ( context.Answers.Any( x => x.RepresentativeID == model.RepresentativeID && x.QuestionID == model.QuestionID ) )
					return false;

				var answer = new Answer
					{
						QuestionID = model.QuestionID,
						RepresentativeID = model.RepresentativeID,
						AnsweredTimeUtc = DateTime.UtcNow,
						Text = model.Answer
					};

				context.Answers.Add( answer );
				context.SaveChanges();
				context.AnswerTokens
					.Where( x => x.QuestionID == answerToken.QuestionID && x.RepresentativeID == answerToken.RepresentativeID && x.AnswerTokenID <= answerToken.AnswerTokenID )
					.Update( x => new AnswerToken { Processed = true, AnswerID = answer.AnswerID } );

				context.SaveChanges();
				return true;
			}
		}
	}
}