
namespace JavnaRasprava.WEB.Models
{
	public class AskRepresentativeModel
	{
		#region == Fields ==

		#endregion

		#region == Properties ==

		public int ID { get; set; }

		public string FullName { get; set; }
		
		public string PartyName { get; set; }

		public string Reason { get; set; }

		public string ImageRelativePath { get; set; }

		public bool IsSuggested { get; set; }

		public bool IsSelected { get; set; }

		#endregion

		#region == Constructors ==

		#endregion

		#region == Methods ==

		#endregion
	}
}
