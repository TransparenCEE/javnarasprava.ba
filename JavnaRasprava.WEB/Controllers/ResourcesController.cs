using JavnaRasprava.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JavnaRasprava.WEB.Controllers
{
	/// <summary>
	/// Serves different resurces for translation purposes
	/// </summary>
	public class ResourcesController : BaseController
	{
		[Route( "Resources/GetResources", Name = "def.Resources.GetResources" )]
		public JsonResult GetResources()
		{
			return Json( new Dictionary<string, string> {
				{"JS_Admin_DeleteSuccess", GlobalLocalization.JS_Admin_DeleteSuccess},
				{"JS_Admin_DeleteError", GlobalLocalization.JS_Admin_DeleteError},
				{"JS_Global_Error", GlobalLocalization.JS_Global_Error},
				{"JS_Law_PleaseChooseOneOfAnswers", GlobalLocalization.JS_Law_PleaseChooseOneOfAnswers},
				{"JS_Law_PleaseProvideYourAnswer", GlobalLocalization.JS_Law_PleaseProvideYourAnswer},
				{"JS_Law_ErrorSavingVote", GlobalLocalization.JS_Law_ErrorSavingVote},
				{"JS_Law_PleaseSelectQuestion", GlobalLocalization.JS_Law_PleaseSelectQuestion},
				{"JS_Law_PleaseSelectRep", GlobalLocalization.JS_Law_PleaseSelectRep},
			}, JsonRequestBehavior.AllowGet );
		}
	}
}