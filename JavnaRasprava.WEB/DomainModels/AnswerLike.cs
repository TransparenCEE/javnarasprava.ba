
namespace JavnaRasprava.WEB.DomainModels
{
	public class AnswerLike
	{
		public int AnswerLikeID { get; set; }

		public int AnswerID { get; set; }
		public virtual Answer Answer { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public bool Vote { get; set; }

	}
}