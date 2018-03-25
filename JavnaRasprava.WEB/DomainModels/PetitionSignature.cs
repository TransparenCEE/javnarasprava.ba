using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class PetitionSignature
	{
		public int PetitionSignatureID { get; set; }

		public int PetitionID { get; set; }
		public virtual Petition Petition { get; set; }

		public string ApplicationUserID { get; set; }
		public virtual ApplicationUser ApplicationUser { get; set; }

		public DateTime SignedTime { get; set; }
	}
}