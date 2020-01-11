using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dramarr.Scrapers.Tests
{
    [TestClass]
    public class KshowTests
    {
        private Scrapers.MyAsianTv.Manager MATScraper;
        private Scrapers.EstrenosDoramas.Manager ESScraper;
        private Scrapers.Kshow.Manager KSScraper;

        public KshowTests()
        {
            var MATEpisodeUrl = $"https://myasiantv.to/drama/<dorama>/download/";
            var MATAllShowsUrl = $"https://myasiantv.to/";
            var MATLatestEpisodesUrl = $"https://myasiantv.to/";
            MATScraper = new Scrapers.MyAsianTv.Manager(MATEpisodeUrl, MATAllShowsUrl, MATLatestEpisodesUrl);

            var ESShowUrl = "https://www.estrenosdoramas.net/";
            ESScraper = new Scrapers.EstrenosDoramas.Manager(ESShowUrl);

            var KSShowUrl = "https://kshow.to/";
            KSScraper = new Scrapers.Kshow.Manager(KSShowUrl);
        }

        [TestMethod]
        public void ShouldGetMetadata()
        {
            var showUrl = "how-do-you-play";
            var metadata = KSScraper.GetMetadata(showUrl);
            Assert.IsNotNull(metadata);
        }

        [TestMethod]
        public void ShouldGetAllShows()
        {
            var shows = KSScraper.GetAllShows();
            Assert.IsNotNull(shows);
        }

        [TestMethod]
        public void ShouldGetLatestShow()
        {
            var episodesString = KSScraper.GetLatestShows();
            Assert.IsNotNull(episodesString);
        }

        [TestMethod]
        public void ShouldGetStatus()
        {
            var showUrl = "how-do-you-play";
            var status = KSScraper.GetStatus(showUrl);
            Assert.IsNotNull(status);
        }
    }
}
