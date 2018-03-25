using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
    public class LawCustomVote
    {
        public int LawCustomVoteID { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public bool? Vote { get; set; }

        public bool AdminIgnore { get; set; }

		public bool IsSuggested { get; set; }

        public int LawID { get; set; }
        public virtual Law Law { get; set; }

		public ICollection<LawVote> Votes { get; set; }

    }
}