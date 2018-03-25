using JavnaRasprava.Resources;
using JavnaRasprava.WEB.Helpers;
using JavnaRasprava.WEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.User
{
	public class UserModel
	{
		public string Id { get; set; }

		[Display( Name = "User_UserName", ResourceType = typeof( GlobalLocalization ) )]
		public virtual string UserName { get; set; }

		public string Email { get; set; }

		public bool EmailConfirmed { get; set; }

		public DateTime? LockoutEndDateUtc { get; set; }

		public bool LockoutEnabled { get; set; }

		public int AccessFailedCount { get; set; }

		public bool IsAdmin { get; set; }

		public Age? Age { get; set; }

		public string AgeValue { get { return this.Age.HasValue ? EnumHelper<Age>.GetDisplayValue( this.Age.Value ) : "N/A"; } }

		public Education? Education { get; set; }

		public string EducationValue { get { return this.Education.HasValue ? EnumHelper<Education>.GetDisplayValue( this.Education.Value ) : "N/A"; } }

		public string Location { get; set; }

		public string Party { get; set; }
		public bool IsDisabled { get; internal set; }
	}
}