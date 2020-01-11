using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dramarr.Scrapers.Tests
{
    [TestClass]
    public class MyAsianTvTests
    {
        private Scrapers.MyAsianTv.Manager MATScraper;

        public MyAsianTvTests()
        {
            var MATEpisodeUrl = $"https://myasiantv.to/drama/<dorama>/download/";
            var MATAllShowsUrl = $"https://myasiantv.to/";
            var MATLatestEpisodesUrl = $"https://myasiantv.to/";
            MATScraper = new Scrapers.MyAsianTv.Manager(MATEpisodeUrl, MATAllShowsUrl, MATLatestEpisodesUrl);
        }

        [TestMethod]
        public void ShouldGetMetadata()
        {
            var showUrl = "crash-landing-on-you";
            var metadata = MATScraper.GetMetadata(showUrl);
            Assert.IsNotNull(metadata);
        }

        [TestMethod]
        public void ShouldGetAllShows()
        {
            var shows = MATScraper.GetAllShows();
            Assert.IsNotNull(shows);
        }

        [TestMethod]
        public void ShouldGetLatestShow()
        {
            var episodesString = MATScraper.GetLatestShows();
            Assert.IsNotNull(episodesString);
        }

        [TestMethod]
        public void ShouldGetStatus()
        {
            var showUrl = "dr-romantic-2";
            var status = MATScraper.GetStatus(showUrl);
            Assert.IsNotNull(status);
        }
    }
}
