
namespace JavnaRasprava.WEB.DomainModels
{
	public class LawCommentLike
	{
		public int LawCommentLikeID { get; set; }

		public bool Vote { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public int LawCommentID { get; set; }
		public virtual LawComment LawComment { get; set; }

		public int LawID { get; set; }
		public virtual Law Law { get; set; }
	}
}