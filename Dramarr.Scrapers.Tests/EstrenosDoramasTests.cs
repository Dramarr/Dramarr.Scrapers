using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dramarr.Scrapers.Tests
{
    [TestClass]
    public class EstrenosDoramasTests
    {
        private Scrapers.EstrenosDoramas.Manager ESScraper;

        public EstrenosDoramasTests()
        {
            var ESShowUrl = "https://www.estrenosdoramas.net/";
            ESScraper = new Scrapers.EstrenosDoramas.Manager(ESShowUrl);
        }

        [TestMethod]
        public void ShouldGetMetadata()
        {
            var showUrl = "/2015/10/noble-my-love.html";
            var metadata = ESScraper.GetMetadata(showUrl);
            Assert.IsNotNull(metadata);
        }

        [TestMethod]
        public void ShouldGetAllShows()
        {
            var shows = ESScraper.GetAllShows();
            Assert.IsNotNull(shows);
        }

        [TestMethod]
        public void ShouldGetLatestShow()
        {
            var episodesString = ESScraper.GetLatestShows();
            Assert.IsNotNull(episodesString);
        }

        [TestMethod]
        public void ShouldGetStatus()
        {
            var showUrl = "/2015/10/noble-my-love.html";
            var status = ESScraper.GetStatus(showUrl);
            Assert.IsNotNull(status);
        }
    }
}
