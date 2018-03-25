namespace JavnaRasprava.WEB.Models.Quiz
{
	public class QuizSearchModel
	{
		public string QueryString { get; set; }

		public int? ParliamentId { get; set; }

		public QuizSort? Sort { get; set; }

		public int? page { get; set; }

		public int PageItemCount { get; set; }
	}
}