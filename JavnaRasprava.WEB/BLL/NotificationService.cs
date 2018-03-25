using JavnaRasprava.WEB.BLL.Mail;
using JavnaRasprava.WEB.DomainModels;
using NVelocity;
using NVelocity.App;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NLog;
using System.Diagnostics;
using JavnaRasprava.WEB.Infrastructure;
using System.Reflection;

namespace JavnaRasprava.WEB.BLL
{
    public class NotificationService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region == Process Questions Methods ==

        public void ProcessNewQuestions( List<UserRepresentativeQuestion> newQuestions )
        {
            if ( newQuestions == null || newQuestions.Count == 0 )
                return;

            var questionIds = newQuestions.Select( x => x.QuestionID ).ToList();

            

			Task.Run( () => ProcessNewQuestionsInternal( ( ctx ) =>
				{
					var ids = newQuestions.Select( x => x.UserRepresentativeQuestionID ).ToList();
					return ctx.UserRepresentativeQuestions.Where( x => ids.Contains( x.UserRepresentativeQuestionID ) );
				} ) );
		}

        public void ProcessNewQuestions( List<int> newQuestionIds, int? representativeId = null )
        {
            if ( newQuestionIds == null || newQuestionIds.Count == 0 )
                return;

			Task.Run( () => ProcessNewQuestionsInternal( ( ctx ) =>
			{
				var query = ctx.UserRepresentativeQuestions
						.Where( x => newQuestionIds.Contains( x.QuestionID ) )
						.Where( x => x.AnswerToken != null );

				if ( representativeId.HasValue )
				{
					query = query.Where( x => x.RepresentativeID == representativeId );
				}

				return query;
			} ) );

        }

        private static void ProcessNewQuestionsInternal( Func<ApplicationDbContext, IQueryable<UserRepresentativeQuestion>> queryFunc )
        {
            try
            {
                using ( var context = ApplicationDbContext.Create() )
                {                    
                    var questions = queryFunc(context).Include( x => x.AnswerToken )
                    .Include( x => x.Representative )
                    .Include( x => x.Question.Law )
                    .ToList();

                    using ( var sender = new Mail.EmailSender() )
                        foreach ( var question in questions )
                        {
                            Mail.Message message = null;
                            if ( question.Question.Law != null )
                                message = GenerateLawMessage( question );
                            else
                                message = GenerateDirectMessage( question );

                            // AnswerToken.Processed should be removed in near future
                            question.AnswerToken.Processed = true;

                            var questionMessage = new QuestionMessage
                            {
                                AnswerToken = question.AnswerToken,
                                MessageCreatedTime = DateTime.UtcNow,
                                Recipients = message.To,
                                Subject = message.Subject
                            };
                            var sent = sender.SendMessage( message );
                            if ( sent )
                                questionMessage.MessageSentTime = DateTime.UtcNow;

                            context.QuestionMessages.Add( questionMessage );

                            context.SaveChanges();
                        }
                }
            }
            catch ( Exception ex )
            {
                logger.Error( "Failed to send comm emails = " + ex.ToString() );
            }
        }

        private static Mail.Message GenerateDirectMessage( UserRepresentativeQuestion question )
        {
            DirectMessageModel model = new DirectMessageModel
            {
                BaseUrl = ConfigurationManager.AppSettings[ "BaseAddress" ],
                Question = question
            };

			return new Mail.Message
			{
				To = GetRepEmail( question.Representative.Email ),
				Bcc = GetBccDeliveryList(),
                Subject = ApplyMacros( GetDirectQuesitonEmailSubject(), model ),
                Body = ApplyMacros( GetDirectQuesitonEmailBody(), model )
            };
        }

        
        private static Mail.Message GenerateLawMessage( UserRepresentativeQuestion question )
        {
            DirectMessageModel model = new DirectMessageModel
            {
                BaseUrl = GetBaseUrl(),
                Question = question
            };

            return new Mail.Message
            {
                To = GetRepEmail( question.Representative.Email ),
				Bcc = GetBccDeliveryList(),
                Subject = ApplyMacros(GetLawQuesitonEmailSubject(), model ),
                Body = ApplyMacros(GetLawQuesitonEmailBody(), model )
            };
        }

        

        #endregion

        #region == Weekly Reports Methods ==

        public void StartWeeklyReportAsync()
        {
            Task.Run( () => ProcessWeeklyReport() );
        }

        public bool ProcessWeeklyReport()
        {
            StringBuilder sb = new StringBuilder();
            bool hadErrors = false;
            Stopwatch sw = new Stopwatch();
            Stopwatch swInternal = new Stopwatch();
            sw.Start();
            try
            {
                List<Mail.Message> messages = new List<Mail.Message>();

                using ( var context = ApplicationDbContext.Create() )
                {
                    var representatives = context.Representatives
                        .Where( x => x.UserRepresentativeQuestions.Any( y => y.AnswerToken != null && y.AnswerToken.Processed && y.AnswerToken.Answer == null ) )
                        .ToList();

                    foreach ( var rep in representatives )
                    {
                        try
                        {
                            swInternal = new Stopwatch();
                            swInternal.Start();
                            var questions = context.UserRepresentativeQuestions
                                .Where( x => x.RepresentativeID == rep.RepresentativeID )
                                .Where( x => x.Question.Verified )
								.Where( x => x.AnswerToken != null )
								.Where( x => x.AnswerToken.Answer == null )
                                .Include( x => x.Question.Law )
                                .Include( x => x.Question.Likes )
                                .Include( x => x.AnswerToken )
                                .ToList();

                            var questionCounts = questions
                                .GroupBy( x => x.QuestionID )
                                .Select( x => new { QuestionID = x.Key, Count = x.Count() } )
                                .ToDictionary( k => k.QuestionID, v => v.Count );

                            var laws = questions
                                .Select( x => x.Question )
                                .ToList()
                                .Select( x => x.Law )
                                .Where( x => x != null )
                                .Distinct();

                            var model = new WeeklyReportModel
                            {
                                BaseUrl = GetBaseUrl(),
                                FirstName = rep.FirstName,
                                LastName = rep.LastName,
                                RepresentativeId = rep.RepresentativeID,
                                Laws = laws.Select( x => new LawModel
                                {
                                    LawID = x.LawID,
                                    Title = x.Title,
                                    Questions = questions
                                                .Select( y => new { Question = y.Question, Token = y.AnswerToken } )
                                                .Where( y => y.Question.LawID != null && y.Question.LawID == x.LawID )
                                                .Select( y => new QuestionModel
                                                {
                                                    QuestionId = y.Question.QuestionID,
                                                    AskedOn = y.Question.CreateTimeUtc,
                                                    QuestionText = y.Question.Text,
                                                    AnswerToken = y.Token.Token,
                                                    VotesUp = y.Question.Likes.Count( z => z.Vote ),
                                                    VotesDown = y.Question.Likes.Count( z => !z.Vote ),
                                                    AskedCount = questionCounts[ y.Question.QuestionID ]
                                                } )
                                                .ToList()
                                } ).ToList(),
                                Questions = questions
											.Where( x => x.Question.Law == null )
                                            .Select( x => new QuestionModel
                                            {
                                                QuestionId = x.QuestionID,
                                                QuestionText = x.Question.Text,
                                                AskedOn = x.Question.CreateTimeUtc,
                                                AnswerToken = x.AnswerToken.Token,
                                                VotesUp = x.Question.Likes.Count( z => z.Vote ),
                                                VotesDown = x.Question.Likes.Count( z => !z.Vote ),
                                                AskedCount = questionCounts[ x.Question.QuestionID ]
                                            } )
                                            .ToList()
                            };

                            messages.Add( new Mail.Message
                            {
                                To = GetRepEmail( rep.Email ),
								Bcc = GetBccDeliveryList(),
								Subject = ApplyMacros( GetWeeklyEmailSubject(), model ),
                                Body = ApplyMacros( GetWeeklyEmailBody(), model )
                            } );

                            swInternal.Stop();
                            sb.AppendLine( string.Format( "Weekly email sent to {0}  in {1}ms", rep.DisplayName, swInternal.ElapsedMilliseconds ) );
                        }
                        catch ( Exception ex )
                        {
                            sb.AppendLine( string.Format( "Failed to send weekly email to {0} with exception: {1}{2} ", rep.DisplayName,  Environment.NewLine, ex ) );
                            hadErrors = true;
                        }
                    }

                }


                Mail.EmailSender.SendEmails( messages );
            }
            catch ( Exception ex )
            {
                sb.AppendLine( "Failed to send weekly emails = " + ex.ToString() );
            }


            sw.Stop();
            sb.AppendLine( string.Format( "Processing completed in {0}ms", sw.ElapsedMilliseconds ) );

            if ( hadErrors )
            {
                sb.AppendLine( "Errors Found !!!" );
                logger.Error( sb.ToString() );
            }
            else
            {
                sb.AppendLine( "No Errors Found." );
                logger.Warn( sb.ToString() );
            }


            return hadErrors;
        }

       

        #endregion

        #region == Helpers ==

        private static string GetBaseUrl()
        {
            return ConfigurationManager.AppSettings[ "BaseAddress" ];
        }

        private static string ApplyMacros( string stringToFormat, object model )
        {
            Velocity.Init();

            var velocityContext = new VelocityContext();
            velocityContext.Put( "model", model );

            var sb = new StringBuilder();
            Velocity.Evaluate( velocityContext, new StringWriter( sb ), "test template", new StringReader( stringToFormat ) );

            return sb.ToString();
        }

        private static string GetRepEmail( string original )
        {
            var overrideEmail = ConfigurationManager.AppSettings[ "EMAIL.TOOVERRIDE" ];

            if ( !string.IsNullOrWhiteSpace( overrideEmail ) )
                return overrideEmail;

			return original;
		}

		private static string GetBccDeliveryList()
		{
			return ConfigurationManager.AppSettings[ "EMAIL.TOADD" ];
		}

        #endregion

        #region GetEmailTemplatesMethods

        private static string GetDirectQuesitonEmailBody()
        {
            return GetEmailTemplate("directquestion.body");
        }

        public static string GetDirectQuesitonEmailSubject()
        {
            return GetEmailTemplate("directquestion.subject");
        }

        private static string GetLawQuesitonEmailSubject()
        {
            return GetEmailTemplate("lawquestion.subject");
        }

        private static string GetLawQuesitonEmailBody()
        {
            return GetEmailTemplate("lawquestion.body");
        }

        private string GetWeeklyEmailBody()
        {
            return GetEmailTemplate("weekly.body");
        }

        private string GetWeeklyEmailSubject()
        {
            return GetEmailTemplate("weekly.subject");
        }

        private static string GetEmailTemplate(string templateName)
        {
            var culture = FeatureToggle.GetDefaultCulture();

            return GetEmbeddedResource($"JavnaRasprava.WEB.App_GlobalResources.EmailTemplates.email.{templateName}._{culture}.txt");
        }

        private static string GetEmbeddedResource(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new KeyNotFoundException($"Resource [{resourceName}; not found in assembly {assembly.FullName}; available resources: [{string.Join(", ", assembly.GetManifestResourceNames())}]");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        #endregion
    }
}