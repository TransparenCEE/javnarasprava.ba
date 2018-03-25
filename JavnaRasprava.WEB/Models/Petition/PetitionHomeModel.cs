using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace JavnaRasprava.WEB.Models
{
    public class PetitionHomeModel
    {
        public IPagedList<Models.PetitionSummaryModel> LastSuccessfulPetitions { get; set; }
        public IPagedList<Models.PetitionSummaryModel> TopActivePetitions { get; set; }

		public ParliamentListModel Parliaments { get; set; }
		public int? ParliamentId { get; set; }
	}
}