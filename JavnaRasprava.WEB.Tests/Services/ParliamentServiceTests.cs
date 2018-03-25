using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

using JavnaRasprava.WEB.BLL;
using JavnaRasprava.WEB.Infrastructure;

namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class ParliamentServiceTests
	{
		[TestMethod]
		public void GetParliaments_All_Returned()
		{
			var response = new ParliamentService().GetParliaments();

			Assert.AreEqual( 3, response.Parliaments.Count );
		}
	}
}
