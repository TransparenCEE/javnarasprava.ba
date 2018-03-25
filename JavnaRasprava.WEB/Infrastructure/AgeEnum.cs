using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
	public enum Age
	{
		[Display( Name = "<18" )]
		Child = 1,


		[Display( Name ="18-26" )]
		Student,

		[Display( Name ="27-35" )]
		Young,

		[Display( Name = "36-60" )]
		MidAge,

		[Display( Name = "60>" )]
		Senior
	}
}