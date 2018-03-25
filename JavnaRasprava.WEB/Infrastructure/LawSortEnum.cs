using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
	public enum LawSort
	{
		[Display( Name = "Global_Sort_VoteTime", ResourceType = typeof( GlobalLocalization ) )]
		VoteTime = 1,

		[Display( Name = "Global_Sort_Latest", ResourceType = typeof( GlobalLocalization ) )]
		CreateTime = 2,

		[Display( Name = "Global_Sort_QuestionCount", ResourceType = typeof( GlobalLocalization ) )]
		QuestionCount = 3,

		[Display( Name = "Global_Sort_Title", ResourceType = typeof( GlobalLocalization ) )]
		Title = 4
	}
}