using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.Models.User
{
	public class UserSearchPageModel
	{
		public IPagedList<UserModel> Users { get; set; }

		public UserSearchModel SearchModel { get; set; }
	}
}