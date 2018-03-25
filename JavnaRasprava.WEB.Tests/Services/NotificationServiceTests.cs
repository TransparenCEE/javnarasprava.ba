using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace JavnaRasprava.WEB.Tests.Services
{
    [TestClass]
	public class NotificationServiceTests
    {
		[TestMethod]
		public void GetDirectQuesitonEmailSubject_WhenCalled_ThenReturnsExpectedResult()
		{
            string expectedResult = "Novo pitanje na javnarasprava.ba";

            string actualResutl = BLL.NotificationService.GetDirectQuesitonEmailSubject();

            Assert.AreEqual(expectedResult, actualResutl);
        }
	}
}
