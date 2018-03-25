using JavnaRasprava.Resources;
using System.ComponentModel.DataAnnotations;

namespace JavnaRasprava.WEB.Infrastructure
{
	public enum Education
	{
		[Display( Name = "Education_Elementary", ResourceType = typeof( GlobalLocalization ) )]
		Osnovna = 1,

		[Display( Name = "Education_MiddleScool", ResourceType = typeof( GlobalLocalization ) )]
		Srednja,

		[Display( Name = "Education_UpperSchool", ResourceType = typeof( GlobalLocalization ) )]
		Visa,

		[Display( Name = "Education_Bachelor", ResourceType = typeof( GlobalLocalization ) )]
		Bachelor,

		[Display( Name = "Education_Faculty", ResourceType = typeof( GlobalLocalization ) )]
		Visoka,

		[Display( Name = "Education_Master", ResourceType = typeof( GlobalLocalization ) )]
		Magisterij,

		[Display( Name = "Education_Doctoral", ResourceType = typeof( GlobalLocalization ) )]
		Doktorat
	}
}