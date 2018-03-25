using JavnaRasprava.WEB.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JavnaRasprava.WEB.Tests.Services
{
	[TestClass]
	public class VotingServiceTests
	{
		[TestMethod]
		public void GetResultsForLaw_Initial_BaseResults()
		{
			var user1 = Helpers.CreateNewUser( Age.MidAge, Education.Visoka, "Zenica", "SDA" );
			var user2 = Helpers.CreateNewUser( Age.Senior, Education.Srednja, "Centar", "SDP" );
		}
	}
}
