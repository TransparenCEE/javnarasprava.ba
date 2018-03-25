using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure.Helpers
{
	public static class ExtenstionMethods
	{
		public static IQueryable<Models.LawSummaryModel> SelectLawSummaryModel( this IQueryable<DomainModels.Law> laws )
		{
			return laws.Select( x => new Models.LawSummaryModel
			{
				ID = x.LawID,
				Title = x.Title,
				CreateDateTimeUtc = x.CreateDateTimeUtc,
				ExpetedVotingDayDateTime = x.ExpetedVotingDay,
				ImageRelativePath = x.ImageRelativePath,
				AskedCount = x.Questions.Sum( q => (int?)q.UserRepresentativeQuestions.Count ) ?? 0,
				AnswersCount = x.Questions.Sum( q => (int?)q.Answers.Count ) ?? 0,
				VotesDown = x.LawVotes.Count( lv => lv.Vote.HasValue && !lv.Vote.Value ),
				VotesUp = x.LawVotes.Count( lv => lv.Vote.HasValue && lv.Vote.Value ),
				Sections = x.LawSections.Select( y => new Models.LawSectionSummaryModel
				{
					LawSectionID = y.LawSectionID,
					Text = y.Text,
					Title = y.Title,
					Description = y.Description,
					ImageRelativePath = y.ImageRelativePath
				} )
			} );
		}

		public static IQueryable<Models.LawSummaryModel> SelectLawSummaryModelForSearch( this IQueryable<DomainModels.Law> laws )
		{
			return laws.Select( x => new Models.LawSummaryModel
			{
				ID = x.LawID,
				Title = x.Title,
				CreateDateTimeUtc = x.CreateDateTimeUtc,
				ExpetedVotingDayDateTime = x.ExpetedVotingDay,
				ImageRelativePath = x.ImageRelativePath,
				AskedCount = x.Questions.Sum( q => (int?)q.UserRepresentativeQuestions.Count ) ?? 0,
				AnswersCount = x.Questions.Sum( q => (int?)q.Answers.Count ) ?? 0,
				VotesDown = x.LawVotes.Count( lv => lv.Vote.HasValue && !lv.Vote.Value ),
				VotesUp = x.LawVotes.Count( lv => lv.Vote.HasValue && lv.Vote.Value ),
				Sections = x.LawSections.Select(ls => new Models.LawSectionSummaryModel {  LawSectionID = ls.LawSectionID, Title = ls.Title} )			
			} );
		}

		public static IQueryable<Models.LawSectionSummaryModel> SelectLawSummaryModelForSearch( this IQueryable<DomainModels.LawSection> sections )
		{
			return sections.Select( x => new Models.LawSectionSummaryModel
			{
				LawSectionID = x.LawSectionID,
				Title = x.Title,
				ImageRelativePath = x.ImageRelativePath,
				VotesDown = x.LawSectionVotes.Count( lv => lv.Vote.HasValue && !lv.Vote.Value ),
				VotesUp = x.LawSectionVotes.Count( lv => lv.Vote.HasValue && lv.Vote.Value ),
				LawCreateDateTimeUtc = x.Law.CreateDateTimeUtc,
				LawExpetedVotingDayDateTime = x.Law.ExpetedVotingDay,
				LawAskedCount = x.Law.Questions.Sum( q => (int?)q.UserRepresentativeQuestions.Count ) ?? 0,
				LawID = x.LawID,
				LawTitle = x.Law.Title
			} );
		}

		

		//public static IQueryable<Models.Quiz.QuizEditModel> SelectQuiz(this IQueryable<DomainModels.Quiz> quizes)
		//{
		//	return quizes
		//		.Select( x => new Models.Quiz.QuizEditModel
		//		{
		//			QuizId = x.QuizId,
		//			Title = x.Title,
		//			Description = x.Description,
		//			TimeCreated = x.TimeCreated,
		//			Items = x.Items.Select( i => new Models.Quiz.QuizItemAnsweringModel
		//			{
		//				QuizItemId = i.QuizItemId,
		//				LawId = i.Type == DomainModels.QuizItemType.Law ? i.Law.LawID : (int?)null,
		//				LawSectionId = i.Type == DomainModels.QuizItemType.LawSection ? i.LawSection.LawSectionID : (int?)null
		//			} ).ToList()
		//		} );
		//}

		
	}
}