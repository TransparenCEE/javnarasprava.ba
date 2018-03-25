using JavnaRasprava.Resources;
using System.ComponentModel.DataAnnotations;

namespace JavnaRasprava.WEB.Models.News
{
	public enum NewsSort
	{
		[Display( Name = "Global_Sort_Latest", ResourceType = typeof( GlobalLocalization ) )]
		CreateTime = 1,

		[Display( Name = "Global_Sort_Title", ResourceType = typeof( GlobalLocalization ) )]
		Title = 2
	}
}