
namespace JavnaRasprava.WEB.DomainModels
{
	public class QuestionLike
	{
		public int QuestionLikeID { get; set; }

		public int QuestionID { get; set; }
		public virtual Question Question { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public bool Vote { get; set; }

	}
}