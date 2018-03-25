using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Petition
	{
		public int PetitionID { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public Parliament TargetInstitution { get; set; }
		public int TargetInstitutionId { get; set; }

		public string SubmitterName { get; set; }

		public bool Verified { get; set; }

        public bool AdminIgnore { get; set; }

		public virtual ApplicationUser SubmitterUser { get; set; }

		public string SubmitterUserID { get; set; }

		public virtual ICollection<PetitionSignature> PetitionSignatures { get; set; }

		public string YoutubeCode { get; set; }

		public string ImageRelativePath { get; set; }

		public DateTime CreateTime { get; set; }

		public Petition()
		{
			CreateTime = DateTime.UtcNow;
		}

	}
}