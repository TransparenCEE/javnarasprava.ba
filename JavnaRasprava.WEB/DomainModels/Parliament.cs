using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.DomainModels
{
	public class Parliament
	{
		public int ParliamentID { get; set; }

		public string Name { get; set; }

		public string Code { get; set; }

		public int Order { get; set; }

		public string RepresentativesScreenTitle { get; set; }

		public bool IsExclusive { get; set; }

		public string Tenant { get; set; }

		public virtual ICollection<Law> Laws { get; set; }

		public virtual ICollection<ParliamentHouse> ParliamentHouses { get; set; }

		public string TenantSubDomain { get; internal set; }
	}
}