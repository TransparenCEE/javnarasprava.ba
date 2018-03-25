using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Infrastructure
{
	[Obsolete]
	public enum CommentOrder
	{
		[Display( Name = "Najstariji prvo" )]
		Chronological = 0,

		[Display( Name = "Najmladji prvo" )]
		ReverseChronological = 1,

		[Display( Name = "Najpopularniji prvo" )]
		Popular = 2
	}
}